using System;
using System.IO;
using System.Runtime.InteropServices;

using Polaris.Lib.Data;
using Polaris.Lib.Extensions;
using Polaris.Lib.Packet.Common;


namespace Polaris.Lib.Packet
{
	public class PacketShipList : PacketBase, IPacketSent
	{
		public PacketShipList(byte type, byte subType) : base(type, subType)
		{
			Header.flag1 = 0x04;
			Header.flag2 = 0x00;
		}

		public ShipEntry[] ships;

		public override uint PKT_SUB { get { return 0x51; } }
		public override uint PKT_XOR { get { return 0xE418; } }

		public void ConstructPayload()
		{
			Header.size = (uint)(HeaderSize + Marshal.SizeOf<ShipEntry>() * ships.Length + 12);
			Payload = new byte[Header.size - HeaderSize];
			using (BinaryWriter bw = new BinaryWriter(new MemoryStream(Payload)))
			{
				bw.Write(AddXor((uint)ships.Length));
				foreach (ShipEntry s in ships)
					bw.WriteStructure(s);

				bw.Write((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
				bw.Write(1);
			}
		}

	}
}
