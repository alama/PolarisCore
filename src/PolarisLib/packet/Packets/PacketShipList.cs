using System;
using System.IO;
using System.Runtime.InteropServices;

using Polaris.Lib.Packet.Common;

namespace Polaris.Lib.Packet
{
	public class PacketShipList : PacketBase
	{
		public enum ShipStatus : ushort
		{
			Unknown = 0,
			Online,
			Busy,
			Full,
			Offline
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		public struct ShipEntry
		{
			public uint shipNumber;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string name;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] ip;

			public uint zero;
			public ShipStatus status;
			public ushort order;
			public uint unknown;
		}

		
	}
}
