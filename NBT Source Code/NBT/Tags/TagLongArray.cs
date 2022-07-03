using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagLongArray : Tag, IEquatable<TagLongArray>
	{
		public long[] value;

		public TagLongArray() : this(Array.Empty<long>())
		{
		}

		public TagLongArray(long[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagLongArray(Stream stream) : this(Array.Empty<long>())
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
				return TagTypes.TagLongArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagLongArray.ReadLongArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagLongArray.WriteLongArray(stream, this.value);
		}

		internal static long[] ReadLongArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			long[] buffer = new long[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagLong.ReadLong(stream);
			}
			return buffer;
		}

		internal static void WriteLongArray(Stream stream, long[] value)
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
					TagLong.WriteLong(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagLongArray(this.value);
		}

		public static explicit operator TagLongArray(long[] value)
		{
			return new TagLongArray(value);
		}

		public bool Equals(TagLongArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagLongArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagLongArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<long, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
