// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationshipStatus.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the RelationshipStatus type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// This is a similar (but not identical) to the Facebook point of view of relations people can
    /// have. It does not cover complex relation ships with other people, but only the status of a single
    /// human. It can be seen technically as a "mating status". Also this does not take any historical 
    /// data into account, so it does not take a divorce into account.
    /// </summary>
    public enum RelationshipStatus
    {
        /// <summary>
        /// The raltionsship status is not known.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The person is known to not be in a relationship, but we cannot
        /// tell if 
        /// </summary>
        Single = 1,

        /// <summary>
        /// The person is known to want a(nother) relation to a person
        /// </summary>
        Searching = 1,

        /// <summary>
        /// The person is in a relationship, so it's expected to not
        /// searching for another relation.
        /// </summary>
        InARelationship = 2,

        /// <summary>
        /// This is a publicly announced relationship to one person with the
        /// publicly announcement of not searching another relation.
        /// </summary>
        Engaged,

        /// <summary>
        /// The person is married.
        /// </summary>
        Married = 3,
    }
}
