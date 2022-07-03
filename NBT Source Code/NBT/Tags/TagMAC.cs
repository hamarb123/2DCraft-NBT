using System;
using System.IO;
using NBT.Exceptions;
using System.Net.NetworkInformation;

namespace NBT.Tags
{
	public sealed class TagMAC : Tag, IEquatable<TagMAC>
	{
		public PhysicalAddress value;

		public TagMAC() : this(new PhysicalAddress(new byte[6] { 0, 0, 0, 0, 0, 0 }))
		{
		}

		public TagMAC(PhysicalAddress value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagMAC(Stream stream) : this(new PhysicalAddress(new byte[6] { 0, 0, 0, 0, 0, 0 }))
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
				return TagTypes.TagMAC;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagMAC.ReadMAC(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagMAC.WriteMAC(stream, this.value);
		}

		internal static PhysicalAddress ReadMAC(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			byte[] buffer = new byte[6];
			if (stream.ReadAll(buffer, 0, buffer.Length) != buffer.Length)
			{
				Helper.ThrowFailedToWriteBytes();
			}
			if (BitConverter.IsLittleEndian == true)
			{
				Array.Reverse(buffer);
			}
			return new PhysicalAddress(buffer);
		}

		internal static void WriteMAC(Stream stream, PhysicalAddress value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			byte[] bytes;
			if (value == null)
			{
				bytes = new byte[] { 0, 0, 0, 0, 0, 0 };
			}
			else
			{
				bytes = value.GetAddressBytes();
			}
			if (BitConverter.IsLittleEndian == true)
			{
				Array.Reverse(bytes);
			}
			stream.Write(bytes, 0, bytes.Length);
		}

		public override object Clone()
		{
			return new TagMAC(this.value);
		}

		public static explicit operator TagMAC(PhysicalAddress value)
		{
			return new TagMAC(value);
		}

		public bool Equals(TagMAC other)
		{
			if (other == null) return false;
			if ((other.value == null) ^ (value == null)) return false;
			return value.Equals(other.value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagMAC other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagMAC other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
