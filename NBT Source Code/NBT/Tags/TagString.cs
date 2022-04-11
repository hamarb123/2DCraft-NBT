using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagString : Tag, IEquatable<TagString>
	{

		public string value;

		public TagString() : this("")
		{
		}

		public TagString(string value)
		{
			this.value = value;
		}

		internal TagString(Stream stream) : this("")
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			this.readTag(stream);
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagString;
			}
		}

		public override string toString()
		{
			return this.value;
		}

		internal override void readTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			this.value = TagString.ReadString(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagString.WriteString(stream, this.value);
		}

		internal static string ReadString(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			var size = TagUShort.ReadUShort(stream);
			if (size == 0)
			{
				return string.Empty;
			}
			else if (size < 1024)
			{
				Span<byte> allocation = stackalloc byte[size];
				if (stream.ReadAll(allocation) != size)
				{
					throw new NBT_EndOfStreamException();
				}
				return Encoding.UTF8.GetString(allocation);
			}
			else
			{
				byte[] arr = ArrayPool<byte>.Shared.Rent(size);
				try
				{
					if (stream.ReadAll(arr, 0, size) != size)
					{
						throw new NBT_EndOfStreamException();
					}
					return Encoding.UTF8.GetString(arr, 0, size);
				}
				finally
				{
					ArrayPool<byte>.Shared.Return(arr);
				}
			}
		}

		internal static void WriteString(Stream stream, string value)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			if (value == string.Empty) //special case
			{
				TagUShort.WriteUShort(stream, 0);
				return;
			}
			int size = -1;
			var maxSize = Encoding.UTF8.GetMaxByteCount(value.Length);
			if (maxSize > ushort.MaxValue && (size = Encoding.UTF8.GetByteCount(value)) > ushort.MaxValue)
			{
				throw new NBT_InvalidArgumentException("String is too long");
			}
			if (maxSize <= 1024 || (size = Encoding.UTF8.GetByteCount(value)) <= 1024)
			{
				Span<byte> allocation = stackalloc byte[size == -1 ? maxSize : size];
				size = Encoding.UTF8.GetBytes(value, allocation);
				TagUShort.WriteUShort(stream, (ushort)size);
				stream.Write(allocation[..size]);
			}
			else
			{
				byte[] arr = ArrayPool<byte>.Shared.Rent(size == -1 ? maxSize : size);
				try
				{
					size = Encoding.UTF8.GetBytes(value, arr);
					TagUShort.WriteUShort(stream, (ushort)size);
					stream.Write(arr, 0, (ushort)size);
				}
				finally
				{
					ArrayPool<byte>.Shared.Return(arr);
				}
			}
		}

		public override object Clone()
		{
			return new TagString(this.value);
		}

		public static explicit operator TagString(string value)
		{
			return new TagString(value);
		}

		public override Type getType()
		{
			return typeof(TagString);
		}

		public override object ValueProp
		{
			get
			{
				return this.value;
			}
			set
			{
				if (value == null)
				{
					throw new NBT_InvalidArgumentNullException();
				}
				if (value.GetType() != typeof(string))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (string)value;
			}
		}

		public bool Equals(TagString other)
		{
			bool bResult = false;
			try
			{
				bResult = this.value.Equals(other.value);
			}
			catch (ArgumentNullException nullEx)
			{
				throw new NBT_InvalidArgumentNullException(nullEx.Message, nullEx.InnerException);
			}
			catch (Exception ex)
			{
				throw new NBT_InvalidArgumentException(ex.Message, ex.InnerException);
			}
			return bResult;
		}

		public override bool Equals(Tag other)
		{
			bool bResult = true;

			if (typeof(TagString) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagString)other);
			}

			return bResult;
		}
	}
}
