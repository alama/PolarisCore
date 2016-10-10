using System;

using Polaris.Lib.Packet.Common;

namespace Polaris.Lib.Packet.Packets
{
	public class PacketAuth : PacketBase, IPacketRecv
    {

		public PacketAuth(byte[] pkt) : base(pkt)
		{
		}

		public void ParsePacket()
		{
			throw new NotImplementedException();
		}

    }
}
