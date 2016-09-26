using System.IO;

namespace Polaris.Lib.Packet
{
	public class PacketShipList : Packet, ISendPacket
	{
		public byte[] unk1; //0x8-0x63, 0x5C
		public byte[] IPAddress; //0x64-0x67, 0x4
		public ushort port; //0x68

		public PacketShipList() : base()
		{
			unk1 = new byte[0x5C];
		}

		public byte[] ConstructPacket(PacketHeader header)
		{
			this.header = header;
			this.data = new byte[this.header.size - PacketHeader.HeaderSize];

			using (BinaryWriter writer = new BinaryWriter(new MemoryStream(this.data)))
			{
				writer.Write(unk1);
				writer.Write(IPAddress);
				writer.Write(port);
			}

			BuildPacket();
			return this.packet;
		}
	}
}
