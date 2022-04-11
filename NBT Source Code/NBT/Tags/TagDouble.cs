using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagDouble : Tag, IEquatable<TagDouble>
	{
		public double value;

		public TagDouble() : this((double)0)
		{
		}

		public TagDouble(double value)
		{
			this.value = value;
		}

		internal TagDouble(Stream stream) : this((double)0)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
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
				if (value == null)
				{
					throw new NBT_InvalidArgumentNullException();
				}
				if (value.GetType() != typeof(double))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (double)value;
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagDouble;
			}
		}

		public override string toString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			this.value = TagDouble.ReadDouble(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagDouble.WriteDouble(stream, this.value);
		}

		internal static double ReadDouble(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			Span<byte> buffer = stackalloc byte[8];
			if (stream.ReadAll(buffer) != buffer.Length)
			{
				throw new NBT_EndOfStreamException();
			}
			if (BitConverter.IsLittleEndian)
			{
				buffer.ReverseOrder();
			}
			return BitConverter.ToDouble(buffer);
		}

		internal static void WriteDouble(Stream stream, double value)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			Span<byte> bytes = stackalloc byte[8];
			if (!BitConverter.TryWriteBytes(bytes, value))
			{
				throw new Exception("Failed to write bytes.");
			}
			if (BitConverter.IsLittleEndian == true)
			{
				bytes.ReverseOrder();
			}
			stream.Write(bytes);
		}

		public override object Clone()
		{
			return new TagDouble(this.value);
		}

		public static explicit operator TagDouble(double value)
		{
			return new TagDouble(value);
		}

		public override Type getType()
		{
			return typeof(TagDouble);
		}

		public bool Equals(TagDouble other)
		{
			bool bResult = false;
			try
			{
				bResult = this.value.Equals(other.value);
			}
			catch (ArgumentNullException nullEx)
			{
				throw new NBT_InvalidArgumentNullException(nullEx.Message, nullEx.InnerException);
			}
			catch (Exception ex)
			{
				throw new NBT_InvalidArgumentException(ex.Message, ex.InnerException);
			}
			return bResult;
		}

		public override bool Equals(Tag other)
		{
			bool bResult = true;

			if (typeof(TagDouble) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagDouble)other);
			}

			return bResult;
		}
	}
}
