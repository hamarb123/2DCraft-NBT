using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagFloatArray : Tag, IEquatable<TagFloatArray>
	{
		public float[] value;

		public TagFloatArray() : this(Array.Empty<float>())
		{
		}

		public TagFloatArray(float[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagFloatArray(Stream stream) : this(Array.Empty<float>())
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
				return TagTypes.TagFloatArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagFloatArray.ReadFloatArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagFloatArray.WriteFloatArray(stream, this.value);
		}

		internal static float[] ReadFloatArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			float[] buffer = new float[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagFloat.ReadFloat(stream);
			}
			return buffer;
		}

		internal static void WriteFloatArray(Stream stream, float[] value)
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
					TagFloat.WriteFloat(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagFloatArray(this.value);
		}

		public static explicit operator TagFloatArray(float[] value)
		{
			return new TagFloatArray(value);
		}

		public bool Equals(TagFloatArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagFloatArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagFloatArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<float, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
