using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagByteArray : Tag, IEquatable<TagByteArray>
	{
		public byte[] value;

		public TagByteArray() : this(Array.Empty<byte>())
		{
		}

		public TagByteArray(byte[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagByteArray(Stream stream) : this(Array.Empty<byte>())
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
				return TagTypes.TagByteArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagByteArray.ReadByteArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagByteArray.WriteByteArray(stream, this.value);
		}

		internal static byte[] ReadByteArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			byte[] buffer = new byte[TagInt.ReadInt(stream)];
			if (stream.ReadAll(buffer, 0, buffer.Length) != buffer.Length)
			{
				NBT_EndOfStreamException.Throw();
			}
			return buffer;
		}

		internal static void WriteByteArray(Stream stream, byte[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			if (value == null)
			{
				TagInt.WriteInt(stream, 0);
			}
			else
			{
				TagInt.WriteInt(stream, value.Length);
				stream.Write(value, 0, value.Length);
			}
		}

		public override object Clone()
		{
			return new TagByteArray(this.value);
		}

		public static explicit operator TagByteArray(byte[] value)
		{
			return new TagByteArray(value);
		}

		public bool Equals(TagByteArray other)
		{
			return other != null && other.value.AreSame(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagByteArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagByteArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 16 bytes only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			var sp = value.AsSpan();
			hash.AddBytes(sp[..Math.Min(16, sp.Length)]);
			return hash.ToHashCode();
		}
	}
}
