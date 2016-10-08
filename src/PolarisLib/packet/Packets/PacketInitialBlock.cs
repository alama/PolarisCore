using System;
using System.IO;
using System.Net;

using Polaris.Lib.Extensions;
using Polaris.Lib.Packet.Common;


namespace Polaris.Lib.Packet.Packets
{
	public class PacketInitialBlock : PacketBase, IPacketSent
    {
		private const uint PAYLOAD_SIZE = 0x88;

		public IPAddress BlockAddress;
		public ushort BlockPort;
		public string BlockNameDescription;

		/// struct PacketStruct
		/// {
		/// 	public byte[] Zeros; //0x08 to 0x27, 0x20
		/// 	public byte[] BlockNameDescription; //0x28 to 0x67, 0x40
		/// 	public byte[] IPAddress; //0x68 to 0x6B, 0x4
		/// 	public ushort port; //0x6C to 0x6D, 0x2
		/// 	public byte[] unk1; //0x6E to 0x8F, 0x22
		/// }

		public PacketInitialBlock(byte type, byte subType) : base(type, subType)
		{
			Header.flag1 = 0x00;
			Header.flag2 = 0x00;
		}

		public void ConstructPayload()
		{
			Header.size = (uint)HeaderSize + PAYLOAD_SIZE;
			Payload = new byte[Header.size - HeaderSize];
			byte[] nameDescArray = new byte[0x40];
			Array.Clear(nameDescArray, 0, nameDescArray.Length);
			Array.Copy(BlockNameDescription.ToByteArrayUnicode(), nameDescArray, nameDescArray.Length > BlockNameDescription.ToByteArrayUnicode().Length ? BlockNameDescription.ToByteArrayUnicode().Length : nameDescArray.Length);

			using (BinaryWriter bw = new BinaryWriter(new MemoryStream(Payload)))
			{
				bw.Write(new byte[0x20]);
				bw.Write(nameDescArray);
				bw.Write(BlockAddress.GetAddressBytes());
				bw.Write(BlockPort);
				bw.Write(new byte[0x22]);
			}
		}

	}
}
