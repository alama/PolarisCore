using System.IO;
using Polaris.Lib.Utility;

namespace Polaris.Lib.Extensions
{
	public static class BinaryWriterExtensions
	{
		public static void WriteStructure<T>(this BinaryWriter writer, T structure)
		{
			writer.Write(Structure.StructureToByteArray(structure));
		}
	}
}
