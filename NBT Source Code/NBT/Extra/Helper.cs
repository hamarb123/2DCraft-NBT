using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBT.Exceptions;

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

		public static bool AreSame<T>(this T[] arr1, T[] arr2) where T : IEquatable<T>
		{
			if (arr1 == arr2) return true;
			if (arr1 == null || arr2 == null) return false;

			if (arr1.Length != arr2.Length) return false;
			for (int i = 0; i < arr1.Length; i++)
			{
				var v1 = arr1[i];
				var v2 = arr2[i];
				if ((v1 is null) != (v2 is null)) return false;
				if (v1 is not null && !v1.Equals(v2)) return false;
			}

			return true;
		}

		public static bool AreSameObj(this object[] arr1, object[] arr2)
		{
			if (arr1 == arr2) return true;
			if (arr1 == null || arr2 == null) return false;

			if (arr1.Length != arr2.Length) return false;
			for (int i = 0; i < arr1.Length; i++)
			{
				var v1 = arr1[i];
				var v2 = arr2[i];
				if (v1 != v2) return false;
			}

			return true;
		}

		private class TypeNameHelper<T>
		{
			public static readonly string TypeName;
			static TypeNameHelper()
			{
				TypeName = typeof(T).Name;
			}
		}

		public static void ValuePropHelper<T>(ref T field, object value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			if (value is T tValue)
			{
				field = tValue;
			}
			else
			{
				NBT_InvalidArgumentException.ThrowInvalidType(TypeNameHelper<T>.TypeName);
			}
		}

		public static void ThrowFailedToWriteBytes()
		{
			throw new Exception("Failed to write bytes.");
		}

		public static void ThrowArgumentOutOfRangeException(string message)
		{
			throw new ArgumentOutOfRangeException(message);
		}

		public static void ThrowInvalidOperationException()
		{
			throw new InvalidOperationException();
		}

		public static void ThrowInvalidOperationException(string message)
		{
			throw new InvalidOperationException(message);
		}

		public static void ThrowException(string message)
		{
			throw new Exception(message);
		}

		public static void ThrowEndOfStreamException()
		{
			throw new EndOfStreamException();
		}

		public static void ThrowInvalidDataException()
		{
			throw new InvalidDataException();
		}
	}
}
