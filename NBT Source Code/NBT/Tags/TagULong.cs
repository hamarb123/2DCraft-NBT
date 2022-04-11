using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagULong : Tag, IEquatable<TagULong>
	{
		public ulong value;

		public TagULong() : this((ulong)0)
		{
		}

		public TagULong(ulong value)
		{
			this.value = value;
		}

		internal TagULong(Stream stream) : this((ulong)0)
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
				if (value.GetType() != typeof(ulong))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (ulong)value;
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagULong;
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
			this.value = TagULong.ReadULong(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagULong.WriteULong(stream, this.value);
		}

		internal static ulong ReadULong(Stream stream)
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
			return BitConverter.ToUInt64(buffer);
		}

		internal static void WriteULong(Stream stream, ulong value)
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
			return new TagULong(this.value);
		}

		public static explicit operator TagULong(ulong value)
		{
			return new TagULong(value);
		}

		public override Type getType()
		{
			return typeof(TagULong);
		}

		public bool Equals(TagULong other)
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

			if (typeof(TagULong) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagULong)other);
			}

			return bResult;
		}
	}
}
