using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagDateTime : Tag, IEquatable<TagDateTime>, IFormattable, ISpanFormattable
	{
		public DateTime value;

		public TagDateTime() : this(new DateTime(DateTime.Now.Ticks))
		{
		}

		public TagDateTime(DateTime value)
		{
			this.value = value;
		}

		internal TagDateTime(Stream stream): this(new DateTime(DateTime.Now.Ticks))
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
				return TagTypes.TagDateTime;
			}
		}

		public override string ToString()
		{
			return this.value.ToShortDateString();
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

		public string ToNormalString()
		{
			return this.value.ToString();
		}

		public string ToLongDateString()
		{
			return this.value.ToLongDateString();
		}

		public string ToLongTimeString()
		{
			return this.value.ToLongTimeString();
		}

		public string ToShortDateString()
		{
			return this.value.ToShortDateString();
		}

		public string ToShortTimeString()
		{
			return this.value.ToShortTimeString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagDateTime.ReadDateTime(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagDateTime.WriteDateTime(stream, this.value);
		}

		internal static DateTime ReadDateTime(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			return new DateTime(TagLong.ReadLong(stream));
		}

		internal static void WriteDateTime(Stream stream, DateTime value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagLong.WriteLong(stream, value.Ticks);
		}

		public override object Clone()
		{
			return new TagDateTime(this.value);
		}

		public static explicit operator TagDateTime(DateTime value)
		{
			return new TagDateTime(value);
		}

		public bool Equals(TagDateTime other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagDateTime other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagDateTime other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
