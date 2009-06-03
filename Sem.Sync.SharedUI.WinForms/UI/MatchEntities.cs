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
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    using ViewModel;

    public partial class MatchEntities : Form
    {
        private readonly Matching matching = new Matching();

        public MatchEntities()
        {
            InitializeComponent();
        }

        public List<StdElement> PerformMerge(List<StdElement> sourceList, List<StdElement> targetList, List<StdElement> baselineList)
        {
            // todo: this should be configurable
            this.matching.Profile = ProfileIdentifierType.XingProfileId;

            this.matching.Source = sourceList.ToContacts();
            this.matching.Target = targetList.ToContacts();
            this.matching.BaseLine = baselineList.ToMatchingEntries();

            SetupCandidateGrid(this.dataGridSourceCandidates, this.matching.SourceAsList());
            SetupCandidateGrid(this.dataGridTargetCandidates, this.matching.TargetAsList());
            SetupCandidateGrid(this.dataGridMatches, this.matching.BaselineAsList());

            // cange the target list only if the OK-button has been clicked
            // otherwise we return null to not writy any content to the target
            return this.ShowDialog() != DialogResult.OK 
                ? null 
                : this.matching.BaseLine.ToStdElement();
        }

        private void dataGridTargetCandidatesCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                this.SelectTargetRow(((DataGridView)sender).Rows[e.RowIndex]);
            }
        }

        private void dataGridSourceCandidatesCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                this.SelectSourceRow(((DataGridView)sender).Rows[e.RowIndex]);
            }
        }

        private bool SelectSourceRow(DataGridViewRow row)
        {
            if (row.Index == -1)
            {
                return false;
            }

            var element = ((MatchCandidateView)row.DataBoundItem).Element;
            this.matching.CurrentSourceElement = element;

            this.dataGridSourceDetail.DataSource = this.matching.CurrentSourceProperties();
            this.dataGridSourceDetail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridSourceDetail.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            this.dataGridTargetCandidates.ClearSelection();

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

        private void SelectTargetRow(DataGridViewRow row)
        {
            if (row.Index == -1)
            {
                return;
            }

            var element = ((MatchCandidateView)row.DataBoundItem).Element;
            this.matching.CurrentTargetElement = element;

            dataGridTargetDetail.DataSource = this.matching.CurrentTargetProperties();
            dataGridTargetDetail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridTargetDetail.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void btnMatch_Click(object sender, System.EventArgs e)
        {
            // perform the matching
            this.matching.Match();

            // setup grids for next match
            this.SetupGui();
        }

        private void SetupGui()
        {
            // clear all selections
            this.dataGridSourceCandidates.ClearSelection();
            this.dataGridTargetCandidates.ClearSelection();

            // clear detail grids
            this.dataGridSourceDetail.DataSource = null;
            this.dataGridTargetDetail.DataSource = null;

            // rebind grids
            SetupCandidateGrid(this.dataGridSourceCandidates, this.matching.SourceAsList());
            SetupCandidateGrid(this.dataGridTargetCandidates, this.matching.TargetAsList());
            SetupCandidateGrid(this.dataGridMatches, this.matching.BaselineAsList());

            // enumerate the source grid to find one entry that does
            // have a matching entry in the target grid
            for (var r = 0; r < this.dataGridSourceCandidates.Rows.Count; r++)
            {
                if (this.SelectSourceRow(this.dataGridSourceCandidates.Rows[r]))
                {
                    break;
                }
            }
        }

        private static void SetupCandidateGrid<T>(DataGridView theGrid, List<T> elementList)
        {
            theGrid.AutoGenerateColumns = true;
            theGrid.DataSource = elementList;

            var col = theGrid.Columns["Element"];
            if (col != null)
            {
                col.Visible = false;
            }

            col = theGrid.Columns["ElementMatch"];
            if (col != null)
            {
                col.Visible = false;
            }

            col = theGrid.Columns["BaselineId"];
            if (col != null)
            {
                col.Visible = false;
            }

            col = theGrid.Columns["ContactName"];
            if (col != null)
            {
                col.HeaderText = "Contact Name";
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            col = theGrid.Columns["ContactNameMatch"];
            if (col != null)
            {
                col.HeaderText = "matched to";
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            col = theGrid.Columns[theGrid.Columns.Count - 1];
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnUnMatch_Click(object sender, System.EventArgs e)
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

        private void btnFinished_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
