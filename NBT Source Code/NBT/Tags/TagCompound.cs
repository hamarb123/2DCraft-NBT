using System;
using System.IO;
using System.Linq;
using System.Text;
using NBT.Exceptions;
using System.Collections;
using System.Collections.Generic;

namespace NBT.Tags
{
	public sealed class TagCompound : Tag, IEnumerable<KeyValuePair<string, Tag>>, IEnumerable, IEquatable<TagCompound>, IDictionary<string, Tag>
	{
		/// <summary>
		/// Don't modify this dictionary directly.
		/// </summary>
		public Dictionary<string, Tag> value;

		public TagCompound()
		{
			this.value = new Dictionary<string, Tag>();
		}

		internal TagCompound(Stream stream) : this()
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			this.readTag(stream);
		}

		public TagCompound(Dictionary<string, Tag> value) : base()
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = new Dictionary<string, Tag>(value);
		}

		public TagCompound(int capacity) : base()
		{
			this.value = new Dictionary<string, Tag>(capacity);
		}

		public TagCompound(IEnumerable<KeyValuePair<string, Tag>> values) : this()
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			this.value = new Dictionary<string, Tag>(values);
		}

		public override byte tagID
		{
			get
			{
				return TagTypes.TagCompound;
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
			bool exit = false;
			while (exit != true)
			{
				byte id = TagByte.ReadByte(stream);
				if (id == TagTypes.TagEnd)
				{
					exit = true;
				}
				if (exit != true)
				{
					string tagEntry_Key = TagString.ReadString(stream);
					Tag tagEntry_Value = Tag.ReadTag(stream, id);
					this.value.Add(tagEntry_Key, tagEntry_Value);
				}
			}
		}

		internal override void writeTag(Stream stream)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(stream);
			foreach (KeyValuePair<string, Tag> keyValue in this.value)
			{
				//Escribimos el identificador de la etiqueta
				stream.WriteByte(keyValue.Value.tagID);
				//Escribimos el nombre de la etiqueta
				TagString.WriteString(stream, keyValue.Key);
				//Escribimos el contenido de la etiqueta
				keyValue.Value.writeTag(stream);
			}
			//Escribimos la etiqueta de cierre
			stream.WriteByte(TagTypes.TagEnd);
		}

		public int Count
		{
			get
			{
				return this.value.Count;
			}
		}

		public Dictionary<string, Tag>.Enumerator GetEnumerator()
		{
			return this.value.GetEnumerator();
		}

		IEnumerator<KeyValuePair<string, Tag>> IEnumerable<KeyValuePair<string, Tag>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override object Clone()
		{
			TagCompound result = new TagCompound();

			foreach (KeyValuePair<string, Tag> value in this.value)
			{
				result.Add(value.Key, (Tag)value.Value.Clone());
			}

			return result;
		}

		public Dictionary<string, Tag>.ValueCollection Values
		{
			get
			{
				return this.value.Values;
			}
		}
		ICollection<Tag> IDictionary<string, Tag>.Values => Values;

		public Dictionary<string, Tag>.KeyCollection Keys
		{
			get
			{
				return this.value.Keys;
			}
		}
		ICollection<string> IDictionary<string, Tag>.Keys => Keys;

		public void Add(string key, Tag value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(key);
			NBT_InvalidArgumentNullException.ThrowIfNull(value);
			if (!this.TryAdd(key, value)) NBT_InvalidArgumentException.Throw("This key already exist");
		}
		public void Add(KeyValuePair<string, Tag> value) => Add(value.Key, value.Value);

		public void AddRange(IEnumerable<KeyValuePair<string, Tag>> items)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(items);
			foreach (KeyValuePair<string, Tag> pair in items)
			{
				this.Add(pair.Key, pair.Value);
			}
		}

		public bool TryAdd(string key, Tag value)
		{
			NBT_InvalidArgumentNullException.ThrowIfNull(key);
			return this.value.TryAdd(key, value);
		}

		public void Clear()
		{
			this.value.Clear();
		}

		public bool ContainsKey(string key)
		{
			return this.value.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<string, Tag> value)
		{
			return this.value.Contains(value);
		}

		public bool Remove(string key)
		{
			return this.value.Remove(key);
		}

		public void Rename(string oldKeyName, string newKeyName)
		{
			if (this.ContainsKey(oldKeyName) == false)
			{
				NBT_InvalidArgumentException.Throw("oldKeyName");
			}
			if (this.ContainsKey(newKeyName) == true)
			{
				NBT_InvalidArgumentException.Throw("newKeyName");
			}
			Tag tag = this[oldKeyName];
			this.Remove(oldKeyName);
			this.Add(newKeyName, tag);
		}

		public override Tag this[string key]
		{
			get
			{
				if (!TryGetValue(key, out var value)) NBT_InvalidArgumentException.Throw();
				return value;
			}
			set
			{
				NBT_InvalidArgumentNullException.ThrowIfNull(key);
				NBT_InvalidArgumentNullException.ThrowIfNull(value);
				this.value[key] = value;
			}
		}

		public bool TryGetValue(string key, out Tag value)
		{
			return this.value.TryGetValue(key, out value);
		}

		bool ICollection<KeyValuePair<string, Tag>>.IsReadOnly => ((ICollection<KeyValuePair<string, Tag>>)value).IsReadOnly;

		void ICollection<KeyValuePair<string, Tag>>.CopyTo(KeyValuePair<string, Tag>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, Tag>>)value).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<string, Tag>>.Remove(KeyValuePair<string, Tag> item)
		{
			return ((ICollection<KeyValuePair<string, Tag>>)value).Remove(item);
		}

		public static explicit operator TagCompound(Dictionary<string, Tag> value)
		{
			return new TagCompound(value);
		}

		public override object ValueProp
		{
			get
			{
				return this.value;
			}
			set
			{
				Dictionary<string, Tag> value2 = null;
				Helper.ValuePropHelper(ref value2, value);
				this.value = new(value2);
			}
		}

		public bool Equals(TagCompound other)
		{
			bool bResult = false;
			bool exitFor = false;
			try
			{
				if (this.value.Keys.Count == other.value.Keys.Count)
				{
					foreach (KeyValuePair<string, Tag> kvp in this.value)
					{
						Tag tmpValue;
						if (other.value.TryGetValue(kvp.Key, out tmpValue) == false)
						{
							exitFor = true;
						}
						else
						{
							exitFor = !kvp.Value.Equals(tmpValue);
						}
						if (exitFor == true)
						{
							break;
						}
					}
					bResult = (exitFor == false);
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
			return other is TagCompound other2 && Equals(other2);
		}

		public override bool Equals(object obj)
		{
			return obj is TagCompound other2 && Equals(other2);
		}

		public override int GetHashCode()
		{
			//not sure what a useful return here would be
			return 0;
		}
	}
}
