using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Polaris.Lib.Extensions;
using Polaris.Lib.Packet.Common;


namespace Polaris.Lib.Packet.Packets
{
	public class PacketBlockHello : PacketBase, IPacketSent
	{
		private const uint PAYLOAD_SIZE = 0xC;

		/// struct PacketStruct
		/// {
		///		uint16 protocolVersion;
		///		uint16 blockCode;
		///		uint8[0x4] unk1;
		/// }

		public ushort ProtocolVersion;
		public ushort BlockCode;

		public PacketBlockHello(byte type, byte subType) : base(type, subType)
		{
			Header.flag1 = 0x00;
			Header.flag2 = 0x00;
		}

		public void ConstructPayload()
		{
			Header.size = (uint)HeaderSize + PAYLOAD_SIZE;
			Payload = new byte[Header.size - HeaderSize];

			using (BinaryWriter bw = new BinaryWriter(new MemoryStream(Payload)))
			{
				bw.Write(ProtocolVersion);
				bw.Write(BlockCode);
				bw.Write(new byte[4]);
			}
		}

	}
}
