namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Merging;

    /// <summary>
    /// ViewModel of the "Merge Entities" view.
    /// </summary>
    public class MergeEntitiesViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MergeEntitiesViewModel"/> class.
        /// </summary>
        /// <param name="dataToMerge"> The data to be merge. </param>
        public MergeEntitiesViewModel(IEnumerable<MergeConflict> dataToMerge)
        {
            this.MergeList = (from x in dataToMerge
                              select
                                  new MergeView
                                      {
                                          ContactName = GetEntityName(x),
                                          PropertyName = x.PathToProperty,
                                          SourceValue = x.SourcePropertyValue,
                                          TargetValue = x.TargetPropertyValue,
                                          Conflict = x,
                                      }).ToList();
        }

        /// <summary>
        /// Gets or sets the view list for the merging diaqlog
        /// </summary>
        public List<MergeView> MergeList { get; set; }

        /// <summary>
        /// Gets the name of the entity that did cause the specified merge conflict.
        /// </summary>
        /// <param name="mergeConflict"> The merge conflict. </param>
        /// <returns> The get entity name. </returns>
        private static string GetEntityName(MergeConflict mergeConflict)
        {
            var element = mergeConflict.SourceElement ?? mergeConflict.TargetElement ?? mergeConflict.BaselineElement;
            var type = element.GetType().Name;
            switch (type)
            {
                case "StdContact":
                    return ((StdContact)element).GetFullName();

                default:
                    return element.ToSortSimple();
            }
        }
    }
}
