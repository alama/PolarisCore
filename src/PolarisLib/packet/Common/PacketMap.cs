using Polaris.Lib.Packet.Packets;
using System;
using System.Collections.Generic;

namespace Polaris.Lib.Packet.Common
{
	public partial class PacketBase
	{
		// TODO: Maybe find a better readonly structure for this, or just use an array of class types with the index equal to the packet ID
		public static readonly Dictionary<ushort, Type> PacketMap = new Dictionary<ushort, Type>()
			{
				{ 0x113D, typeof(PacketShipList) },
				{ 0x112C, typeof(PacketInitialBlock) },
				{ 0x1100, typeof(PacketAuth) },
				{ 0x0308, typeof(PacketBlockHello) }
			};
	}
}
