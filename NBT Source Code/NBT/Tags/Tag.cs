using System;
using System.IO;
using System.Net;
using System.Drawing;
using NBT.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NBT.Tags
{
	public abstract class Tag : ICloneable, IEquatable<Tag>
	{
		public abstract object ValueProp { get; set; }

		public abstract byte tagID { get; }

		internal abstract void readTag(Stream stream);

		internal abstract void writeTag(Stream stream);

		public abstract object Clone();

		public abstract bool Equals(Tag other);
		public override bool Equals(object obj)
		{
			return obj is Tag other2 && Equals(other2);
		}
		public override int GetHashCode()
		{
			return 0;
		}

		public virtual string ToString(IFormatProvider provider) => ToString();

		public virtual Tag this[string key]
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public virtual Tag this[int index]
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		internal static Tag ReadTag(Stream stream, byte id)
		{
			switch (id)
			{
				case TagTypes.TagEnd:
					return new TagEnd();

				case TagTypes.TagByte:
					return new TagByte(stream);

				case TagTypes.TagShort:
					return new TagShort(stream);

				case TagTypes.TagInt:
					return new TagInt(stream);

				case TagTypes.TagLong:
					return new TagLong(stream);

				case TagTypes.TagFloat:
					return new TagFloat(stream);

				case TagTypes.TagDouble:
					return new TagDouble(stream);

				case TagTypes.TagByteArray:
					return new TagByteArray(stream);

				case TagTypes.TagString:
					return new TagString(stream);

				case TagTypes.TagList:
					return new TagList(stream);

				case TagTypes.TagCompound:
					return new TagCompound(stream);

				case TagTypes.TagIntArray:
					return new TagIntArray(stream);

				case TagTypes.TagSByte:
					return new TagSByte(stream);

				case TagTypes.TagUShort:
					return new TagUShort(stream);

				case TagTypes.TagUInt:
					return new TagUInt(stream);

				case TagTypes.TagULong:
					return new TagULong(stream);

#if !NO_GDI
				case TagTypes.TagImage:
					return new TagImage(stream);
#endif

				case TagTypes.TagIP:
					return new TagIP(stream);

				case TagTypes.TagMAC:
					return new TagMAC(stream);

				case TagTypes.TagShortArray:
					return new TagShortArray(stream);

				case TagTypes.TagDateTime:
					return new TagDateTime(stream);

				case TagTypes.TagTimeSpan:
					return new TagTimeSpan(stream);

				case TagTypes.TagLongArray:
					return new TagLongArray(stream);

				case TagTypes.TagFloatArray:
					return new TagFloatArray(stream);

				case TagTypes.TagDoubleArray:
					return new TagDoubleArray(stream);

				case TagTypes.TagSByteArray:
					return new TagSByteArray(stream);

				case TagTypes.TagUShortArray:
					return new TagUShortArray(stream);

				case TagTypes.TagUIntArray:
					return new TagUIntArray(stream);

				case TagTypes.TagULongArray:
					return new TagULongArray(stream);

#if !NO_GDI
				case TagTypes.TagImageArray:
					return new TagImageArray(stream);
#endif

			}
			NBT_InvalidTagTypeException.Throw();
			return default;
		}

		public static string GetNamedTypeFromId(byte id)
		{
			switch (id)
			{
				case TagTypes.TagEnd:
					return "TagEnd";

				case TagTypes.TagByte:
					return "TagByte";

				case TagTypes.TagShort:
					return "TagShort";

				case TagTypes.TagInt:
					return "TagInt";

				case TagTypes.TagLong:
					return "TagLong";

				case TagTypes.TagFloat:
					return "TagFloat";

				case TagTypes.TagDouble:
					return "TagDouble";

				case TagTypes.TagByteArray:
					return "TagByteArray";

				case TagTypes.TagString:
					return "TagString";

				case TagTypes.TagIntArray:
					return "TagIntArray";

				case TagTypes.TagList:
					return "TagList";

				case TagTypes.TagCompound:
					return "TagCompound";

				case TagTypes.TagSByte:
					return "TagSByte";

				case TagTypes.TagUShort:
					return "TagUShort";

				case TagTypes.TagUInt:
					return "TagUInt";

				case TagTypes.TagULong:
					return "TagULong";

#if !NO_GDI
				case TagTypes.TagImage:
					return "TagImage";
#endif

				case TagTypes.TagIP:
					return "TagIP";

				case TagTypes.TagMAC:
					return "TagMAC";

				case TagTypes.TagShortArray:
					return "TagShortArray";

				case TagTypes.TagDateTime:
					return "TagDateTime";

				case TagTypes.TagTimeSpan:
					return "TagTimeSpan";

				case TagTypes.TagLongArray:
					return "TagLongArray";

				case TagTypes.TagFloatArray:
					return "TagFloatArray";

				case TagTypes.TagDoubleArray:
					return "TagDoubleArray";

				case TagTypes.TagSByteArray:
					return "TagSByteArray";

				case TagTypes.TagUShortArray:
					return "TagUShortArray";

				case TagTypes.TagUIntArray:
					return "TagUIntArray";

				case TagTypes.TagULongArray:
					return "TagULongArray";

#if !NO_GDI
				case TagTypes.TagImageArray:
					return "TagImageArray";
#endif

			}
			NBT_InvalidTagTypeException.Throw("Unknown TagId '" + id + "'.");
			return default;
		}

		//Operadores de conversiones
		public static explicit operator byte(Tag value)
		{
			if (value is TagByte tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagByte");
			return default;
		}
		public static explicit operator short(Tag value)
		{
			if (value is TagShort tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagShort");
			return default;
		}
		public static explicit operator int(Tag value)
		{
			if (value is TagInt tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagInt");
			return default;
		}
		public static explicit operator long(Tag value)
		{
			if (value is TagLong tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagLong");
			return default;
		}
		public static explicit operator float(Tag value)
		{
			if (value is TagFloat tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagFloat");
			return default;
		}
		public static explicit operator double(Tag value)
		{
			if (value is TagDouble tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagDouble");
			return default;
		}
		public static explicit operator byte[](Tag value)
		{
			if (value is TagByteArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagByteArray");
			return default;
		}
		public static explicit operator string(Tag value)
		{
			if (value is TagString tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagString");
			return default;
		}
		public static explicit operator List<Tag>(Tag value)
		{
			if (value is TagList tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagList");
			return default;
		}
		public static explicit operator Dictionary<string, Tag>(Tag value)
		{
			if (value is TagCompound tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagCompound");
			return default;
		}
		public static explicit operator int[](Tag value)
		{
			if (value is TagIntArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagIntArray");
			return default;
		}
		public static explicit operator sbyte(Tag value)
		{
			if (value is TagSByte tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagSByte");
			return default;
		}
		public static explicit operator ushort(Tag value)
		{
			if (value is TagUShort tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagUShort");
			return default;
		}
		public static explicit operator uint(Tag value)
		{
			if (value is TagUInt tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagUInt");
			return default;
		}
		public static explicit operator ulong(Tag value)
		{
			if (value is TagULong tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagULong");
			return default;
		}
#if !NO_GDI
		public static explicit operator Image(Tag value)
		{
			if (value is TagImage tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.Throw("TagImage");
			return default;
		}
#endif
		public static explicit operator IPAddress(Tag value)
		{
			if (value is TagIP tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagIPv4");
			return default;
		}
		public static explicit operator PhysicalAddress(Tag value)
		{
			if (value is TagMAC tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagMAC");
			return default;
		}
		public static explicit operator short[](Tag value)
		{
			if (value is TagShortArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagShortArray");
			return default;
		}
		public static explicit operator DateTime(Tag value)
		{
			if (value is TagDateTime tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagDateTime");
			return default;
		}
		public static explicit operator TimeSpan(Tag value)
		{
			if (value is TagTimeSpan tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagTimeSpan");
			return default;
		}
		public static explicit operator long[](Tag value)
		{
			if (value is TagLongArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagLongArray");
			return default;
		}
		public static explicit operator float[](Tag value)
		{
			if (value is TagFloatArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagFloatArray");
			return default;
		}
		public static explicit operator double[](Tag value)
		{
			if (value is TagDoubleArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagDoubleArray");
			return default;
		}
		public static explicit operator sbyte[](Tag value)
		{
			if (value is TagSByteArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagSByteArray");
			return default;
		}
		public static explicit operator ushort[](Tag value)
		{
			if (value is TagUShortArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagUShortArray");
			return default;
		}
		public static explicit operator uint[](Tag value)
		{
			if (value is TagUIntArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagUIntArray");
			return default;
		}
		public static explicit operator ulong[](Tag value)
		{
			if (value is TagULongArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.ThrowInvalidType("TagULongArray");
			return default;
		}
#if !NO_GDI
		public static explicit operator Image[](Tag value)
		{
			if (value is TagImageArray tag2)
			{
				return tag2.value;
			}
			NBT_InvalidArgumentException.Throw("TagImageArray");
			return default;
		}
#endif
	}
}
