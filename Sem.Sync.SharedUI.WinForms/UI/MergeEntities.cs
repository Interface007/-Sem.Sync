// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeEntities.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MergeEntities type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using SyncBase;
    using SyncBase.Merging;

    using ViewModel;
    using System.Globalization;
    using System;

    public partial class MergeEntities : Form
    {
        public MergeEntities()
        {
            InitializeComponent();
        }

        public List<StdElement> PerformMerge(List<MergeConflict> toMerge, List<StdElement> targetList)
        {
            if (toMerge.Count == 0)
            {
                return targetList;
            }
            
            this.conflictGrid.AutoGenerateColumns = true;
            this.conflictGrid.DataSource =
                (from x in toMerge
                 select new MergeView
                            {
                                ContactName = ((StdContact)(x.SourceElement ?? x.TargetElement ?? x.BaselineElement)).GetFullName(),
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
            var merge = from y in (List<MergeView>)this.conflictGrid.DataSource
                        select y.Conflict;

            // perform the user selected action
            foreach (var conflict in merge)
            {
                if (conflict.ActionToDo == MergePropertyAction.CopySourceToTarget)
                {
                    var theConflict = conflict;
                    SetPropertyValue(
                        (StdContact)
                        (from x in targetList where x.Id == theConflict.TargetElement.Id select x).FirstOrDefault(),
                        conflict.PathToProperty,
                        conflict.SourcePropertyValue);
                }
            }

            return targetList;
        }

        private static void SetPropertyValue(StdContact stdElement, string pathToProperty, string newValue)
        {
            object propObject = stdElement;
            var propType = typeof(StdContact);
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
                propObject = member.GetValue(propObject, null);
            }

            var memberToSet = propType.GetProperty(pathToProperty);

            // we have to deal with special type data (int, datetime) that need to be
            // converted back from string - there is no automated cast in SetValue.
            var destinationType = memberToSet.PropertyType.Name;
            switch (destinationType)
            {
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

        private void conflictGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void SelectRowAndColum(int columnIndex, int rowIndex)
        {
            var currentRow = this.conflictGrid.Rows[rowIndex];
            foreach (DataGridViewTextBoxCell cell in currentRow.Cells)
            {
                cell.Style.BackColor = Color.White;
            }

            ((MergeView)currentRow.DataBoundItem).Conflict.ActionToDo =
                columnIndex == 2
                    ? MergePropertyAction.CopySourceToTarget
                    : MergePropertyAction.KeepCurrentTarget;

            currentRow.Cells[columnIndex].Style.BackColor = Color.PaleGreen;
        }

        private void SelectCompleteColumn(int columnIndex)
        {
            foreach (DataGridViewRow row in this.conflictGrid.Rows)
            {
                this.SelectRowAndColum(columnIndex, row.Index);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void MergeEntities_Load(object sender, EventArgs e)
        {
            conflictGrid.ColumnHeaderMouseClick += (s, ev) => this.SelectCompleteColumn(ev.ColumnIndex);
        }
    }
}