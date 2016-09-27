using System;
using System.Collections.Generic;

namespace Polaris.Lib.Packet.Common
{
	public partial class PacketBase
	{
		// TODO: Maybe find a better readonly structure for this, or just use an array of class types with the index equal to the ship list
		public static readonly Dictionary<ushort, Type> PacketMap = new Dictionary<ushort, Type>()
			{
				{ 0x113D, typeof(PacketShipList) }
			};
	}
}
