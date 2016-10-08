using System;
using System.Collections.Generic;
using System.Text;

namespace Polaris.Lib.Extensions
{
	public static class StringExtensions
	{
		public static string Combine(this string[] str, int offset = 0)
		{
			return str.Combine(string.Empty, offset);
		}

		public static string Combine(this string[] str, string suffix = "", int offset = 0)
		{
			string result = string.Empty;

			for (int i = offset; i < str.Length; i++)
				result += str[i] + suffix;

			return result;
		}

		public static string TrimComma(this string str)
		{
			return str.Remove(str.Length - 2);
		}

		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			if (string.IsNullOrEmpty(str))
				throw new ArgumentException();

			if (chunkLength < 1)
				throw new ArgumentException();

			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength + i > str.Length)
					chunkLength = str.Length - i;

				yield return str.Substring(i, chunkLength);
			}
		}

		public static byte[] ToByteArray(this string str)
		{
			int numberChars = str.Length;
			byte[] bytes = new byte[numberChars / 2];

			for (var i = 0; i < numberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);

			return bytes;
		}

		public static byte[] ToByteArrayUnicode(this string str)
		{
			UnicodeEncoding uEncoding = new UnicodeEncoding();
			byte[] stringContentBytes = uEncoding.GetBytes(str);
			return stringContentBytes;
		}
	}
}
