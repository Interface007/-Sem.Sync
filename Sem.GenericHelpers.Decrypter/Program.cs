// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Decrypter
{
    using System;
    using System.IO;

    using Exceptions;

    /// <summary>
    /// Decrypts all files in the local directory.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main execution method
        /// </summary>
        /// <param name="args"> The args are ignored. </param>
        public static void Main(string[] args)
        {
            ////var key = File.ReadAllText("PrivateKey.xml");
            ////Directory.GetFiles("????-??-??-??-??-??-*.xml").ForEach((x) =>
            ////    {
            ////        ExceptionHandler.Suppress<Action<string>(
            ////            delegate(string y) { File.WriteAllText(y, SimpleCrypto.DecryptString(File.ReadAllText(x), key)); });}
            ////    });
        }
    }
}
