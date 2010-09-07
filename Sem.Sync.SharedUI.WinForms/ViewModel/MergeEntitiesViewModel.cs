using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Merging;

    public class MergeEntitiesViewModel
    {
        public MergeEntitiesViewModel(IEnumerable<MergeConflict> toMerge)
        {
            this.MergeList = (from x in toMerge
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
        /// The get entity name.
        /// </summary>
        /// <param name="x"> The x. </param>
        /// <returns> The get entity name. </returns>
        private static string GetEntityName(MergeConflict x)
        {
            var element = x.SourceElement ?? x.TargetElement ?? x.BaselineElement;
            var type = element.GetType().Name;
            switch (type)
            {
                case "StdContact":
                    return ((StdContact)element).GetFullName();

                default:
                    return element.ToSortSimple();
            }
        }

        public List<MergeView> MergeList { get; set; }
    }
}
