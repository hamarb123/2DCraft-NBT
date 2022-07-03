using System;
using System.IO;
using System.Net;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagIP : Tag, IEquatable<TagIP>
	{
		public IPAddress value;

		public TagIP() : this(new IPAddress(new byte[4] { 127, 0, 0, 1 }))
		{
		}

		public TagIP(IPAddress value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagIP(Stream stream) : this(new IPAddress(new byte[4] { 127, 0, 0, 1 }))
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
				return TagTypes.TagIP;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagIP.ReadIP(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagIP.WriteIP(stream, this.value);
		}

		internal static IPAddress ReadIP(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			return new IPAddress(TagByteArray.ReadByteArray(stream));
		}

		internal static void WriteIP(Stream stream, IPAddress value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			if (value == null)
			{
				TagByteArray.WriteByteArray(stream, new byte[] { 127, 0, 0, 1 });
			}
			else
			{
				TagByteArray.WriteByteArray(stream, value.GetAddressBytes());
			}
		}

		public override object Clone()
		{
			return new TagIP(this.value);
		}

		public static explicit operator TagIP(IPAddress value)
		{
			return new TagIP(value);
		}

		public bool Equals(TagIP other)
		{
			if (other == null) return false;
			if ((other.value == null) ^ (value == null)) return false;
			return value.Equals(other.value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagIP other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagIP other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
