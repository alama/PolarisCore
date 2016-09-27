using System;

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

	public class ShipEntry
	{
		public UInt32 Number { get; set; }
		public string Name { get; set; } // 16 characters
		public byte[] IP { get; set; } // 4 bytes
		public ShipStatus Status { get; set; }
		public ushort Order { get; set; }
		public UInt32 Unknown { get; set; }
	}
}
