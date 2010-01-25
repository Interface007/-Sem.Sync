// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Connect.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Connect type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Obex.Operation
{
    /// <summary>
    /// Represents the connect request
    /// </summary>
    public class Connect : OperationBase
    {
        /// <summary>
        /// Gets OpCode of the connect request.
        /// </summary>
        public override OpCode OpCode
        {
            get
            {
                return OpCode.Connect;
            }
        }

        /// <summary>
        /// Gets PacketLength of this request in bytes.
        /// </summary>
        public override short PacketLength
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// Gets version number of the obex protocol to be used.
        /// </summary>
        public int ObexVersionNumber
        {
            get
            {
                return 13;
            }
        }

        /// <summary>
        /// Gets the flags for this request.
        /// </summary>
        public byte Flags
        { 
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the the largest request that the host can receive.
        /// </summary>
        public short MaxObexPacketLength 
        { 
            get
            {
                return 0x0400;
            } 
        }

    }
}