// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Obex.Console
{
    using System;
    using System.IO.Ports;

    /// <summary>
    /// Test tool to execute some operation
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Test method to execute some operations using the obex client.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            var com = new ObexClient
                          {
                              PortName = "COM7",
                              DataBits = 8,
                              Parity = Parity.None,
                              StopBits = StopBits.One,
                              TransType = ObexClient.TransmissionType.Text,
                              BaudRate = 57600
                          };
            com.test();
            ////com.Connect();

            Console.ReadLine();
        }
    }
}
