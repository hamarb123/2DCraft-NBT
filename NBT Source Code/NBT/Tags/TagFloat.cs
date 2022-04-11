using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagFloat : Tag, IEquatable<TagFloat>
	{
		public float value;

		public TagFloat() : this((float)0)
		{
		}

		public TagFloat(float value)
		{
			this.value = value;
		}

		internal TagFloat(Stream stream) : this((float)0)
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
				if (value.GetType() != typeof(float))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (float)value;
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagFloat;
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
			this.value = TagFloat.ReadFloat(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagFloat.WriteFloat(stream, this.value);
		}

		internal static float ReadFloat(Stream stream)
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
			return BitConverter.ToSingle(buffer);
		}

		internal static void WriteFloat(Stream stream, float value)
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
			return new TagFloat(this.value);
		}

		public static explicit operator TagFloat(float value)
		{
			return new TagFloat(value);
		}

		public override Type getType()
		{
			return typeof(TagFloat);
		}

		public bool Equals(TagFloat other)
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

			if (typeof(TagFloat) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagFloat)other);
			}

			return bResult;
		}
	}
}
