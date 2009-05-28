using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sem.Sync.SyncBase;
using Sem.Sync.SyncBase.DetailData;

namespace Sem.Sync.SharedUI.WinForms.UI
{
    public partial class MatchEntries : Form
    {
        public MatchEntries()
        {
            InitializeComponent();
        }

        public List<StdElement> PerformMerge(List<StdElement> sourceList, List<StdElement> targetList, List<StdElement> baselineList)
        {
            this.dataGridSourceCandidates.AutoGenerateColumns = true;
            this.dataGridSourceCandidates.DataSource = 
                (from x in sourceList
                 select x).ToList();

            this.dataGridTargetCandidates.AutoGenerateColumns = true;
            this.dataGridTargetCandidates.DataSource =
                (from x in targetList
                 select x).ToList();

            this.dataGridMatches.AutoGenerateColumns = true;
            this.dataGridMatches.DataSource =
                (from x in baselineList
                 select x).ToList();

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
    }
}
