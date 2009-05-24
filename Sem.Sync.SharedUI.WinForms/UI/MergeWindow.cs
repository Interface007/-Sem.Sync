﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Sem.Sync.SharedUI.WinForms.Tools;
using Sem.Sync.SyncBase;
using Sem.Sync.SyncBase.Interfaces;
using Sem.Sync.SyncBase.Merging;

namespace Sem.Sync.SharedUI.WinForms.UI
{
    public partial class MergeWindow : Form, IMergeConflictResolver
    {
        public MergeWindow()
        {
            InitializeComponent();
        }

        public List<StdElement> PerformMerge(List<MergeConflict> toMerge, List<StdElement> targetList)
        {
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

            SelectCompleteColumn(2);

            // cange the target list only if the OK-button has been clicked
            // otherwise we return null to not writy any content to the target
            if (this.ShowDialog() != DialogResult.OK)
                return null;

            // get the list of solved merge conflicts
            var merge = from y in (List<MergeView>)this.conflictGrid.DataSource
                        select y.Conflict;

            // perform the user selected action
            foreach (var conflict in merge)
            {
                if (conflict.ActionToDo == MergePropertyAction.CopySourceToTarget)
                    SetPropertyValue(
                        (StdContact)
                        (from x in targetList where x.Id == conflict.TargetElement.Id select x).FirstOrDefault(),
                        conflict.PathToProperty,
                        conflict.SourcePropertyValue);
            }

            return targetList;
        }

        private static void SetPropertyValue(StdContact stdElement, string pathToProperty, string newValue)
        {
            object propObject = stdElement;
            var propType = typeof(StdContact);
            while (pathToProperty.Contains("."))
            {
                var propName = pathToProperty.Substring(0, pathToProperty.IndexOf("."));
                pathToProperty = pathToProperty.Substring(pathToProperty.IndexOf(".") + 1);
                if (string.IsNullOrEmpty(propName)) continue;

                var member = propType.GetProperty(propName);
                propType = member.PropertyType;
                propObject = member.GetValue(propObject, null);
            }

            var memberToSet = propType.GetProperty(pathToProperty);
            if (memberToSet.PropertyType == typeof(System.DateTime))
            {
                memberToSet.SetValue(propObject, System.DateTime.Parse(newValue), null);
                return;
            }
            memberToSet.SetValue(propObject, newValue, null);
        }

        private void conflictGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var columnIndex = e.ColumnIndex;
            if (columnIndex <= 1) return;

            var rowIndex = e.RowIndex;
            if (rowIndex == -1)
            {
                SelectCompleteColumn(columnIndex);
            }
            else
            {
                SelectRowAndColum(columnIndex, rowIndex);
            }
        }

        private void SelectRowAndColum(int columnIndex, int rowIndex)
        {
            var currentRow = this.conflictGrid.Rows[rowIndex];
            foreach (var cell in currentRow.Cells)
            {
                ((DataGridViewTextBoxCell)cell).Style.BackColor = Color.White;
            }

            ((MergeView)currentRow.DataBoundItem).Conflict.ActionToDo =
                columnIndex == 2
                    ?
                        MergePropertyAction.CopySourceToTarget
                    :
                        MergePropertyAction.KeepCurrentTarget;

            this.Text = rowIndex + " " + columnIndex;

            var currentCell = (DataGridViewTextBoxCell)currentRow.Cells[columnIndex];
            currentCell.Style.BackColor = Color.PaleGreen;
        }

        private void SelectCompleteColumn(int columnIndex)
        {
            foreach (var row in this.conflictGrid.Rows)
            {
                foreach (var cell in ((DataGridViewRow)row).Cells)
                {
                    ((DataGridViewTextBoxCell)cell).Style.BackColor = Color.White;
                }
                (((DataGridViewRow)row).Cells[columnIndex]).Style.BackColor = Color.PaleGreen;
                ((MergeView)((DataGridViewRow)row).DataBoundItem).Conflict.ActionToDo =
                    columnIndex == 2
                        ?
                            MergePropertyAction.CopySourceToTarget
                        :
                            MergePropertyAction.KeepCurrentTarget;
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}