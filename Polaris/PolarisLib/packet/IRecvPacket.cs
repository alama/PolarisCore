namespace Polaris.Lib.Packet
{
	/// Packets sent by the client and received by the server
	interface IRecvPacket
	{
		void ParseData(byte[] data);
	}
}
