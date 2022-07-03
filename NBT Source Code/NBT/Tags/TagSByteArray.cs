using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagSByteArray : Tag, IEquatable<TagSByteArray>
	{

		public sbyte[] value;

		public TagSByteArray() : this(Array.Empty<sbyte>())
		{
		}

		public TagSByteArray(sbyte[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagSByteArray(Stream stream) : this(Array.Empty<sbyte>())
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
				return TagTypes.TagSByteArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagSByteArray.ReadSByteArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagSByteArray.WriteSByteArray(stream, this.value);
		}

		internal static sbyte[] ReadSByteArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			sbyte[] buffer = new sbyte[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagSByte.ReadSByte(stream);
			}
			return buffer;
		}

		internal static void WriteSByteArray(Stream stream, sbyte[] value)
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
					TagSByte.WriteSByte(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagSByteArray(this.value);
		}

		public static explicit operator TagSByteArray(sbyte[] value)
		{
			return new TagSByteArray(value);
		}

		public bool Equals(TagSByteArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagSByteArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagSByteArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<sbyte, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
