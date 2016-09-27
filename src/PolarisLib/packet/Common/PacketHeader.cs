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