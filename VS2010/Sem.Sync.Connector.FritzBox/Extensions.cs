// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Extensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox
{
    using System.Collections.Generic;
    using System.Linq;

    using Sem.Sync.Connector.FritzBox.Entities;

    using PhoneNumber = Sem.Sync.SyncBase.DetailData.PhoneNumber;

    internal static class Extensions
    {
        internal static PhoneNumber ExtractPhoneNumber(this List<FritzBox.Entities.PhoneNumber> numbers, PhoneNumberType numberType)
        {
            return
                new PhoneNumber(
                    (from x in numbers
                     where x.DestinationType == numberType
                     orderby x.Priority
                     select x.Number).FirstOrDefault());
        }
    }
}
