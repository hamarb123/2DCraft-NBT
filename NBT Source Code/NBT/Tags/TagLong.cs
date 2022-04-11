using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagLong : Tag, IEquatable<TagLong>
	{
		public long value;

		public TagLong() : this((long)0)
		{
		}

		public TagLong(long value)
		{
			this.value = value;
		}

		internal TagLong(Stream stream) : this((long)0)
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
				if (value.GetType() != typeof(long))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (long)value;
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagLong;
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
			this.value = TagLong.ReadLong(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagLong.WriteLong(stream, this.value);
		}

		internal static long ReadLong(Stream stream)
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
			return BitConverter.ToInt64(buffer);
		}

		internal static void WriteLong(Stream stream, long value)
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
			return new TagLong(this.value);
		}

		public static explicit operator TagLong(long value)
		{
			return new TagLong(value);
		}

		public override Type getType()
		{
			return typeof(TagLong);
		}

		public bool Equals(TagLong other)
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

			if (typeof(TagLong) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagLong)other);
			}

			return bResult;
		}
	}
}
