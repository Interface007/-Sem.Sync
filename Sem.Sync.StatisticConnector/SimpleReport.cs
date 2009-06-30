namespace Sem.Sync.StatisticConnector
{
    #region usings

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    #endregion usings

    public class SimpleReport : StdClient
    {
        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Simple Statistic";
            }
        }

        /// <summary>
        /// This client is a write only client, so reading is not supported
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            return result;
        }

        /// <summary>
        /// Writing will write a simple XML with some statistical information.
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var statistic = new SimpleStatisticResult
                {
                    NumberOfElements = elements.Count,
                    PropertyUsage = PropertyUsageCounter.GetPropertyUsage(elements),
                    ValueAnalysis = new ValueAnalysisCounter(elements).GetInfo(),
                };

            SyncTools.SaveToFile(statistic, Path.Combine(clientFolderName, this.FriendlyClientName + ".xml"));
        }

    }

    public class ValueAnalysisCounter
    {
        public decimal PercentageGenderFemale { get; set; }
        public decimal PercentageGenderMale { get; set; }

        public ValueAnalysisCounter(List<StdElement> elements)
        {
            var contacts = elements.ToContacts();
            this.PercentageGenderMale = (decimal)((from x in contacts where x.PersonGender == Gender.Male select x).Count() * 100) / contacts.Count ;
            this.PercentageGenderFemale = (decimal)((from x in contacts where x.PersonGender == Gender.Female select x).Count() * 100) / contacts.Count ;
        }

        public List<KeyValuePair> GetInfo()
        {
            var result = new List<KeyValuePair>();
            result.Add(new KeyValuePair("Male", string.Format(CultureInfo.CurrentCulture, "male: {0:0.00} %", this.PercentageGenderMale)));
            result.Add(new KeyValuePair("Male", string.Format(CultureInfo.CurrentCulture, "female: {0:0.00} %", this.PercentageGenderFemale)));

            return result;
        }
    }
}