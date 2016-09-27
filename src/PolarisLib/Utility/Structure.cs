using System;
using System.Runtime.InteropServices;

namespace Polaris.Lib.Utility
{
	public static class Structure
	{
		public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
		{
			var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			var stuff = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
			handle.Free();
			return stuff;
		}

		public static byte[] StructureToByteArray<T>(T structure)
		{
			int size = Marshal.SizeOf(structure);
			byte[] arr = new byte[size];
			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(structure, ptr, true);
			Marshal.Copy(ptr, arr, 0, size);
			Marshal.FreeHGlobal(ptr);
			return arr;
		}
	}
}
