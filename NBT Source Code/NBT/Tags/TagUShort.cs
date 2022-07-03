using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagUShort : Tag, IEquatable<TagUShort>, IFormattable, ISpanFormattable
	{
		public ushort value;

		public TagUShort() : this((ushort)0)
		{
		}

		public TagUShort(ushort value)
		{
			this.value = value;
		}

		internal TagUShort(Stream stream) : this((ushort)0)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.readTag(stream);
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

		public override byte tagID
		{
			get
			{
				return TagTypes.TagUShort;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		public override string ToString(IFormatProvider provider)
		{
			return this.value.ToString(provider);
		}

		public string ToString(string format)
		{
			return this.value.ToString(format);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return this.value.ToString(format, provider);
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
		{
			return value.TryFormat(destination, out charsWritten, format, provider);
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagUShort.ReadUShort(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagUShort.WriteUShort(stream, this.value);
		}

		internal static ushort ReadUShort(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			Span<byte> buffer = stackalloc byte[2];
			if (stream.ReadAll(buffer) != buffer.Length)
			{
				NBT_EndOfStreamException.Throw();
			}
			if (BitConverter.IsLittleEndian)
			{
				buffer.ReverseOrder();
			}
			return BitConverter.ToUInt16(buffer);
		}

		internal static void WriteUShort(Stream stream, ushort value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			Span<byte> bytes = stackalloc byte[2];
			if (!BitConverter.TryWriteBytes(bytes, value))
			{
				Helper.ThrowFailedToWriteBytes();
			}
			if (BitConverter.IsLittleEndian == true)
			{
				bytes.ReverseOrder();
			}
			stream.Write(bytes);
		}

		public override object Clone()
		{
			return new TagUShort(this.value);
		}

		public static explicit operator TagUShort(ushort value)
		{
			return new TagUShort(value);
		}

		public bool Equals(TagUShort other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagUShort other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagUShort other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
