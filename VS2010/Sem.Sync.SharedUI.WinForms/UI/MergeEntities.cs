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
    using System.Drawing;
    using System.Windows.Forms;

    using Sem.Sync.SharedUI.WinForms.ViewModel;
    using Sem.Sync.SyncBase.Merging;

    /// <summary>
    /// Dialog that enables the user to resolve merge conflicts by selecting the desired property content.
    /// </summary>
    public partial class MergeEntities : Form
    {
        private MergeEntitiesViewModel DataContext;

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MergeEntities" /> class.
        /// </summary>
        /// <param name="dataContext"></param>
        public MergeEntities(MergeEntitiesViewModel dataContext)
        {
            this.DataContext = dataContext;
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up the dialog and lets the user select the desired content.
        /// </summary>
        /// <returns>The manipulated target list </returns>
        public bool PerformMerge()
        {
            return this.ShowDialog() == DialogResult.OK;
        }

        #endregion

        #region Methods

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
        private void ConflictGridCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var columnIndex = e.ColumnIndex;
            
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
        /// <param name="sender">The sender form. </param>
        /// <param name="e">The empty event arguments. </param>
        private void MergeEntitiesLoad(object sender, EventArgs e)
        {
            this.conflictGrid.AutoGenerateColumns = true;
            this.conflictGrid.DataSource = this.DataContext.MergeList;
            this.conflictGrid.CellEnter += this.ConflictGridCellContentClick;
            this.conflictGrid.ColumnHeaderMouseClick += (s, ev) => this.SelectCompleteColumn(ev.ColumnIndex);

            var col = this.conflictGrid.Columns["Conflict"];
            if (col != null)
            {
                col.Visible = false;
            }

            this.SelectCompleteColumn(4);

            this.btnCancel.Click += (x, y) => this.Close();
            this.btnOk.Click += (x, y) =>
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
        }

        /// <summary>
        /// Iterates through the cells of a column and selects them all.
        /// </summary>
        /// <param name="columnIndex">The column index of the cells to be selected. </param>
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
        /// <param name="columnIndex">The column index of the cell to select. </param>
        /// <param name="rowIndex">The row index of the cell to select. </param>
        private void SelectRowAndColum(int columnIndex, int rowIndex)
        {
            if (columnIndex <= 1)
            {
                return;
            }
            
            var currentRow = this.conflictGrid.Rows[rowIndex];
            foreach (DataGridViewTextBoxCell cell in currentRow.Cells)
            {
                cell.Style.BackColor = Color.White;
            }

            ((MergeView)currentRow.DataBoundItem).Conflict.ActionToDo = columnIndex == 3
                                                                            ? MergePropertyAction.CopySourceToTarget
                                                                            : MergePropertyAction.KeepCurrentTarget;

            currentRow.Cells[columnIndex].Style.BackColor = Color.PaleGreen;
        }

        #endregion

    }
}