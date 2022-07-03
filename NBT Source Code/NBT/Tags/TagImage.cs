using System;
using System.IO;
using System.Drawing;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagImage : Tag, IEquatable<TagImage>
	{
		public Image value;

		public TagImage()
		{
		}

		public TagImage(Image value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagImage(Stream stream)
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
				return TagTypes.TagImage;
			}
		}

		public override string ToString()
		{
			return "";
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagImage.ReadImage(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagImage.WriteImage(stream, this.value);
		}

		internal static Image ReadImage(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			byte[] buffer = new byte[TagInt.ReadInt(stream)];
			if (stream.ReadAll(buffer, 0, buffer.Length) != buffer.Length)
			{
				NBT_EndOfStreamException.Throw();
			}
			if (buffer.Length > 0)
			{
				MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length);
				return Image.FromStream(ms);
			}
			else
			{
				return null;
			}
		}

		internal static void WriteImage(Stream stream, Image value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			if (value != null)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Bitmap temporalImage = new Bitmap(value);
					temporalImage.Save(memoryStream, value.RawFormat);
					temporalImage.Dispose();
					byte[] bytes = memoryStream.GetBuffer();
					TagInt.WriteInt(stream, bytes.Length);
					stream.Write(bytes, 0, bytes.Length);
				}
			}
			else
			{
				TagInt.WriteInt(stream, 0);
			}
		}

		public override object Clone()
		{
			return new TagImage(this.value);
		}

		public static explicit operator TagImage(Image value)
		{
			return new TagImage(value);
		}

		public bool Equals(TagImage other)
		{
			return other != null && other.value == value;
		}

		public override bool Equals(Tag other)
		{
			return other is TagImage other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagImage other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
