// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteString.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ByteString type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Obex.Headers
{
    using System.Text;

    public class ByteString : BinaryElement
    {
        private ASCIIEncoding encoder = new ASCIIEncoding();

        public string Content { get; set; }

        public byte[] SerializedContent()
        {
            var content = new byte[this.Content.Length + 4];
            content[0] = 0x40;
            content[1] = (byte)((this.Content.Length & 0xff00) / 0x0100);
            content[2] = (byte)(this.Content.Length & 0xff);

            var i = 3;
            var byteContent = this.encoder.GetBytes(this.Content);

            foreach (var singleByte in byteContent)
            {
                content[i] = singleByte;
                i++;
            }

            return content;
        }
    }
}
