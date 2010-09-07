// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateUserStatus.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   An enumeration of the values that can be returned from <see cref="UserRegistrationService.CreateUser" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SilverContacts.Web
{
    /// <summary>
    /// An enumeration of the values that can be returned from <see cref="UserRegistrationService.CreateUser"/>
    /// </summary>
    public enum CreateUserStatus
    {
        Success = 0,
        InvalidUserName = 1,
        InvalidPassword = 2,
        InvalidQuestion = 3,
        InvalidAnswer = 4,
        InvalidEmail = 5,
        DuplicateUserName = 6,
        DuplicateEmail = 7,
        Failure = 8,
    }
}