using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagShortArray : Tag, IEquatable<TagShortArray>
	{
		public short[] value;

		public TagShortArray() : this(Array.Empty<short>())
		{
		}

		public TagShortArray(short[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagShortArray(Stream stream) : this(Array.Empty<short>())
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
				return TagTypes.TagShortArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagShortArray.ReadShortArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagShortArray.WriteShortArray(stream, this.value);
		}

		internal static short[] ReadShortArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			short[] buffer = new short[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagShort.ReadShort(stream);
			}
			return buffer;
		}

		internal static void WriteShortArray(Stream stream, short[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			if (value == null)
			{
				TagInt.WriteInt(stream, 0);
			}
			else
			{
				TagInt.WriteInt(stream, value.Length);
				for (int i = 0; i < value.Length; i++)
				{
					TagShort.WriteShort(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagShortArray(this.value);
		}

		public static explicit operator TagShortArray(short[] value)
		{
			return new TagShortArray(value);
		}

		public bool Equals(TagShortArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagShortArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagShortArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<short, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
