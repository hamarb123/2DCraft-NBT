using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace NBT.Tags
{
	public sealed class TagList : Tag, IList<Tag>, ICollection<Tag>, IEnumerable<Tag>, IEnumerable, IEquatable<TagList>
	{
		/// <summary>
		/// Don't modify this list directly.
		/// </summary>
		public List<Tag> value;
		private byte typeOfList;

		public TagList(byte idTagType)
		{
			this.value = new List<Tag>();
			this.typeOfList = idTagType;
		}

		internal TagList(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.value = new List<Tag>();
			this.readTag(stream);
		}

		public override Tag this[int index]
		{
			get
			{
				return this.value[index];
			}
			set
			{
				NBT_InvalidArgumentNullException.ThrowIfNull(value);
				if (value.tagID != this.typeOfList)
				{
					NBT_InvalidArgumentException.Throw("TagType doesn't match.");
				}
				this.value[index] = value;
			}
		}

		public byte Type
		{
			get
			{
				return this.typeOfList;
			}
			set
			{
				if (this.Count > 0)
				{
					NBT_InvalidArgumentException.Throw("Clear the TagList before changing its TagType.");
				}
				this.typeOfList = value;
			}
		}

		public void Add(Tag item)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(item);
			if (item.tagID != this.typeOfList)
			{
				NBT_InvalidArgumentException.Throw("TagType doesn't match.");
			}
			if (this.value.Count == int.MaxValue)
			{
				NBT_Exception.Throw("List is in the limit.");
			}
			this.value.Add(item);
		}

		public void AddRange(IEnumerable<Tag> items)
		{
			foreach (Tag tag in items)
			{
				this.Add(tag);
			}
		}

		public List<Tag>.Enumerator GetEnumerator()
		{
			return this.value.GetEnumerator();
		}

		IEnumerator<Tag> IEnumerable<Tag>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Clear()
		{
			this.value.Clear();
		}

		public bool Contains(Tag item)
		{
			return this.value.Contains(item);
		}

		public void CopyTo(Tag[] array, int arrayIndex)
		{
			this.value.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				return this.value.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Remove(Tag item)
		{
			return this.value.Remove(item);
		}

		public int IndexOf(Tag item)
		{
			return this.value.IndexOf(item);
		}

		public void Insert(int index, Tag item)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(item);
			if (item.tagID != this.typeOfList)
			{
				NBT_InvalidArgumentException.Throw("TagType doesn't match.");
			}
			this.value.Insert(index, item);
		}

		public void InsertRange(int index, IEnumerable<Tag> items)
		{
			foreach (Tag tag in items)
			{
				this.Add(tag);
				index++;
			}
		}

		public void Move(Tag item, int index)
		{
			this.Remove(item);
			this.Insert(index, item);
		}

		public void Move(int fromIndex, int toIndex)
		{
			Tag item = this[fromIndex];
			this.Remove(item);
			this.Insert(toIndex, item);
		}

		public void RemoveAt(int index)
		{
			this.value.RemoveAt(index);
		}

		public void RemoveRange(int index, int count)
		{
			this.value.RemoveRange(index, count);
		}

		public void Reverse()
		{
			this.value.Reverse();
		}

		public void Reverse(int index, int count)
		{
			this.value.Reverse(index, count);
		}

		public override object ValueProp
		{
			get
			{
				return this.value;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagList;
			}
		}

		public override string ToString()
		{
			return "";
		}

		internal override void readTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.Clear();
			this.typeOfList = TagByte.ReadByte(stream);
			int count = TagInt.ReadInt(stream);
			this.value.Capacity = count;
			for (int i = 0; i < count; i++)
			{
				Tag item = Tag.ReadTag(stream, this.typeOfList);
				if (item == null)
				{
					NBT_Exception.Throw("Unexpected TagEnd.");
				}
				this.Add(item);
			}
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			TagByte.WriteByte(stream, this.typeOfList);
			TagInt.WriteInt(stream, this.Count);
			foreach (Tag tag in this)
			{
				tag.writeTag(stream);
			}
		}

		public override object Clone()
		{
			TagList list = new TagList(this.typeOfList);
			foreach (Tag tag in this)
			{
				list.Add((Tag)tag.Clone());
			}
			return list;
		}

		public static TagList operator +(TagList list, Tag tag)
		{
			list.Add(tag);
			return list;
		}

		public string getNamedTypeOfList()
		{
			return Tag.GetNamedTypeFromId(this.typeOfList);
		}

		public bool Equals(TagList other)
		{
			bool bResult = false;
			try
			{
				if (ReferenceEquals(other, this)) return true;
				if (this.Type == other.Type)
				{
					bResult = this.value.SequenceEqual(other.value);
				}
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
			return other is TagList other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagList other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//use length and first 4 elements only because this should be fast
			var hash = new HashCode();
			hash.Add(value.Count);
			for (int i = 0; i < value.Count && i < 4; i++) hash.Add(value[i]);
			return hash.ToHashCode();
		}
	}
}
