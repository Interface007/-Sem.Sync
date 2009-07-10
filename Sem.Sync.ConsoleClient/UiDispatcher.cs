namespace Sem.Sync.ConsoleClient
{
    using System;
    using System.Collections.Generic;

    using SyncBase;
    using SyncBase.Interfaces;
    using SyncBase.Merging;

    internal class UiDispatcher : IUiInteraction
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserDomain { get; set; }

        /// <summary>
        /// Requests log on credentials from the user and insert them into an object implementing 
        /// the ICredentialAware interface.
        /// </summary>
        /// <param name="client">the object that should get the credentials</param>
        /// <param name="messageForUser">a message that should be displayed to the user</param>
        /// <param name="logOnUserId">a pre-selection for the user part of the credentails</param>
        /// <param name="logOnPassword">a pre-selection for the password part of the credentails</param>
        /// <returns>a value indicating whether the user did click the cancel button</returns>
        public bool AskForLogOnCredentials(ICredentialAware client, string messageForUser, string logOnUserId, string logOnPassword)
        {
            Console.WriteLine(messageForUser);
            
            client.LogOnDomain = GetInfoWithDefault(this.UserDomain, "Please enter the user domain");
            client.LogOnUserId = GetInfoWithDefault(this.UserName, "Please enter the user name");
            client.LogOnPassword = GetInfoWithDefault(this.UserPassword, "Please enter the user password");
            
            return client.LogOnPassword.Length > 0;
        }

        private static string GetInfoWithDefault(string p, string message)
        {
            if (string.IsNullOrEmpty(p))
            {
                Console.WriteLine(message);
                p = Console.ReadLine();
            }
            return p;
        }

        /// <summary>
        /// Requests a confirmation from the user
        /// </summary>
        /// <param name="messageForUser">a message that should be displayed to the user</param>
        /// <param name="title">the title of the message box</param>
        /// <returns>a value indicating whether the user did click the "ok" button</returns>
        public bool AskForConfirm(string messageForUser, string title)
        {
            Console.WriteLine(messageForUser);
            Console.WriteLine("-=> YES");
            return true;
        }

        /// <summary>
        /// Requests a merge action for attributes of conflicting entities
        /// </summary>
        /// <param name="toMerge">the list of merge conflicts to reslove</param>
        /// <param name="targetList">the list of elements that should be changed</param>
        /// <returns></returns>
        public List<StdElement> PerformAttributeMerge(List<MergeConflict> toMerge, List<StdElement> targetList)
        {
            Console.WriteLine("Interactive attribute merge to solve merge conflicts not implemented - skipped");
            return targetList;
        }

        /// <summary>
        /// Requests an entity merge of elements
        /// </summary>
        /// <param name="sourceList">the source entity list</param>
        /// <param name="targetList">the list that will be changed</param>
        /// <param name="baselineList">a baseline list that helps merging</param>
        /// <returns></returns>
        public List<StdElement> PerformEntityMerge(List<StdElement> sourceList, List<StdElement> targetList, List<StdElement> baselineList)
        {
            Console.WriteLine("Interactive entity merge to solve merge conflicts not implemented - skipped");
            return targetList;
        }
    }
}