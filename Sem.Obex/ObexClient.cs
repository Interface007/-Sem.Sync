// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObexClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ObexClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Obex
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO.Ports;
    using System.Windows.Forms;

    using Brecham.Obex;
    using Brecham.Obex.Objects;

    using InTheHand;
    using InTheHand.Net;
    using InTheHand.Windows.Forms;

    /// <summary>
    /// Client class for accessing information via Obex
    /// </summary>
    public class ObexClient
    {
        /// <summary>
        /// enumeration to hold our transmission types
        /// </summary>
        public enum TransmissionType
        {
            /// <summary>
            /// Text transport without encoding
            /// </summary>
            Text,

            /// <summary>
            /// Encoding of binary information using hax-formatting
            /// </summary>
            Hex
        }

        /// <summary>
        /// enumeration to hold our message types
        /// </summary>
        public enum MessageType
        {
            Incoming,
            Outgoing,
            Normal,
            Warning,
            Error
        }

        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public int DataBits { get; set; }
        public string PortName { get; set; }
        public TransmissionType TransType { get; set; }

        private readonly SerialPort _comPort = new SerialPort();
        private ObexClientSession _session;
        private readonly BackgroundWorker _folderLister = new BackgroundWorker();
        private readonly List<string> _result = new List<string>();

        public void test()
        {
            var dialog = new SelectBluetoothDeviceDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var device = dialog.SelectedDevice;
            var uri = new Uri(ObexUri.UriSchemeObexPush + "://" + device.DeviceAddress + "/" + "vcard.vcf");

            var request = new ObexWebRequest(uri);
            request.ReadFile("vcard.vcf");

            var response = (ObexWebResponse) request.GetResponse();
            response.Close();
        }

        private void FolderListerDoWork(object sender, DoWorkEventArgs e)
        {
            var old = DateTime.Now;
            var dr = TimeSpan.FromMilliseconds(200);

            using (var str = this._session.Get(null, ObexConstant.Type.FolderListing))
            {
                var parser = new ObexFolderListingParser(str)
                                 {
                                     IgnoreUnknownAttributeNames = true
                                 };

                ObexFolderListingItem item;

                while ((item = parser.GetNextItem()) != null)
                {
                    if (item is ObexParentFolderItem)
                    {
                        continue;
                    }

                    var filefolderitem = item as ObexFileOrFolderItem;
                    var isfolder = filefolderitem is ObexFolderItem;
                    
                    if (filefolderitem == null)
                    {
                        continue;
                    }

                    var temp = filefolderitem.Name + " - " +
                               FormatSize(filefolderitem.Size, isfolder) + " - " +
                               FormatDate(filefolderitem.Modified) + " - " +
                               FormatDate(filefolderitem.Accessed) + " - " +
                               FormatDate(filefolderitem.Created);

                    this._result.Add(temp);

                    if (old.Add(dr) >= DateTime.Now)
                    {
                        continue;
                    }

                    old = DateTime.Now;
                    //// _folderLister.ReportProgress(0, temp);
                }

                e.Result = this._result;
            }
        } 
        
        private static void FolderListerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            Console.WriteLine("Operation completed");

            var resultList = e.Result as List<string>;

            if (resultList == null)
            {
                return;
            }

            foreach (var line in resultList)
            {
                Console.WriteLine(line);
            }
        }

        public bool Connect()
        {
            this.OpenPort();
            this._session = new ObexClientSession(this._comPort.BaseStream, UInt16.MaxValue);
            this._session.Connect(ObexConstant.Target.SyncML);

            this._folderLister.DoWork += this.FolderListerDoWork;
            this._folderLister.RunWorkerCompleted += FolderListerCompleted;
            this._folderLister.RunWorkerAsync();

            return true;
        }

        public bool Disconnect()
        {
            this._comPort.Close();
            return true;
        }

        /// <summary>
        /// method to display the data to & from the port
        /// on the screen
        /// </summary>
        /// <param name="type">MessageType of the message</param>
        /// <param name="msg">Message to display</param>
        private static void DisplayData(MessageType type, string msg)
        {
            Console.ForegroundColor =
                type == MessageType.Error ? ConsoleColor.Red :
                type == MessageType.Warning ? ConsoleColor.Yellow :
                type == MessageType.Incoming ? ConsoleColor.Cyan :
                type == MessageType.Outgoing ? ConsoleColor.Green :
                ConsoleColor.White;

            Console.WriteLine(msg);
        }

        private void OpenPort()
        {
            try
            {
                // first check if the port is already open
                // if its open then close it
                if (this._comPort.IsOpen)
                {
                    this._comPort.Close();
                }

                // set the properties of our SerialPort Object
                this._comPort.BaudRate = this.BaudRate;
                this._comPort.DataBits = this.DataBits;
                this._comPort.StopBits = this.StopBits;
                this._comPort.Parity = this.Parity;
                this._comPort.PortName = this.PortName;
                this._comPort.Handshake = Handshake.None;

                // now open the port
                this._comPort.Open();

                // display message
                DisplayData(MessageType.Normal, "Port opened at " + DateTime.Now + "\n");
            }
            catch (Exception ex)
            {
                DisplayData(MessageType.Error, ex.Message);
                return;
            }
        }

        private static string FormatDate(DateTime date)
        {
            return 
                date == DateTime.MinValue 
                ? "-" 
                : date.ToString();
        }

        private static string FormatSize(long size, bool isFolder)
        {
            if (isFolder)
            {
                return "-";
            }

            if (size < 1024)
            {
                return size + " B";
            }

            if (size < 1024 * 1024)
            {
                return Math.Round((double)size / 1024, 2) + " KB";
            }

            if (size < 1024 * 1024 * 1024)
            {
                return Math.Round((double)size / 1024 / 1024, 2) + " MB";
            }
            else
            {
                return Math.Round((double)size / 1024 / 1024 / 1024, 2) + " GB";
            }
        }
    }
}
