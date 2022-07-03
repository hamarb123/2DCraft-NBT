using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagByte : Tag, IEquatable<TagByte>, IFormattable, ISpanFormattable
	{
		public byte value;

		public TagByte() : this((byte)0)
		{
		}

		public TagByte(byte value)
		{
			this.value = value;
		}

		internal TagByte(Stream stream) : this((byte)0)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.readTag(stream);
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagByte;
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
			this.value = TagByte.ReadByte(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagByte.WriteByte(stream, this.value);
		}

		internal static byte ReadByte(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			int num = stream.ReadByte();
			if (num == -1)
			{
				NBT_EndOfStreamException.Throw();
			}
			return (byte)num;
		}

		internal static void WriteByte(Stream stream, byte value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			stream.WriteByte(value);
		}

		public override object Clone()
		{
			return new TagByte(this.value);
		}

		public static explicit operator TagByte(byte value)
		{
			return new TagByte(value);
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

		public bool Equals(TagByte other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagByte other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagByte other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
