using System.Runtime.InteropServices;

namespace Polaris.Lib.Data
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
		public uint ShipNumber;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string ShipName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] IP;

		public uint zero;
		public ShipStatus Status;
		public ushort Order;
		public uint unk1;
	}
}
