// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationBase.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the OperationBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Obex.Operation
{
    public abstract class OperationBase : BinaryElement
    {
        public abstract OpCode OpCode { get; }

        public abstract short PacketLength { get; }

        public virtual byte[] SerializedContent()
        {
            var content = new byte[3];
            content[0] = (byte)this.OpCode;
            content[1] = (byte)((this.PacketLength & 0xff00) / 0x0100);
            content[2] = (byte)(this.PacketLength & 0xff);

            return content;
        }
    }
}
