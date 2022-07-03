using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagUIntArray : Tag, IEquatable<TagUIntArray>
	{
		public uint[] value;

		public TagUIntArray() : this(Array.Empty<uint>())
		{
		}

		public TagUIntArray(uint[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagUIntArray(Stream stream) : this(Array.Empty<uint>())
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
				return TagTypes.TagUIntArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagUIntArray.ReadUIntegerArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagUIntArray.WriteUIntegerArray(stream, this.value);
		}

		internal static uint[] ReadUIntegerArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			uint[] buffer = new uint[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagUInt.ReadUInt(stream);
			}
			return buffer;
		}

		internal static void WriteUIntegerArray(Stream stream, uint[] value)
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
					TagUInt.WriteUInt(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagUIntArray(this.value);
		}

		public static explicit operator TagUIntArray(uint[] value)
		{
			return new TagUIntArray(value);
		}

		public bool Equals(TagUIntArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagUIntArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagUIntArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<uint, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
