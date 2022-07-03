using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagUShortArray : Tag, IEquatable<TagUShortArray>
	{
		public ushort[] value;

		public TagUShortArray() : this(Array.Empty<ushort>())
		{
		}

		public TagUShortArray(ushort[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagUShortArray(Stream stream) : this(Array.Empty<ushort>())
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
				return TagTypes.TagUShortArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagUShortArray.ReadUShortArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagUShortArray.WriteUShortArray(stream, this.value);
		}

		internal static ushort[] ReadUShortArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			ushort[] buffer = new ushort[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagUShort.ReadUShort(stream);
			}
			return buffer;
		}

		internal static void WriteUShortArray(Stream stream, ushort[] value)
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
					TagUShort.WriteUShort(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagUShortArray(this.value);
		}

		public static explicit operator TagUShortArray(ushort[] value)
		{
			return new TagUShortArray(value);
		}

		public bool Equals(TagUShortArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagUShortArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagUShortArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<ushort, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
