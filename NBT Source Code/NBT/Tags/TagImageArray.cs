using System;
using System.IO;
using System.Linq;
using System.Drawing;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagImageArray : Tag, IEquatable<TagImageArray>
	{
		public Image[] value;

		public TagImageArray() : this(Array.Empty<Image>())
		{
		}

		public TagImageArray(Image[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = value;
		}

		internal TagImageArray(Stream stream) : this(Array.Empty<Image>())
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
				return TagTypes.TagImageArray;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = TagImageArray.ReadImageArray(stream);
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagImageArray.WriteImageArray(stream, this.value);
		}

		internal static Image[] ReadImageArray(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			Image[] buffer = new Image[TagInt.ReadInt(stream)];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = TagImage.ReadImage(stream);
			}
			return buffer;
		}

		internal static void WriteImageArray(Stream stream, Image[] value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			if (value == null)
			{
				TagInt.WriteInt(stream, 0);
			}
			else
			{
				TagInt.WriteInt(stream, value.Length);
				for (int i = 0; i < value.Length; i++)
				{
					TagImage.WriteImage(stream, value[i]);
				}
			}
		}

		public override object Clone()
		{
			return new TagImageArray(this.value);
		}

		public static explicit operator TagImageArray(Image[] value)
		{
			return new TagImageArray(value);
		}

		public bool Equals(TagImageArray other)
		{
			return other != null && other.value.AreSameObj(value);
		}

		public override bool Equals(Tag other)
		{
			return other is TagImageArray other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagImageArray other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 4 elements only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Length);
			for (int i = 0; i < value.Length && i < 4; i++) hash.Add(value[i]);
			return hash.ToHashCode();
		}
	}
}
