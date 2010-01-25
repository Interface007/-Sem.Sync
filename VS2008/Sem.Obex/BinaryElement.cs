// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryElement.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the BinaryElement type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Obex
{
    public interface BinaryElement
    {
        byte[] SerializedContent();
    }
}
