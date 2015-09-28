// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   GlobalSuppressions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
        Scope = "member", Target = "Sem.GenericHelpers.ExceptionService.IExceptionService.#GetEncryptionKey()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "Sem.GenericHelpers.ExceptionService.ExceptionService")]
