namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using SyncBase;
    using SyncBase.DetailData;

    using Tools;

    public partial class MatchEntries : Form
    {
        public MatchEntries()
        {
            InitializeComponent();
        }

        public List<StdElement> PerformMerge(List<StdElement> sourceList, List<StdElement> targetList, List<StdElement> baselineList)
        {
            SetupCandidateGrid(dataGridSourceCandidates, sourceList);
            SetupCandidateGrid(dataGridTargetCandidates, targetList);

            this.dataGridMatches.AutoGenerateColumns = true;
            this.dataGridMatches.DataSource =
                (from x in
                     baselineList
                 select new MatchView { ContactName = x.ToString(), Element = x }).ToList();

            // cange the target list only if the OK-button has been clicked
            // otherwise we return null to not writy any content to the target
            if (this.ShowDialog() != DialogResult.OK)
                return null;

            // get the list of solved merge conflicts
            var matchedEntrities = from y in (List<MatchingEntry>)this.dataGridMatches.DataSource
                                   select y;

            // perform the user selected action
            foreach (var match in matchedEntrities)
            {

            }

            return baselineList;
        }

        /// <summary>
        /// Setup the grid for a candidate list
        /// </summary>
        /// <param name="theGrid"></param>
        /// <param name="sourceList"></param>
        private static void SetupCandidateGrid(DataGridView theGrid, IEnumerable<StdElement> sourceList)
        {
            theGrid.AutoGenerateColumns = true;
            theGrid.DataSource =
                (from x in sourceList
                 select
                     new MatchCandidateView
                         {
                             ContactName = x.ToString(),
                             Element = x
                         }).ToList();

            var col = theGrid.Columns["Element"];
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
        }

        private void dataGridTargetCandidatesCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                SelectTargetRow(dataGridTargetCandidates.Rows[e.RowIndex]);
        }

        private void dataGridSourceCandidatesCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1) 
                SelectSourceRow(dataGridSourceCandidates.Rows[e.RowIndex]);
        }

        private void SelectSourceRow(DataGridViewRow row)
        {
            var element = ((MatchCandidateView)row.DataBoundItem).Element;
            dataGridSourceDetail.DataSource = GetPropertyList((StdContact) element);
            dataGridSourceDetail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridSourceDetail.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridTargetCandidates.ClearSelection();

            var autoMatch = (from x in dataGridTargetCandidates.Rows.Cast<DataGridViewRow>()
                            where ((MatchCandidateView)x.DataBoundItem).Element.ToStringSimple() == element.ToStringSimple()
                            select x).FirstOrDefault();

            if (autoMatch != null)
            {
                autoMatch.Selected = true;
                dataGridTargetCandidates.FirstDisplayedScrollingRowIndex = autoMatch.Index;
                SelectTargetRow(autoMatch);
            }
        }

        private void SelectTargetRow(DataGridViewRow row)
        {
            if (row.Index == -1) return;

            var element = ((MatchCandidateView)row.DataBoundItem).Element;
            dataGridTargetDetail.DataSource = GetPropertyList((StdContact)element);
            dataGridTargetDetail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridTargetDetail.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private static List<KeyValuePair> GetPropertyList<T>(T objectToInspect)
        {
            var resultList = new List<KeyValuePair>();

            var members = typeof(T).GetProperties();

            foreach (var item in members)
            {
                var typeName = item.PropertyType.Name;
                if (item.PropertyType.BaseType.FullName == "System.Enum")
                    typeName = "Enum";

                switch (typeName)
                {
                    case "Enum":
                    case "Guid":
                    case "String":
                    case "DateTime":
                    case "Int32":
                        if (item.GetValue(objectToInspect, null) != null)
                            resultList.Add(
                                new KeyValuePair
                                    {
                                        Key = item.Name,
                                        Value = item.GetValue(objectToInspect, null).ToString()
                                    });
                        break;

                    case "Byte[]":
                        break;

                    default:
                        resultList.AddRange(GetPropertyList(item.GetValue(objectToInspect, null)));
                        break;
                }
            }

            return resultList;
        }
    }
}
