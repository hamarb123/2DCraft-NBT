using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagSByte : Tag, IEquatable<TagSByte>, IFormattable, ISpanFormattable
	{
		public SByte value;

		public TagSByte() : this((sbyte)0)
		{
		}

		public TagSByte(sbyte value)
		{
			this.value = value;
		}

		internal TagSByte(Stream stream) : this((sbyte)0)
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
				return TagTypes.TagSByte;
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
			this.value = TagSByte.ReadSByte(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagSByte.WriteSByte(stream, this.value);
		}

		internal static sbyte ReadSByte(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			int num = stream.ReadByte();
			if (num == -1)
			{
				NBT_EndOfStreamException.Throw();
			}
			return (sbyte)num;
		}

		internal static void WriteSByte(Stream stream, sbyte value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			stream.WriteByte((byte)value);
		}

		public override object Clone()
		{
			return new TagSByte(this.value);
		}

		public static explicit operator TagSByte(sbyte value)
		{
			return new TagSByte(value);
		}

		public bool Equals(TagSByte other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagSByte other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagSByte other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
