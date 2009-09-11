namespace Sem.Sync.GoogleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Google.Contacts;
    using Google.GData.Client;
    using Google.GData.Contacts;
    using Google.GData.Extensions;

    public class GoogleContactGroups
    {
        private readonly ContactsRequest myRequester;
        private readonly Uri myUri;
        private Dictionary<string, Group> cache = new Dictionary<string, Group>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleContactGroups"/> class.
        /// </summary>
        /// <param name="requester"> The contact request object that acts as a requester. </param>
        public GoogleContactGroups(ContactsRequest requester, Uri userUri)
        {
            this.myRequester = requester;
            this.myUri = userUri;
        }

        public Group GetGroupByName(string name)
        {
            if (!this.cache.ContainsKey(name))
            {
                var feed = this.myRequester.GetGroups();
                foreach (var group in feed.Entries)
                {
                    if (!this.cache.ContainsKey(group.Title))
                    {
                        this.cache.Add(group.Title, group);
                    }
                }

                if (!this.cache.ContainsKey(name))
                {
                    var group = this.myRequester.Insert(this.myUri, new Group() { Title = name });
                    this.cache.Add(group.Title, group);
                }
            }

            return this.cache[name];
        }
    }
}
