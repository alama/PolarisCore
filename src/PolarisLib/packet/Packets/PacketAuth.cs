using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Polaris.Lib.Packet.Common;

namespace Polaris.Lib.Packet.Packets
{
    public class PacketAuth : PacketBase, IPacketRecv
    {

		public PacketAuth(byte type, byte subType) : base(type, subType)
		{
			Header.flag1 = 0x04;
			Header.flag2 = 0x00;
		}

		public void ParsePacket()
		{
			throw new NotImplementedException();
		}

    }
}
