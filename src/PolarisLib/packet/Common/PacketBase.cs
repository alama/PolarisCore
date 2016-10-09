using System;
using System.IO;
using System.Runtime.InteropServices;

using Polaris.Lib.Utility;


namespace Polaris.Lib.Packet.Common
{
	public partial class PacketBase
	{
		public static int HeaderSize = Marshal.SizeOf<PacketHeader>();

		public PacketHeader Header = new PacketHeader();
		public byte[] Payload;

		public virtual uint PKT_SUB { get { return 0; } }
		public virtual uint PKT_XOR { get { return 0; } }

		public ushort PacketID { get { return (ushort)((Header.type << 8) | Header.subType); } }

		/// For packets that are received
		public PacketBase(byte[] packet)
		{
			Header.size = BitConverter.ToUInt32(packet, 0);
			Header.type = packet[4];
			Header.subType = packet[5];
			Header.flag1 = packet[6];
			Header.flag2 = packet[7];

			Payload = new byte[Header.size - HeaderSize];

			Array.Copy(packet, HeaderSize, Payload, 0x0, (int)Header.size - HeaderSize);
		}

		/// For packets to be sent
		public PacketBase(PacketHeader Header, byte[] Payload)
		{
			this.Header = Header;
			this.Payload = Payload;
		}

		public PacketBase(byte type, byte subType)
		{
			Header.type = type;
			Header.subType = subType;
		}

		protected PacketBase()
		{
		}

		public static PacketHeader GeneratePacketHeader(uint size, byte type, byte subType, byte flag1, byte flag2)
		{
			PacketHeader header = new PacketHeader();
			header.size = size;
			header.type = type;
			header.subType = subType;
			header.flag1 = flag1;
			header.flag2 = flag2;
			return header;
		}

		public static PacketHeader GetPacketheader(byte[] packet)
		{
			return new PacketHeader
			{
				size = BitConverter.ToUInt32(packet, 0),
				type = packet[4],
				subType = packet[5],
				flag1 = packet[6],
				flag2 = packet[7],
			};
		}

		protected void SetPacketHeader(uint size, byte type, byte subType, byte flag1, byte flag2)
		{
			Header.size = size;
			Header.type = type;
			Header.subType = subType;
			Header.flag1 = flag1;
			Header.flag2 = flag2;
		}

		/// Merge Header and data
		public byte[] Packet()
		{
			byte[] pkt = new byte[Header.size];
			using (BinaryWriter writer = new BinaryWriter(new MemoryStream(pkt)))
			{
				writer.Write(Structure.StructureToByteArray(Header));
				writer.Write(Payload);
			}

			return pkt;
		}

		public uint SubXor(uint num)
		{
			return (num ^ PKT_XOR) - PKT_SUB;
		}

		public uint AddXor(uint num)
		{
			return (num + PKT_SUB) ^ PKT_XOR;
		}

		/// Get PacketID to use with PacketList
		public static ushort GetPacketID(byte[] packet)
		{
			return (ushort)((packet[4] << 8) | packet[5]);
		}

	}
}
