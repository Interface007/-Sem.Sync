namespace Sem.Sync.SyncBase
{
    using System.Collections.Generic;

    using DetailData;

    public interface IExtendedReader
    {
        StdElement FillContacts(StdElement contactToFill, List<MatchingEntry> baseline);
    }
}
