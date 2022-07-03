using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagUInt : Tag, IEquatable<TagUInt>, IFormattable, ISpanFormattable
	{
		public uint value;

		public TagUInt() : this((uint)0)
		{
		}

		public TagUInt(uint value)
		{
			this.value = value;
		}

		internal TagUInt(Stream stream) : this((uint)0)
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
				return TagTypes.TagUInt;
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
			this.value = TagUInt.ReadUInt(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagUInt.WriteUInt(stream, this.value);
		}

		internal static uint ReadUInt(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			Span<byte> buffer = stackalloc byte[4];
			if (stream.ReadAll(buffer) != buffer.Length)
			{
				NBT_EndOfStreamException.Throw();
			}
			if (BitConverter.IsLittleEndian)
			{
				buffer.ReverseOrder();
			}
			return BitConverter.ToUInt32(buffer);
		}

		internal static void WriteUInt(Stream stream, uint value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			Span<byte> bytes = stackalloc byte[4];
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
			return new TagUInt(this.value);
		}

		public static explicit operator TagUInt(uint value)
		{
			return new TagUInt(value);
		}

		public bool Equals(TagUInt other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagUInt other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagUInt other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
