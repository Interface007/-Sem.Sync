// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComPortConnection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ComPortConnection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Obex
{
    using System;
    using System.ComponentModel;
    using System.IO.Ports;

    using Operation;

    public class ComPortConnection : Connection
    {
        /// <summary>
        /// Holds the internal connection of the underlying communication object
        /// </summary>
        private readonly SerialPort comPort = new SerialPort();

        /// <summary>
        /// Internal background worker for handling communication
        /// </summary>
        private readonly BackgroundWorker comPortWorker = new BackgroundWorker();
        
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public int DataBits { get; set; }
        public string PortName { get; set; }

        public void Connect()
        {
            // first check if the port is already open
            // if its open then close it
            if (this.comPort.IsOpen)
            {
                this.comPort.Close();
            }

            // set the properties of our SerialPort Object
            this.comPort.BaudRate = this.BaudRate;
            this.comPort.DataBits = this.DataBits;
            this.comPort.StopBits = this.StopBits;
            this.comPort.Parity = this.Parity;
            this.comPort.PortName = this.PortName;
            this.comPort.Handshake = Handshake.None;

            // now open the port
            this.comPort.Open();

            // now send the connect request

            this.RequestData();
        }

        private void RequestData()
        {
            this.comPortWorker.DoWork += this.SendData;
            ////this.comPortWorker.RunWorkerCompleted += FolderListerCompleted;
            this.comPortWorker.RunWorkerAsync(new Connect());
        }

        private void SendData(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            this.comPort.Close();
        }
    }
}
