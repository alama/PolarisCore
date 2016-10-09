using System;

using Polaris.Lib.Packet.Common;

namespace Polaris.Lib.Packet.Packets
{
	public class PacketServerHandshake : PacketBase, IPacketSent
    {
		public byte[] Blob;

		public PacketServerHandshake(byte type, byte subType) : base(type, subType)
		{
			Header.flag1 = 0x00;
			Header.flag2 = 0x00;
		}

		public void ConstructPayload()
		{
			Header.size = (uint)(HeaderSize + Blob.Length);
			Payload = Blob;
		}
	}
}
