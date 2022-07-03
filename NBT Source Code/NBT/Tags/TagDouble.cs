using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagDouble : Tag, IEquatable<TagDouble>, IFormattable, ISpanFormattable
	{
		public double value;

		public TagDouble() : this((double)0)
		{
		}

		public TagDouble(double value)
		{
			this.value = value;
		}

		internal TagDouble(Stream stream) : this((double)0)
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
				return TagTypes.TagDouble;
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
			this.value = TagDouble.ReadDouble(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagDouble.WriteDouble(stream, this.value);
		}

		internal static double ReadDouble(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			Span<byte> buffer = stackalloc byte[8];
			if (stream.ReadAll(buffer) != buffer.Length)
			{
				NBT_EndOfStreamException.Throw();
			}
			if (BitConverter.IsLittleEndian)
			{
				buffer.ReverseOrder();
			}
			return BitConverter.ToDouble(buffer);
		}

		internal static void WriteDouble(Stream stream, double value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			Span<byte> bytes = stackalloc byte[8];
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
			return new TagDouble(this.value);
		}

		public static explicit operator TagDouble(double value)
		{
			return new TagDouble(value);
		}

		public bool Equals(TagDouble other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagDouble other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagDouble other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
