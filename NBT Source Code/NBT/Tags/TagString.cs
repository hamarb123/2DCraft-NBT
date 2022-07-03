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
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagString(Stream stream) : this("")
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.readTag(stream);
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagString;
			}
		}

		public override string ToString()
		{
			return this.value;
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagString.ReadString(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagString.WriteString(stream, this.value);
		}

		internal static string ReadString(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
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
					NBT_EndOfStreamException.Throw();
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
						NBT_EndOfStreamException.Throw();
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
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			if (value == string.Empty) //special case
			{
				TagUShort.WriteUShort(stream, 0);
				return;
			}
			int size = -1;
			var maxSize = Encoding.UTF8.GetMaxByteCount(value.Length);
			if (maxSize > ushort.MaxValue && (size = Encoding.UTF8.GetByteCount(value)) > ushort.MaxValue)
			{
				NBT_InvalidArgumentException.Throw("String is too long");
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

		public override object ValueProp
		{
			get
			{
				return this.value;
			}
			set
			{
				Helper.ValuePropHelper(ref this.value, value);
			}
		}

		public bool Equals(TagString other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagString other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagString other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
