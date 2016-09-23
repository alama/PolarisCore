namespace Polaris.Lib.Data
{
	public enum Race : ushort
	{
		Human,
		Newman,
		Cast,
		Dewman
	}

	public enum Gender : ushort
	{
		Male,
		Female
	}

	public class LooksParam
	{
		public ushort Height { get; set; }
		public byte[] CharData { get; set; } = new byte[80]; // Figure Data, needs more work
		public ushort AccessoryData1 { get; set; }
		public ushort AccessoryData2 { get; set; }
		public ushort AccessoryData3 { get; set; }
		public ushort AccessoryData4 { get; set; }
		public HSVColor CostumeColor { get; set; }
		public HSVColor MainColor { get; set; }
		public HSVColor Sub1Color { get; set; }
		public HSVColor Sub2Color { get; set; }
		public HSVColor Sub3Color { get; set; }
		public HSVColor EyeColor { get; set; }
		public HSVColor HairColor { get; set; }
		public int ModelID { get; set; }
		public ushort MainParts { get; set; }
		public ushort BodyPaint { get; set; }
		public ushort Emblem { get; set; }
		public ushort EyePattern { get; set; }
		public ushort Eyelashes { get; set; }
		public ushort Eyebrows { get; set; }
		public ushort Face { get; set; }
		public ushort FacePaint1 { get; set; }
		public ushort HairStyle { get; set; }
		public ushort Accessory1 { get; set; }
		public ushort Accessory2 { get; set; }
		public ushort Accessory3 { get; set; }
		public ushort FacePaint2 { get; set; }
		public ushort Arms { get; set; }
		public ushort Legs { get; set; }
		public ushort Accessory4 { get; set; }
		public ushort Costume { get; set; }
		public Race Race { get; set; }
		public Gender Gender { get; set; }
	}
}
