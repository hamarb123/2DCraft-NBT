using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagDoubleArray : Tag, IEquatable<TagDoubleArray>
	{
		public double[] value;

		public TagDoubleArray() : this(Array.Empty<double>())
		{
		}

		public TagDoubleArray(double[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagDoubleArray(Stream stream) : this(Array.Empty<double>())
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
				return TagTypes.TagDoubleArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagDoubleArray.ReadDoubleArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagDoubleArray.WriteDoubleArray(stream, this.value);
		}

		internal static double[] ReadDoubleArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			double[] buffer = new double[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagDouble.ReadDouble(stream);
			}
			return buffer;
		}

		internal static void WriteDoubleArray(Stream stream, double[] value)
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
					TagDouble.WriteDouble(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagDoubleArray(this.value);
		}

		public static explicit operator TagDoubleArray(double[] value)
		{
			return new TagDoubleArray(value);
		}

		public bool Equals(TagDoubleArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagDoubleArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagDoubleArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = MemoryMarshal.Cast<double, byte>(value.AsSpan());
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
