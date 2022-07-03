using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagTimeSpan : Tag, IEquatable<TagTimeSpan>, IFormattable, ISpanFormattable
	{
		public TimeSpan value;

		public TagTimeSpan() : this(new TimeSpan(DateTime.Now.Ticks))
		{
		}

		public TagTimeSpan(TimeSpan value)
		{
			this.value = value;
		}

		internal TagTimeSpan(Stream stream) : this(new TimeSpan(DateTime.Now.Ticks))
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
				return TagTypes.TagTimeSpan;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		public override string ToString(IFormatProvider provider)
		{
			return this.value.ToString(null, provider);
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
			this.value = TagTimeSpan.ReadTimeSpan(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagTimeSpan.WriteTimeSpan(stream, this.value);
		}

		internal static TimeSpan ReadTimeSpan(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			return new TimeSpan(TagLong.ReadLong(stream));
		}

		internal static void WriteTimeSpan(Stream stream, TimeSpan value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagLong.WriteLong(stream, value.Ticks);
		}

		public override object Clone()
		{
			return new TagTimeSpan(this.value);
		}

		public static explicit operator TagTimeSpan(TimeSpan value)
		{
			return new TagTimeSpan(value);
		}

		public bool Equals(TagTimeSpan other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagTimeSpan other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagTimeSpan other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
