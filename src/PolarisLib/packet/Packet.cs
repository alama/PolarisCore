using System;
using System.IO;

namespace Polaris.Lib.Packet
{
	// TODO: For packets it might be better to use unsafe code and Marshal + packed structures to parse the data out
	public partial class Packet
	{
		public PacketHeader header;
		public byte[] data;
		public byte[] packet { get; protected set; }

		/// For when a packet is received
		public Packet(byte[] packet)
		{
			this.packet = packet;
			this.header = new PacketHeader(packet);
			Array.Copy(packet, 0x8, this.data, 0x0, packet.Length - 0x8);
		}

		/// For when a packet is being sent
		public Packet(PacketHeader header, byte[] data)
		{
			this.header = header;
			this.data = data;
			BuildPacket();
		}

		/// Build the packet given a header and data, usually called when constructing a packet to send
		public void BuildPacket()
		{
			this.packet = new byte[header.size];
			using (BinaryWriter writer = new BinaryWriter(new MemoryStream(this.packet)))
			{
				writer.Write(header.size);
				writer.Write(header.type);
				writer.Write(header.subType);
				writer.Write(header.flag1);
				writer.Write(header.flag2);
				writer.Write(data);
			}
		}

		public Packet()
		{
		}

		/// Get PacketID to use with PacketList
		public ushort GetPacketID()
		{
			return (ushort)((header.type << 8) | header.subType);
		}

	}
}
