using System;

using Polaris.Lib.Packet.Common;

namespace Polaris.Lib.Packet.Packets
{
	public class PacketClientHandshake : PacketBase, IPacketRecv
    {

		public PacketClientHandshake(byte type, byte subType) : base(type, subType)
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
