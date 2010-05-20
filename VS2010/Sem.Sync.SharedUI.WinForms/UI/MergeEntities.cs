// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeEntities.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Dialog that enables the user to resolve merge conflicts by selecting the desired property content.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;

    using Sem.Sync.SharedUI.WinForms.ViewModel;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Merging;

    /// <summary>
    /// Dialog that enables the user to resolve merge conflicts by selecting the desired property content.
    /// </summary>
    public partial class MergeEntities : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MergeEntities" /> class.
        /// </summary>
        public MergeEntities()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up the dialog and lets the user select the desired content.
        /// </summary>
        /// <param name="toMerge">
        /// The list of <see cref="MergeConflict"/> instances to be resolved. 
        /// </param>
        /// <param name="targetList">
        /// The target target elements that should be manipulated according to the users selection. 
        /// </param>
        /// <returns>
        /// The manipulated target list 
        /// </returns>
        public List<StdElement> PerformMerge(List<MergeConflict> toMerge, List<StdElement> targetList)
        {
            if (toMerge.Count == 0)
            {
                return targetList;
            }

            this.conflictGrid.AutoGenerateColumns = true;
            this.conflictGrid.DataSource = (from x in toMerge
                                            select
                                                new MergeView
                                                    {
                                                        ContactName = GetEntityName(x), 
                                                        PropertyName = x.PathToProperty, 
                                                        SourceValue = x.SourcePropertyValue, 
                                                        TargetValue = x.TargetPropertyValue, 
                                                        Conflict = x, 
                                                    }).ToList();

            var col = this.conflictGrid.Columns["Conflict"];
            if (col != null)
            {
                col.Visible = false;
            }

            this.SelectCompleteColumn(2);

            // cange the target list only if the OK-button has been clicked
            // otherwise we return null to not writy any content to the target
            if (this.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            // get the list of solved merge conflicts
            var merge = from y in (List<MergeView>)this.conflictGrid.DataSource select y.Conflict;

            // perform the user selected action
            foreach (var conflict in merge)
            {
                if (conflict.ActionToDo != MergePropertyAction.CopySourceToTarget)
                {
                    continue;
                }

                var theConflict = conflict;
                SetPropertyValue(
                    (from x in targetList where x.Id == theConflict.TargetElement.Id select x).FirstOrDefault(), 
                    conflict.PathToProperty, 
                    conflict.SourcePropertyValue);
            }

            return targetList;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get entity name.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The get entity name.
        /// </returns>
        private static string GetEntityName(MergeConflict x)
        {
            var element = x.SourceElement ?? x.TargetElement ?? x.BaselineElement;
            var type = element.GetType().Name;
            switch (type)
            {
                case "StdContact":
                    return ((StdContact)element).GetFullName();

                default:
                    return element.ToSortSimple();
            }
        }

        /// <summary>
        /// Sets a property inside an object
        /// </summary>
        /// <param name="stdElement">
        /// The <see cref="StdElement"/> with the property to be set. 
        /// </param>
        /// <param name="pathToProperty">
        /// The path to the property to be set property. 
        /// </param>
        /// <param name="newValue">
        /// The new value. 
        /// </param>
        private static void SetPropertyValue(StdElement stdElement, string pathToProperty, string newValue)
        {
            object propObject = stdElement;
            var propType = stdElement.GetType();
            while (pathToProperty.Contains("."))
            {
                var nextSeparator = pathToProperty.IndexOf(".", StringComparison.Ordinal);
                var propName = pathToProperty.Substring(0, nextSeparator);
                pathToProperty = pathToProperty.Substring(nextSeparator + 1);
                if (string.IsNullOrEmpty(propName))
                {
                    continue;
                }

                var member = propType.GetProperty(propName);
                propType = member.PropertyType;

                var destinObject = member.GetValue(propObject, null);
                if (destinObject == null)
                {
                    destinObject = propType.GetConstructor(new Type[0]).Invoke(null);
                    member.SetValue(propObject, destinObject, null);
                }

                propObject = destinObject;
            }

            var memberToSet = propType.GetProperty(pathToProperty);

            // we have to deal with special type data (int, datetime) that need to be
            // converted back from string - there is no automated cast in SetValue.
            var destinationType = memberToSet.PropertyType.Name;
            var destinationBaseType = memberToSet.PropertyType.BaseType;
            if (destinationBaseType == typeof(Enum))
            {
                destinationType = "Enum";
            }

            switch (destinationType)
            {
                case "Enum":
                    memberToSet.SetValue(propObject, Enum.Parse(memberToSet.PropertyType, newValue, true), null);
                    break;

                case "TimeSpan":
                    memberToSet.SetValue(propObject, TimeSpan.Parse(newValue, CultureInfo.InvariantCulture), null);
                    break;

                case "DateTime":
                    memberToSet.SetValue(propObject, DateTime.Parse(newValue, CultureInfo.CurrentCulture), null);
                    break;

                case "List`1":

                    // TODO: Implement setting of List<> from string
                    break;

                case "Int32":
                    memberToSet.SetValue(propObject, Int32.Parse(newValue, CultureInfo.CurrentCulture), null);
                    break;

                default:
                    memberToSet.SetValue(propObject, newValue, null);
                    break;
            }
        }

        /// <summary>
        /// Handels a click of the user to cancel the dialog (abort the merge).
        /// </summary>
        /// <param name="sender">
        /// The sender button control. 
        /// </param>
        /// <param name="e">
        /// The empty event arguments. 
        /// </param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handels a click of the user to accept the changes inside the dialog (perform the merge).
        /// </summary>
        /// <param name="sender">
        /// The sender button control. 
        /// </param>
        /// <param name="e">
        /// The empty event arguments. 
        /// </param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handels the click event from the content grid by selecting the clicked content as the
        ///   desired content for the target entry.
        /// </summary>
        /// <param name="sender">
        /// The sender grid control. 
        /// </param>
        /// <param name="e">
        /// The event arguments containing the click information. 
        /// </param>
        private void ConflictGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var columnIndex = e.ColumnIndex;
            if (columnIndex <= 1)
            {
                return;
            }

            var rowIndex = e.RowIndex;
            if (rowIndex == -1)
            {
                this.SelectCompleteColumn(columnIndex);
            }
            else
            {
                this.SelectRowAndColum(columnIndex, rowIndex);
            }
        }

        /// <summary>
        /// Performs some initialization actions for the dialog loading event.
        /// </summary>
        /// <param name="sender">
        /// The sender form. 
        /// </param>
        /// <param name="e">
        /// The empty event arguments. 
        /// </param>
        private void MergeEntities_Load(object sender, EventArgs e)
        {
            this.conflictGrid.ColumnHeaderMouseClick += (s, ev) => this.SelectCompleteColumn(ev.ColumnIndex);
        }

        /// <summary>
        /// Iterates through the cells of a column and selects them all.
        /// </summary>
        /// <param name="columnIndex">
        /// The column index of the cells to be selected. 
        /// </param>
        private void SelectCompleteColumn(int columnIndex)
        {
            foreach (DataGridViewRow row in this.conflictGrid.Rows)
            {
                this.SelectRowAndColum(columnIndex, row.Index);
            }
        }

        /// <summary>
        /// Performs the UI actions to visualize the selection of a specific cell.
        /// </summary>
        /// <param name="columnIndex">
        /// The column index of the cell to select. 
        /// </param>
        /// <param name="rowIndex">
        /// The row index of the cell to select. 
        /// </param>
        private void SelectRowAndColum(int columnIndex, int rowIndex)
        {
            var currentRow = this.conflictGrid.Rows[rowIndex];
            foreach (DataGridViewTextBoxCell cell in currentRow.Cells)
            {
                cell.Style.BackColor = Color.White;
            }

            ((MergeView)currentRow.DataBoundItem).Conflict.ActionToDo = columnIndex == 2
                                                                            ? MergePropertyAction.CopySourceToTarget
                                                                            : MergePropertyAction.KeepCurrentTarget;

            currentRow.Cells[columnIndex].Style.BackColor = Color.PaleGreen;
        }

        #endregion
    }
}