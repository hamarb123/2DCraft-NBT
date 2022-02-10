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
	}
}
