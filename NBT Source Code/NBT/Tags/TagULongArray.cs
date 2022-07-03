using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagULongArray : Tag, IEquatable<TagULongArray>
	{
		public ulong[] value;

		public TagULongArray() : this(Array.Empty<ulong>())
		{
		}

		public TagULongArray(ulong[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagULongArray(Stream stream) : this(Array.Empty<ulong>())
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
				return TagTypes.TagULongArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagULongArray.ReadULongArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagULongArray.WriteULongArray(stream, this.value);
		}

		internal static ulong[] ReadULongArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			ulong[] buffer = new ulong[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagULong.ReadULong(stream);
			}
			return buffer;
		}

		internal static void WriteULongArray(Stream stream, ulong[] value)
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
					TagULong.WriteULong(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagULongArray(this.value);
		}

		public static explicit operator TagULongArray(ulong[] value)
		{
			return new TagULongArray(value);
		}

		public bool Equals(TagULongArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagULongArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagULongArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<ulong, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
