using System;
using System.IO;
using NBT.Exceptions;

namespace NBT.Tags
{
	public sealed class TagEnd : Tag
	{

		public TagEnd()
		{

		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagEnd;
			}
		}

		public override string ToString()
		{
			return "";
		}

		internal override void readTag(Stream stream)
		{
			throw new NotImplementedException();
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			stream.WriteByte(TagTypes.TagEnd);
		}

		public override object Clone()
		{
			return new TagEnd();
		}

		public override object ValueProp
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool Equals(TagEnd other)
		{
			return other is not null;
		}

		public override bool Equals(Tag other)
		{
			return other is TagEnd;
		}

		public override bool Equals(object obj)
		{
			return obj is TagEnd other2;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}
