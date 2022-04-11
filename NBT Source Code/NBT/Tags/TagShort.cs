using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagShort : Tag, IEquatable<TagShort>
	{
		public short value;

		public TagShort() : this((short)0)
		{
		}

		public TagShort(short value)
		{
			this.value = value;
		}

		internal TagShort(Stream stream) : this((short)0)
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
				if (value.GetType() != typeof(short))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (short)value;
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagShort;
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
			this.value = TagShort.ReadShort(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagShort.WriteShort(stream, this.value);
		}

		internal static short ReadShort(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			Span<byte> buffer = stackalloc byte[2];
			if (stream.ReadAll(buffer) != buffer.Length)
			{
				throw new NBT_EndOfStreamException();
			}
			if (BitConverter.IsLittleEndian)
			{
				buffer.ReverseOrder();
			}
			return BitConverter.ToInt16(buffer);
		}

		internal static void WriteShort(Stream stream, short value)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			Span<byte> bytes = stackalloc byte[2];
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
			return new TagShort(this.value);
		}

		public static explicit operator TagShort(short value)
		{
			return new TagShort(value);
		}

		public override Type getType()
		{
			return typeof(TagShort);
		}

		public bool Equals(TagShort other)
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

			if (typeof(TagShort) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagShort)other);
			}

			return bResult;
		}
	}
}
