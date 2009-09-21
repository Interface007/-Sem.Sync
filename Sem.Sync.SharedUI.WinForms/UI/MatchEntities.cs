// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchEntities.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MatchEntities type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    using ViewModel;

    /// <summary>
    /// This form perfoms an entity merge. The method <see cref="PerformMatch"/> accepts a source, target and baseline list of <see cref="StdElement"/>.
    /// The source is the "variable" list of entities with a pool of contacts that have a stable <see cref="StdContact.PersonalProfileIdentifiers"/> and
    /// unstable <see cref="StdContact.Id"/>. Matched relationships of <see cref="StdContact.PersonalProfileIdentifiers"/> from the source and 
    /// <see cref="StdContact.Id"/> of the target are stored inside the baseline.
    /// </summary>
    public partial class MatchEntities : Form
    {
        /// <summary>
        /// The class providing the "business logic" for merging entities.
        /// </summary>
        private readonly Matching matching = new Matching();

        /// <summary>
        /// The last selected source row of the gui - this will be set after a match has been triggered by the user
        /// </summary>
        private int lastSelectedSourceRow;

        /// <summary>
        /// The last selected target row of the gui - this will be set after a match has been triggered by 
        /// the user if there is no "name match" for the currently selected source entry
        /// </summary>
        private int lastSelectedTargetRow;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchEntities"/> class.
        /// </summary>
        public MatchEntities()
        {
            InitializeComponent();
            dataGridTargetCandidates.CellEnter += (s, e) => this.DataGridCellEnter(s, e, this.SelectTargetRow);
            dataGridSourceCandidates.CellEnter += (s, e) => this.DataGridCellEnter(s, e, this.SelectSourceRow);
        }

        /// <summary>
        /// Lets the user perform a manual matching
        /// </summary>
        /// <param name="sourceList"> The source list. </param>
        /// <param name="targetList"> The target list. </param>
        /// <param name="baselineList"> The baseline list. </param>
        /// <param name="identifierToUse"> The identifier to use. </param>
        /// <returns> a list of matches (the base line list manipulated by the user) </returns>
        public List<StdElement> PerformMatch(List<StdElement> sourceList, List<StdElement> targetList, List<StdElement> baselineList, ProfileIdentifierType identifierToUse)
        {
            if (sourceList.Count == 0)
            {
                return baselineList;
            }

            this.matching.Profile = identifierToUse;
            
            this.matching.Source = sourceList.ToContacts();
            this.matching.Target = targetList.ToContacts();
            this.matching.BaseLine = baselineList.ToMatchingEntries();

            this.SetupGui();

            // cange the target list only if the OK-button has been clicked
            // otherwise we return null to not writy any content to the target
            return 
                this.ShowDialog() != DialogResult.OK 
                ? null 
                : this.matching.BaseLine.ToStdElement();
        }

        /// <summary>
        /// Sets up a grid content from a list of elements
        /// </summary>
        /// <param name="theGrid"> The the grid to be set up. </param>
        /// <param name="elementList"> The list of elements to be set into the grid. </param>
        /// <typeparam name="T"> the type of elements inside <paramref name="elementList"/> </typeparam>
        private static void SetupCandidateGrid<T>(DataGridView theGrid, List<T> elementList)
        {
            theGrid.ClearSelection();

            theGrid.AutoGenerateColumns = true;
            theGrid.DataSource = elementList;

            SetupGridColumn(theGrid, "Element", false, string.Empty);
            SetupGridColumn(theGrid, "ElementMatch", false, string.Empty);
            SetupGridColumn(theGrid, "BaselineId", false, string.Empty);
            SetupGridColumn(theGrid, "ContactName", true, "Contact Name");
            SetupGridColumn(theGrid, "ContactNameMatch", true, "matched to");
        }

        /// <summary>
        /// Performs some setup for the columns
        /// </summary>
        /// <param name="theGrid"> The the grid. </param>
        /// <param name="columnName"> The column name. </param>
        /// <param name="visible"> A value indicating whether the column should be visible. </param>
        /// <param name="headerText"> The header text. </param>
        private static void SetupGridColumn(DataGridView theGrid, string columnName, bool visible, string headerText)
        {
            var col = theGrid.Columns[columnName];
            if (col == null)
            {
                return;
            }

            col.Visible = visible;
            col.HeaderText = headerText;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        /// <summary>
        /// performs the row selection if the cell selection is changed
        /// </summary>
        /// <param name="sender"> The sender control.  </param>
        /// <param name="e"> The event arguments.  </param>
        /// <param name="action"> The method to be invoked. </param>
        private void DataGridCellEnter(object sender, DataGridViewCellEventArgs e, Func<DataGridViewRow, bool> action)
        {
            if (e.RowIndex != -1)
            {
                action(((DataGridView)sender).Rows[e.RowIndex]);
            }
        }

        /// <summary>
        /// Selects a row inside the source grid
        /// </summary>
        /// <param name="row">the row to be selected</param>
        /// <returns>true if there has been selected a target row automatically, too</returns>
        private bool SelectSourceRow(DataGridViewRow row)
        {
            if (row.Index == -1)
            {
                return false;
            }

            var element = ((MatchCandidateView)row.DataBoundItem).Element;
            this.matching.CurrentSourceElement = element;
            this.SourceCardView.Contact = element;

            this.dataGridSourceDetail.DataSource = this.matching.CurrentSourceProperties();
            this.dataGridSourceDetail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridSourceDetail.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            this.dataGridTargetCandidates.ClearSelection();
            this.matching.CurrentTargetElement = null;

            var autoMatch = (from x in this.dataGridTargetCandidates.Rows.Cast<DataGridViewRow>()
                             where ((MatchCandidateView)x.DataBoundItem).Element.ToStringSimple() == element.ToStringSimple()
                             select x).FirstOrDefault();

            if (autoMatch == null)
            {
                return false;
            }

            autoMatch.Selected = true;
            this.dataGridTargetCandidates.FirstDisplayedScrollingRowIndex = autoMatch.Index;
            this.SelectTargetRow(autoMatch);

            return true;
        }

        /// <summary>
        /// Selects a row inside the target candidate grid
        /// </summary>
        /// <param name="row"> The row to be selected.  </param>
        /// <returns> always false. </returns>
        private bool SelectTargetRow(DataGridViewRow row)
        {
            if (row.Index == -1)
            {
                return false;
            }

            var element = ((MatchCandidateView)row.DataBoundItem).Element;
            this.matching.CurrentTargetElement = element;
            this.TargetCardView.Contact = element;

            dataGridTargetDetail.DataSource = this.matching.CurrentTargetProperties();
            dataGridTargetDetail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridTargetDetail.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            return false;
        }

        /// <summary>
        /// Performs a general refresh of the GUI
        /// </summary>
        private void SetupGui()
        {
            // determine display of already matched entities
            this.matching.FilterMatchedEntries = chkMatchedOnly.Checked;

            // rebind grids
            SetupCandidateGrid(this.dataGridMatches, this.matching.BaselineAsList());
            SetupCandidateGrid(this.dataGridTargetCandidates, this.matching.TargetAsList());
            SetupCandidateGrid(this.dataGridSourceCandidates, this.matching.SourceAsList());
            SetupCandidateGrid<object>(this.dataGridSourceDetail, null);
            SetupCandidateGrid<object>(this.dataGridSourceDetail, null);

            // clear detail business cards
            this.SourceCardView.Contact = null;
            this.TargetCardView.Contact = null;

            // if there is a "last selected source row", select it if possible
            if (this.lastSelectedSourceRow > 0 
                && this.lastSelectedSourceRow < this.dataGridSourceCandidates.RowCount 
                && this.dataGridSourceCandidates.RowCount > 1)
            {
                this.dataGridSourceCandidates.Rows[this.lastSelectedSourceRow].Selected = true;
                this.dataGridSourceCandidates.CurrentCell = this.dataGridSourceCandidates.Rows[this.lastSelectedSourceRow].Cells[0];
            }

            // enumerate the source grid to find one entry that does
            // have a matching entry in the target grid
            var found = false;
            for (var r = 0; r < this.dataGridSourceCandidates.Rows.Count; r++)
            {
                if (this.SelectSourceRow(this.dataGridSourceCandidates.Rows[r]))
                {
                    // if we found one, exit this loop
                    found = true;
                    break;
                }
            }

            if (!found && this.lastSelectedTargetRow > 0)
            {
                this.dataGridTargetCandidates.Rows[this.lastSelectedTargetRow].Selected = true;
                this.dataGridTargetCandidates.CurrentCell = this.dataGridTargetCandidates.Rows[this.lastSelectedTargetRow].Cells[0];
            }

            this.btnMatch.Enabled = this.dataGridSourceCandidates.RowCount > 0 &&
                                    this.dataGridTargetCandidates.RowCount > 0;

            this.btnUnMatch.Enabled = this.dataGridMatches.RowCount > 0;
        }

        /// <summary>
        /// Handels the click event of the Match button
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event args parameter. </param>
        private void BtnMatch_Click(object sender, EventArgs e)
        {
            try
            {
                this.lastSelectedSourceRow =
                this.dataGridSourceCandidates.CurrentRow == null
                ? 0
                : this.dataGridSourceCandidates.CurrentRow.Index;

                this.lastSelectedTargetRow =
                    this.dataGridTargetCandidates.CurrentRow == null
                    ? 0
                    : this.dataGridTargetCandidates.CurrentRow.Index;

                // perform the matching
                this.matching.Match();

                // setup grids for next match
                this.SetupGui();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handels the click event of the UnMatch button
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event args parameter. </param>
        private void BtnUnMatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridMatches.SelectedRows.Count <= 0)
                {
                    return;
                }

                // perform the un-match
                this.matching.UnMatch(((MatchView)this.dataGridMatches.SelectedRows[0].DataBoundItem).BaselineId);

                // rebind the gui
                this.SetupGui();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Event handler for pressing the finished button
        /// </summary>
        /// <param name="sender"> The sender object. </param>
        /// <param name="e"> The event parameters. </param>
        private void BtnFinished_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Event handler for changing the ckeckbox
        /// </summary>
        /// <param name="sender"> The sender object. </param>
        /// <param name="e"> The event parameters. </param>
        private void ChkMatchedOnly_CheckedChanged(object sender, EventArgs e)
        {
            // rebind the gui
            this.SetupGui();
        }
    }
}
