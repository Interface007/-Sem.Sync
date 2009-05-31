namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using System;

    public class MatchView : IComparable<MatchView>
    {
        public string ContactName { get; set; }
        public string ContactNameMatch { get; set; }
        public Guid BaselineId { get; set; }

        public override string ToString()
        {
            return ContactName + " is matched to " + ContactNameMatch;
        }

        #region IComparable<MatchView> Members

        public int CompareTo(MatchView other)
        {
            return this.ToString().CompareTo(other.ToString());
        }

        #endregion
    }
}