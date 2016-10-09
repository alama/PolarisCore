using System;

using Polaris.Lib.Packet.Common;

namespace Polaris.Lib.Packet.Packets
{
	public class PacketClientHandshake : PacketBase, IPacketRecv
    {
		public byte[] Blob { get; private set; }

		public PacketClientHandshake(byte[] packet) : base(packet)
		{
			Blob = new byte[0x80];
		}

		public void ParsePacket()
		{
			Array.Copy(Payload, 0, Blob, 0, 0x80);
			Array.Reverse(Blob);
		}

    }
}
