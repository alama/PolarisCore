using System;
using System.Runtime.InteropServices;

namespace Polaris.Lib.Packet.Common
{
	// [Size:4][Type:1][SubType:1][Flag1:1][Flag2:1]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	public struct PacketHeader
	{
		public uint size;
		public byte type;
		public byte subType;
		public byte flag1;
		public byte flag2;
	}
}

/*
  		// Called when receiving a packet
		public PacketHeader(byte[] pkt)
		{
			this.size = BitConverter.ToUInt32(pkt, 0);
			this.type = pkt[4];
			this.subType = pkt[5];
			this.flag1 = pkt[6];
			this.flag2 = pkt[7];
		}

		// Called when constructing a packet to send
		public PacketHeader(uint size, byte type, byte subType, byte flag1, byte flag2)
		{
			this.size = size;
			this.type = type;
			this.subType = subType;
			this.flag1 = flag1;
			this.flag2 = flag2;
		}
 */