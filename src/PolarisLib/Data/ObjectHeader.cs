using System;

namespace Polaris.Lib.Data
{
	public enum EntityType : ushort
	{
		Player = 0x04,
		Map = 0x05,
		Object = 0x06
	}

	public class ObjectHeader
	{
		public UInt32 ID { get; set; }
		public EntityType EntityType { get; set; }
		public ushort Unknown { get; set; }

		public ObjectHeader(uint id, EntityType type)
		{
			this.ID = id;
			this.EntityType = type;
		}
	}
}
