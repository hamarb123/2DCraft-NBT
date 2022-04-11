using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBT.Extra
{
	internal static class Helper
	{
		public static int ReadAll(this Stream s, Span<byte> buffer)
		{
			int read = -1;
			int total = 0;
			while (read != 0)
			{
				read = s.Read(buffer);
				total += read;
				if (total == buffer.Length) return total;
				buffer = buffer[read..];
			}
			return total;
		}

		public static int ReadAll(this Stream s, byte[] buffer, int offset, int count)
		{
			int read = -1;
			int total = 0;
			while (read != 0)
			{
				read = s.Read(buffer, offset + total, count - total);
				total += read;
				if (total == count) return total;
			}
			return total;
		}

		public static void ReverseOrder(this Span<byte> span)
		{
			var limit = span.Length / 2;
			for (int i = 0; i < limit; i++)
			{
				(span[i], span[^(i + 1)]) = (span[^(i + 1)], span[i]);
			}
		}
	}
}
