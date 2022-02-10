﻿using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagInt : Tag, IEquatable<TagInt>
	{
		public int value;

		public TagInt() : this((int)0)
		{
		}

		public TagInt(int value)
		{
			this.value = value;
		}

		internal TagInt(Stream stream) : this((int)0)
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
				if (value.GetType() != typeof(int))
				{
					throw new NBT_InvalidArgumentException();
				}
				this.value = (int)value;
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagInt;
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
			this.value = TagInt.ReadInt(stream);
		}

		internal override void writeTag(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			TagInt.WriteInt(stream, this.value);
		}

		internal static int ReadInt(Stream stream)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			byte[] buffer = new byte[4];
			if (stream.ReadAll(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new NBT_EndOfStreamException();
			}
			if (BitConverter.IsLittleEndian == true)
			{ 
				Array.Reverse(buffer);
			}
			return BitConverter.ToInt32(buffer, 0);
		}

		internal static void WriteInt(Stream stream, int value)
		{
			if (stream == null)
			{
				throw new NBT_InvalidArgumentNullException();
			}
			byte[] bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian == true)
			{ 
				Array.Reverse(bytes);			
			}
			stream.Write(bytes, 0, bytes.Length);
		}

		public override object Clone()
		{
			return new TagInt(this.value);
		}

		public static explicit operator TagInt(int value)
		{
			return new TagInt(value);
		}

		public override Type getType()
		{
			return typeof(TagInt);
		}

		public bool Equals(TagInt other)
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

			if (typeof(TagInt) != other.getType())
			{
				bResult = false;
			}
			else
			{
				bResult = this.Equals((TagInt)other);
			}

			return bResult;
		}
	}
}
