using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagUInt : Tag, IEquatable<TagUInt>
	{
		public uint value;

		public TagUInt() : this((uint)0)
		{
		}

		public TagUInt(uint value)
		{
			this.value = value;
		}

		internal TagUInt(Stream stream) : this((uint)0)
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
				if (value.GetType() != typeof(uint))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (uint)value;
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagUInt;
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
			this.value = TagUInt.ReadUInt(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagUInt.WriteUInt(stream, this.value);
		}

		internal static uint ReadUInt(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			Span<byte> buffer = stackalloc byte[4];
			if (stream.ReadAll(buffer) != buffer.Length)
			{
				throw new NBT_EndOfStreamException();
			}
			if (BitConverter.IsLittleEndian)
			{
				buffer.ReverseOrder();
			}
			return BitConverter.ToUInt32(buffer);
		}

		internal static void WriteUInt(Stream stream, uint value)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			Span<byte> bytes = stackalloc byte[4];
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
			return new TagUInt(this.value);
		}

		public static explicit operator TagUInt(uint value)
		{
			return new TagUInt(value);
		}

		public override Type getType()
		{
			return typeof(TagUInt);
		}

		public bool Equals(TagUInt other)
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

			if (typeof(TagUInt) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagUInt)other);
			}

			return bResult;
		}
	}
}
