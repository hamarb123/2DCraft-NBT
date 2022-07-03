using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagIntArray : Tag, IEquatable<TagIntArray>
	{
		public int[] value;

		public TagIntArray() : this(Array.Empty<int>())
		{
		}

		public TagIntArray(int[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagIntArray(Stream stream) : this(Array.Empty<int>())
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
				return TagTypes.TagIntArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagIntArray.ReadIntegerArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagIntArray.WriteIntegerArray(stream, this.value);
		}

		internal static int[] ReadIntegerArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			int[] buffer = new int[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagInt.ReadInt(stream);
			}
			return buffer;
		}

		internal static void WriteIntegerArray(Stream stream, int[] value)
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
					TagInt.WriteInt(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagIntArray(this.value);
		}

		public static explicit operator TagIntArray(int[] value)
		{
			return new TagIntArray(value);
		}

		public bool Equals(TagIntArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagIntArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagIntArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<int, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
