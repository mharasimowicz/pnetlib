/*
 * ArrayList.cs - Generic array list class.
 *
 * Copyright (c) 2003  Southern Storm Software, Pty Ltd
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

namespace Generics
{

using System;

public class ArrayList<T>
	: ICollection<T>, IEnumerable<T>, IList<T>, ICloneable, IIterable<T>
{
	// Internal state.
	private int count;
	private T[] store;
	private int generation;

	// Simple constructors.
	public ArrayList()
			{
				count = 0;
				store = new T [16];
				generation = 0;
			}
	public ArrayList(int capacity)
			{
				if(capacity < 0)
				{
					throw new ArgumentOutOfRangeException
						("capacity", S._("ArgRange_NonNegative"));
				}
				count = 0;
				store = new T [capacity];
				generation = 0;
			}

	// Construct an array list from the contents of a collection.
	public ArrayList(ICollection<T> c)
			{
				IEnumerator<T> enumerator;
				int index;
				if(c == null)
				{
					throw new ArgumentNullException("c");
				}
				count = c.Count;
				store = new T [count];
				enumerator = c.GetEnumerator();
				for(index = 0; enumerator.MoveNext(); ++index)
				{
					store[index] = enumerator.Current;
				}
				generation = 0;
			}

	// Reallocate the array to accomodate "n" new entries at "index".
	// This leaves "count" unchanged.
	private void Realloc(int n, int index)
			{
				if((count + n) <= store.Length)
				{
					// The current capacity is sufficient, so just
					// shift the contents of the array upwards.
					int posn = count - 1;
					while(posn >= index)
					{
						store[posn + n] = store[posn];
						--posn;
					}
				}
				else
				{
					// We need to allocate a new array.
					int newCapacity = (((count + n) + 31) & ~31);
					int newCapacity2 = count * 2;
					if(newCapacity2 > newCapacity)
					{
						newCapacity = newCapacity2;
					}
					T[] newStore = new T [newCapacity];
					if(index != 0)
					{
						Array.Copy(store, 0, newStore, 0, index);
					}
					if(count != index)
					{
						Array.Copy(store, index, newStore, index + n,
								   count - index);
					}
					store = newStore;
				}
			}

	// Delete "n" entries from the list at "index".
	// This modifies "count".
	private void Delete(int n, int index)
			{
				while((index + n) < count)
				{
					store[index] = store[index + n];
					++index;
				}
				count -= n;
			}

	// Implement the IList<T> interface.
	public virtual int Add(T value)
			{
				if(count >= store.Length)
				{
					Realloc(1, count);
				}
				store[count] = value;
				++generation;
				return count++;
			}
	public virtual void Clear()
			{
				Array.Clear(store, 0, count);
				count = 0;
				++generation;
			}
	public virtual bool Contains(T item)
			{
				int index;
				if(typeof(T).IsValueType)
				{
					for(index = 0; index < count; ++index)
					{
						if(item.Equals(store[index]))
						{
							return true;
						}
					}
					return false;
				}
				else
				{
					if(((Object)item) != null)
					{
						for(index = 0; index < count; ++index)
						{
							if(item.Equals(store[index]))
							{
								return true;
							}
						}
						return false;
					}
					else
					{
						for(index = 0; index < count; ++index)
						{
							if(((Object)(store[index])) == null)
							{
								return true;
							}
						}
						return false;
					}
				}
			}
	public virtual int IndexOf(T value)
			{
				return IndexOf(value, 0, Count);
			}
	public virtual void Insert(int index, T value)
			{
				if(index < 0 || index > count)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				Realloc(1, index);
				store[index] = value;
				++count;
				++generation;
			}
	public virtual void Remove(T value)
			{
				int index = Array.IndexOf(store, T, 0, count);
				if(index != -1)
				{
					Delete(1, index);
					++generation;
				}
			}
	public virtual void RemoveAt(int index)
			{
				if(index < 0 || index > count)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				Delete(1, index);
				++generation;
			}
	public virtual bool IsFixedSize
			{
				get
				{
					return false;
				}
			}
	public virtual bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
	public virtual T this[int index]
			{
				get
				{
					if(index < 0 || index >= count)
					{
						throw new ArgumentOutOfRangeException
							("index", S._("ArgRange_Array"));
					}
					return store[index];
				}
				set
				{
					if(index < 0 || index >= count)
					{
						throw new ArgumentOutOfRangeException
							("index", S._("ArgRange_Array"));
					}
					store[index] = value;
					++generation;
				}
			}

	// Add the contents of a collection as a range.
	public virtual void AddRange(ICollection<T> c)
			{
				int cCount;
				IEnumerator<T> enumerator;
				if(c == null)
				{
					throw new ArgumentNullException("c");
				}
				cCount = c.Count;
				if((count + cCount) > store.Length)
				{
					Realloc(cCount, count);
				}
				enumerator = c.GetEnumerator();
				while(enumerator.MoveNext())
				{
					store[count++] = enumerator.Current;
				}
				++generation;
			}

	// Insert the contents of a collection as a range.
	public virtual void InsertRange(int index, ICollection<T> c)
			{
				int cCount;
				IEnumerator<T> enumerator;
				if(c == null)
				{
					throw new ArgumentNullException("c");
				}
				if(index < 0 || index > count)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				cCount = c.Count;
				Realloc(cCount, index);
				enumerator = c.GetEnumerator();
				while(enumerator.MoveNext())
				{
					store[index++] = enumerator.Current;
				}
				count += cCount;
				++generation;
			}

	// Remove a range of elements from an array list.
	public virtual void RemoveRange(int index, int count)
			{
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((this.count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				Delete(count, index);
				++generation;
			}

	// Perform a binary search on an array list.
	public virtual int BinarySearch(T value)
			{
				return BinarySearch(0, Count, value, null);
			}
	public virtual int BinarySearch(T value, IComparer<T> comparer)
			{
				return BinarySearch(0, Count, value, comparer);
			}
	public virtual int BinarySearch(int index, int count,
								    T value, IComparer<T> comparer)
			{
				// Validate the arguments.
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((Count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}

				// Perform the binary search.
				int left, right, middle, cmp;
				Object elem;
				IComparable icmp;
				left = index;
				right = index + count - 1;
				while(left <= right)
				{
					middle = (left + right) / 2;
					elem = this[middle];
					if(elem != null && value != null)
					{
						if(comparer != null)
						{
							cmp = comparer.Compare(value, elem);
						}
						else if((icmp = (elem as IComparable)) != null)
						{
							cmp = -(icmp.CompareTo(value));
						}
						else if((icmp = (value as IComparable)) != null)
						{
							cmp = icmp.CompareTo(elem);
						}
						else
						{
							throw new ArgumentException
								(_("Arg_SearchCompare"));
						}
					}
					else if(elem != null)
					{
						cmp = -1;
					}
					else if(value != null)
					{
						cmp = 1;
					}
					else
					{
						cmp = 0;
					}
					if(cmp == 0)
					{
						return middle;
					}
					else if(cmp < 0)
					{
						right = middle - 1;
					}
					else
					{
						left = middle + 1;
					}
				}
				return ~left;
			}

	// Implement the ICloneable interface.
	public virtual Object Clone()
			{
				ArrayList<T> clone = new ArrayList<T>(count);
				clone.count = count;
				clone.generation = generation;
				Array.Copy(store, 0, clone.store, 0, count);
				return clone;
			}

	// Implement the ICollection<T> interface.
	public virtual void CopyTo(T[] array, int arrayIndex)
			{
				Array.Copy(store, 0, array, arrayIndex, count);
			}
	public virtual int Count
			{
				get
				{
					return count;
				}
			}
	public virtual bool IsSynchronized
			{
				get
				{
					return false;
				}
			}
	public virtual Object SyncRoot
			{
				get
				{
					return this;
				}
			}

	// Copy from this array list to another array.
	public virtual void CopyTo(T[] array)
			{
				Array.Copy(store, 0, array, 0, count);
			}
	public virtual void CopyTo(int index, T[] array,
							   int arrayIndex, int count)
			{
				// Validate the parameters.
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((Count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}

				// Perform the copy.
				if(GetType() == typeof(ArrayList<T>))
				{
					// We can use a short-cut because we know that
					// the list elements are in "store".
					Array.Copy(store, index, array, arrayIndex, count);
				}
				else
				{
					// The list elements may be elsewhere.
					while(count > 0)
					{
						array.SetValue(this[index], arrayIndex);
						++index;
						++arrayIndex;
						--count;
					}
				}
			}

	// Get the index of a value within an array list.
	public virtual int IndexOf(T value, int startIndex)
			{
				int count = Count;
				if(startIndex < 0 || startIndex >= count)
				{
					throw new ArgumentOutOfRangeException
						("startIndex", S._("ArgRange_Array"));
				}
				return IndexOf(value, startIndex, count - startIndex);
			}
	public virtual int IndexOf(T value, int startIndex, int count)
			{
				// Validate the parameters.
				int thisCount = Count;
				if(startIndex < 0 || startIndex >= thisCount)
				{
					throw new ArgumentOutOfRangeException
						("startIndex", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((thisCount - startIndex) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}

				// Perform the search.
				Object elem;
				while(count > 0)
				{
					elem = this[startIndex];
					if(elem != null && value != null)
					{
						if(value.Equals(elem))
						{
							return startIndex;
						}
					}
					else if(elem == value && value == null)
					{
						return startIndex;
					}
					++startIndex;
					--count;
				}
				return -1;
			}

	// Get the last index of a value within an array list.
	public virtual int LastIndexOf(T value)
			{
				int count = Count;
				return LastIndexOf(value, count - 1, count);
			}
	public virtual int LastIndexOf(T value, int startIndex)
			{
				int count = Count;
				if(startIndex < 0 || startIndex >= count)
				{
					throw new ArgumentOutOfRangeException
						("startIndex", S._("ArgRange_Array"));
				}
				return LastIndexOf(value, startIndex, startIndex + 1);
			}
	public virtual int LastIndexOf(T value, int startIndex, int count)
			{
				// Validate the parameters.
				if(startIndex < 0 || startIndex >= Count)
				{
					throw new ArgumentOutOfRangeException
						("startIndex", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if(count > (startIndex + 1))
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}

				// Perform the search.
				Object elem;
				while(count > 0)
				{
					elem = this[startIndex];
					if(elem != null && value != null)
					{
						if(value.Equals(elem))
						{
							return startIndex;
						}
					}
					else if(elem == value && value == null)
					{
						return startIndex;
					}
					--startIndex;
					--count;
				}
				return -1;
			}

	// Construct an array list with repeated copies of the same element.
	public static ArrayList<T> Repeat<T>(T value, int count)
			{
				ArrayList list;
				int index;
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S_("ArgRange_NonNegative"));
				}
				if(count < 16)
				{
					list = new ArrayList<T>();
				}
				else
				{
					list = new ArrayList<T>(count);
				}
				list.Realloc(count, 0);
				for(index = 0; index < count; ++index)
				{
					list.store[index] = value;
				}
				list.count = count;
				return list;
			}

	// Reverse the contents of this array list.
	public virtual void Reverse()
			{
				Array.Reverse(store, 0, count);
				++generation;
			}
	public virtual void Reverse(int index, int count)
			{
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((this.count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				Array.Reverse(store, index, count);
				++generation;
			}

	// Set a range of array list elements to the members of a collection.
	public virtual void SetRange(int index, ICollection<T> c)
			{
				int count;
				IEnumerator<T> enumerator;
				if(c == null)
				{
					throw new ArgumentNullException("c");
				}
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				count = c.Count;
				if((this.count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				enumerator = c.GetEnumerator();
				while(enumerator.MoveNext())
				{
					store[index++] = enumerator.Current;
				}
				++generation;
			}

	// Inner version of "Sort".
	private void InnerSort(int lower, int upper, IComparer<T> comparer)
			{
				// Temporary hack - use a dumb sort until I can figure
				// out what is wrong with the Quicksort code -- Rhys.
				int i, j, cmp;
				T valuei;
				T valuej;
				for(i = lower; i < upper; ++i)
				{
					for(j = i + 1; j <= upper; ++j)
					{
						valuei = this[i];
						valuej = this[j];
						if(comparer != null)
						{
							cmp = comparer.Compare(valuei, valuej);
						}
						else
						{
							cmp = ((IComparable)valuei).CompareTo(valuej);
						}
						if(cmp > 0)
						{
							this[i] = valuej;
							this[j] = valuei;
						}
					}
				}
			}

	// Sort the contents of this array list.
	public virtual void Sort()
			{
				InnerSort(0, Count - 1, null);
			}
	public virtual void Sort(IComparer<T> comparer)
			{
				InnerSort(0, Count - 1, comparer);
			}
	public virtual void Sort(int index, int count, IComparer<T> comparer)
			{
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((Count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				InnerSort(index, index + count - 1, comparer);
			}

	// Create an array that contains the elements of this array list.
	public virtual T[] ToArray()
			{
				int count = Count;
				T[] array = new T[count];
				int index;
				for(index = 0; index < count; ++index)
				{
					array[index] = this[index];
				}
				return array;
			}
	public virtual Array ToArray(Type type)
			{
				int count = Count;
				Array array = Array.CreateInstance(type, count);
				int index;
				for(index = 0; index < count; ++index)
				{
					array.SetValue(this[index], index);
				}
				return array;
			}

	// Trim the array list to its actual size.
	public virtual void TrimToSize()
			{
				if(count != 0)
				{
					if(count != store.Length)
					{
						T[] newStore = new T[count];
						int index;
						for(index = 0; index < count; ++index)
						{
							newStore[index] = store[index];
						}
						store = newStore;
					}
				}
				else if(store.Length != 16)
				{
					store = new T[16];
				}
				++generation;
			}

	// Get or set the current capacity of the array list.
	public virtual int Capacity
			{
				get
				{
					return store.Length;
				}
				set
				{
					if(value <= 0)
					{
						value = 16;
					}
					if(value < count)
					{
						throw new ArgumentOutOfRangeException
							("value", S._("Arg_CannotReduceCapacity"));
					}
					if(value != store.Length)
					{
						T[] newStore = new T[value];
						int index;
						for(index = 0; index < count; ++index)
						{
							newStore[index] = store[index];
						}
						store = newStore;
					}
					++generation;
				}
			}

	// Get an enumerator for this array list.
	public virtual IEnumerator<T> GetEnumerator()
			{
				return new ArrayListEnumerator<T>(this, 0, Count);
			}
	public virtual IEnumerator<T> GetEnumerator(int index, int count)
			{
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((Count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				return new ArrayListEnumerator<T>(this, index, index + count);
			}

	// Get an iterator for this array list.
	public virtual IIterator<T> GetIterator()
			{
				return new ArrayListIterator<T>(this, 0, Count);
			}
	public virtual IIterator<T> GetIterator(int index, int count)
			{
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((Count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				return new ArrayListIterator<T>(this, index, index + count);
			}

	// Array list enumerator class.
	private class ArrayListEnumerator<T> : IEnumerator<T>
	{
		// Internal state.
		private ArrayList<T> list;
		private int start;
		private int finish;
		private int position;
		private int generation;

		// Constructor.
		public ArrayListEnumerator(ArrayList<T> list, int start, int finish)
				{
					this.list = list;
					this.start = start;
					this.finish = finish;
					position = start - 1;
					generation = list.generation;
				}

		// Implement the IEnumerator<T> interface.
		public bool MoveNext()
				{
					if(generation != list.generation)
					{
						throw new InvalidOperationException
							(S._("Invalid_CollectionModified"));
					}
					++position;
					return (position < finish);
				}
		public void Reset()
				{
					position = start - 1;
				}
		public T Current
				{
					get
					{
						if(generation != list.generation)
						{
							throw new InvalidOperationException
								(S._("Invalid_CollectionModified"));
						}
						else if(position < start || position >= finish)
						{
							throw new InvalidOperationException
								(S._("Invalid_BadEnumeratorPosition"));
						}
						return list[position];
					}
				}

	}; // class ArrayListEnumerator<T>

	// Array list iterator class.
	private class ArrayListIterator<T> : IEnumerator<T>, IIterator<T>
	{
		// Internal state.
		private ArrayList<T> list;
		private int start;
		private int finish;
		private int position;
		private bool reset;

		// Constructor.
		public ArrayListIterator(ArrayList<T> list, int start, int finish)
				{
					this.list = list;
					this.start = start;
					this.finish = finish;
					position = start - 1;
					reset = true;
				}

		// Implement the IEnumerator<T> interface.
		public bool MoveNext()
				{
					if(reset)
					{
						// Start at the beginning of the range.
						position = start - 1;
						reset = false;
					}
					++position;
					return (position < finish);
				}
		public void Reset()
				{
					reset = true;
					position = start - 1;
				}
		T IEnumerator<T>.Current
				{
					get
					{
						if(position < start || position >= finish)
						{
							throw new InvalidOperationException
								(S._("Invalid_BadIteratorPosition"));
						}
						return list[position];
					}
				}

		// Implement the IIterator<T> interface.
		public bool MovePrev()
				{
					if(reset)
					{
						// Start at the end of the range.
						position = finish;
						reset = false;
					}
					--position;
					return (position >= start);
				}
		public T Current
				{
					get
					{
						if(position < start || position >= finish)
						{
							throw new InvalidOperationException
								(S._("Invalid_BadIteratorPosition"));
						}
						return list[position];
					}
					set
					{
						if(position < start || position >= finish)
						{
							throw new InvalidOperationException
								(S._("Invalid_BadIteratorPosition"));
						}
						list[position] = value;
					}
				}

	}; // class ArrayListIterator<T>

	// Adapt an IList<T> to appear to look like an ArrayList<T>.
	public static ArrayList<T> Adapter<T>(IList<T> list)
		{
			if(list == null)
			{
				throw new ArgumentNullException("list");
			}
			else if(list is ArrayList<T>)
			{
				return (ArrayList)list;
			}
			else
			{
				return new IListWrapper<T>(list);
			}
		}

	// Wrapper class for IList.
	private class IListWrapper<T> : ArrayList<T>
	{

		// Internal state.
		private IList<T> list;

		// Constructor
		public IListWrapper(IList<T> list)
				{
					this.list = list;
				}

		// Implement the IList interface.
		public override int Add(T value)
				{
					return list.Add(value);
				}
		public override void Clear()
				{
					list.Clear();
				}
		public override bool Contains(T item)
				{
					return list.Contains(item);
				}
		public override int IndexOf(T value)
				{
					return list.IndexOf(value);
				}
		public override void Insert(int index, T value)
				{
					list.Insert(index, value);
				}
		public override void Remove(T value)
				{
					list.Remove(value);
				}
		public override void RemoveAt(int index)
				{
					list.RemoveAt(index);
				}
		public override bool IsFixedSize
				{
					get
					{
						return list.IsFixedSize;
					}
				}
		public override bool IsReadOnly
				{
					get
					{
						return list.IsReadOnly;
					}
				}
		public override T this[int index]
				{
					get
					{
						return list[index];
					}
					set
					{
						list[index] = value;
					}
				}

		// Implement the ICloneable interface.
		public override Object Clone()
				{
					return new IListWrapper<T>(list);
				}

		// Range-related methods.
		public override void AddRange(ICollection<T> c)
				{
					if(c == null)
					{
						throw new ArgumentNullException("c");
					}
					IEnumerator<T> enumerator = c.GetEnumerator();
					while(enumerator.MoveNext())
					{
						list.Add(enumerator.Current);
					}
				}
		public override void InsertRange(int index, ICollection<T> c)
				{
					if(c == null)
					{
						throw new ArgumentNullException("c");
					}
					IEnumerator<T> enumerator = c.GetEnumerator();
					while(enumerator.MoveNext())
					{
						list.Insert(index++, enumerator.Current);
					}
				}
		public override void RemoveRange(int index, int count)
				{
					if(index < 0 || index >= list.Count)
					{
						throw new ArgumentOutOfRangeException
							("index", S._("ArgRange_Array"));
					}
					if(count < 0)
					{
						throw new ArgumentOutOfRangeException
							("count", S._("ArgRange_Array"));
					}
					while(count > 0)
					{
						list.RemoveAt(index);
						--count;
					}
				}
		public override void SetRange(int index, ICollection<T> c)
				{
					if(c == null)
					{
						throw new ArgumentNullException("c");
					}
					IEnumerator<T> enumerator = c.GetEnumerator();
					while(enumerator.MoveNext())
					{
						list[index++] = enumerator.Current;
					}
				}

		// Implement the ICollection interface.
		public override void CopyTo(T[] array, int arrayIndex)
				{
					list.CopyTo(array, arrayIndex);
				}
		public override int Count
				{
					get
					{
						return list.Count;
					}
				}
		public override bool IsSynchronized
				{
					get
					{
						return list.IsSynchronized;
					}
				}
		public override Object SyncRoot
				{
					get
					{
						return list.SyncRoot;
					}
				}

		// Copy from this array list to another array.
		public override void CopyTo(T[] array)
				{
					list.CopyTo(array, 0);
				}

		// Reverse the contents of this array list.
		public override void Reverse()
				{
					Reverse(0, Count);
				}
		public override void Reverse(int index, int count)
				{
					// Validate the parameters.
					if(index < 0)
					{
						throw new ArgumentOutOfRangeException
							("index", S._("ArgRange_Array"));
					}
					if(count < 0)
					{
						throw new ArgumentOutOfRangeException
							("count", S._("ArgRange_Array"));
					}
					if((Count - index) < count)
					{
						throw new ArgumentException
							(S._("Arg_InvalidArrayRange"));
					}

					// Perform the reversal.
					T temp;
					int lower = index;
					int upper = index + count - 1;
					while(lower < upper)
					{
						temp = list[lower];
						list[lower] = list[upper];
						list[upper] = temp;
						++lower;
						--upper;
					}
				}

		// Trim the array list to its actual size.
		public override void TrimToSize()
				{
					// Nothing can be done here.
				}

		// Get or set the current capacity of the array list.
		public override int Capacity
				{
					get
					{
						// Return the IList<T>'s count as the capacity.
						return list.Count;
					}
					set
					{
						// IList<T> does not have a capacity, so just validate.
						if(value < list.Count)
						{
							throw new ArgumentOutOfRangeException
								("value", S._("Arg_CannotReduceCapacity"));
						}
					}
				}

	}; // class IListWrapper<T>

	// Pass-through wrapper class that encapsulates another array list.
	private class PassThroughWrapper<T> : ArrayList<T>
	{
		protected ArrayList<T> list;

		public PassThroughWrapper(ArrayList<T> list)
				{
					this.list = list;
				}

		// Implement the IList interface.
		public override int Add(T value)
				{
					return list.Add(value);
				}
		public override void Clear()
				{
					list.Clear();
				}
		public override bool Contains(T item)
				{
					return list.Contains(item);
				}
		public override int IndexOf(T value)
				{
					return list.IndexOf(value);
				}
		public override void Insert(int index, T value)
				{
					list.Insert(index, value);
				}
		public override void Remove(T value)
				{
					list.Remove(value);
				}
		public override void RemoveAt(int index)
				{
					list.RemoveAt(index);
				}
		public override bool IsFixedSize
				{
					get
					{
						return list.IsFixedSize;
					}
				}
		public override bool IsReadOnly
				{
					get
					{
						return list.IsReadOnly;
					}
				}
		public override T this[int index]
				{
					get
					{
						return list[index];
					}
					set
					{
						list[index] = value;
					}
				}

		// Range-related methods.
		public override void AddRange(ICollection<T> c)
				{
					list.AddRange(c);
				}
		public override void InsertRange(int index, ICollection<T> c)
				{
					list.InsertRange(index, c);
				}
		public override void RemoveRange(int index, int count)
				{
					list.RemoveRange(index, count);
				}
		public override void SetRange(int index, ICollection<T> c)
				{
					list.SetRange(index, c);
				}

		// Searching methods.
		public override int BinarySearch(T value)
				{
					return list.BinarySearch(value);
				}
		public override int BinarySearch(T value, IComparer<T> comparer)
				{
					return list.BinarySearch(value, comparer);
				}
		public override int BinarySearch(int index, int count,
									     T value, IComparer<T> comparer)
				{
					return list.BinarySearch(index, count, value, comparer);
				}
		public override int IndexOf(T value, int startIndex)
				{
					return list.IndexOf(value, startIndex);
				}
		public override int IndexOf(T value, int startIndex, int count)
				{
					return list.IndexOf(value, startIndex, count);
				}
		public override int LastIndexOf(T value)
				{
					return list.LastIndexOf(value);
				}
		public override int LastIndexOf(T value, int startIndex)
				{
					return list.LastIndexOf(value, startIndex);
				}
		public override int LastIndexOf(T value, int startIndex, int count)
				{
					return list.LastIndexOf(value, startIndex, count);
				}

		// Implement the ICollection<T> interface.
		public override void CopyTo(T[] array, int arrayIndex)
				{
					list.CopyTo(array, arrayIndex);
				}
		public override int Count
				{
					get
					{
						return list.count;
					}
				}
		public override bool IsSynchronized
				{
					get
					{
						return list.IsSynchronized;
					}
				}
		public override Object SyncRoot
				{
					get
					{
						return list.SyncRoot;
					}
				}

		// Copy from this array list to another array.
		public override void CopyTo(T[] array)
				{
					list.CopyTo(array);
				}
		public override void CopyTo(int index, T[] array,
							   	    int arrayIndex, int count)
				{
					list.CopyTo(index, array, arrayIndex, count);
				}

		// Reverse the contents of this array list.
		public override void Reverse()
				{
					list.Reverse();
				}
		public override void Reverse(int index, int count)
				{
					list.Reverse(index, count);
				}

		// Sort the contents of this array list.
		public override void Sort()
				{
					list.Sort();
				}
		public override void Sort(IComparer<T> comparer)
				{
					list.Sort(comparer);
				}
		public override void Sort(int index, int count, IComparer<T> comparer)
				{
					list.Sort(index, count, comparer);
				}

		// Create an array that contains the elements of this array list.
		public override T[] ToArray()
				{
					return list.ToArray();
				}
		public override Array ToArray(Type type)
				{
					return list.ToArray(type);
				}

		// Trim the array list to its actual size.
		public override void TrimToSize()
				{
					list.TrimToSize();
				}

		// Get or set the current capacity of the array list.
		public override int Capacity
				{
					get
					{
						return list.Capacity;
					}
					set
					{
						list.Capacity = value;
					}
				}

		// Get an enumerator for this array list.
		public override IEnumerator<T> GetEnumerator()
				{
					return list.GetEnumerator();
				}
		public override IEnumerator<T> GetEnumerator(int index, int count)
				{
					return list.GetEnumerator(index, count);
				}

		// Get an iterator for this array list.
		public override IIterator<T> GetIterator()
				{
					return list.GetIterator();
				}
		public override IIterator<T> GetIterator(int index, int count)
				{
					return list.GetIterator(index, count);
				}

	}; // class PassThroughWrapper<T>

	// Adapt an array list to appear to have a fixed size.
	public static ArrayList<T> FixedSize<T>(ArrayList<T> list)
			{
				if(list == null)
				{
					throw new ArgumentNullException("list");
				}
				else if(list.IsFixedSize)
				{
					return list;
				}
				else
				{
					return new FixedSizeWrapper<T>(list);
				}
			}

	// Wrapper class for fixed size lists.
	private class FixedSizeWrapper<T> : PassThroughWrapper<T>
	{
		public FixedSizeWrapper(ArrayList<T> list)
				: base(list)
				{
				}

		// Implement the IList interface.
		public override int Add(T value)
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}
		public override void Clear()
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}
		public override void Insert(int index, T value)
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}
		public override void Remove(T value)
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}
		public override void RemoveAt(int index)
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}
		public override bool IsFixedSize
				{
					get
					{
						return true;
					}
				}
		public override bool IsReadOnly
				{
					get
					{
						return list.IsReadOnly;
					}
				}
		public override T this[int index]
				{
					get
					{
						return list[index];
					}
					set
					{
						list[index] = value;
					}
				}

		// Range-related methods.
		public override void AddRange(ICollection<T> c)
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}
		public override void InsertRange(int index, ICollection c)
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}
		public override void RemoveRange(int index, int count)
				{
					throw new NotSupportedException
						(S._("NotSupp_FixedSizeCollection"));
				}

		// Implement the ICloneable interface.
		public override Object Clone()
				{
					return new FixedSizeWrapper<T>
						((ArrayList<T>)(list.Clone()));
				}

		// Trim the array list to its actual size.
		public override void TrimToSize()
				{
					throw new NotSupportedException
						(S. _("NotSupp_FixedSizeCollection"));
				}

	}; // class FixedSizeWrapper<T>

	// Adapt an array list to get access to a sub-range.
	public virtual ArrayList<T> GetRange(int index, int count)
			{
				if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				if(count < 0)
				{
					throw new ArgumentOutOfRangeException
						("count", S._("ArgRange_Array"));
				}
				if((this.count - index) < count)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				return new RangeWrapper<T>(this, index, count);
			}

	// Wrapper class for sub-range array lists.
	private class RangeWrapper<T> : ArrayList<T>
	{
		private ArrayList<T> list;
		private int index;
		private new int count;

		public RangeWrapper(ArrayList<T> list, int index, int count)
				: base(list)
				{
					this.list  = list;
					this.index = index;
					this.count = count;
					generation = list.generation;
				}

		// Determine if the underlying array list has been changed.
		private void UnderlyingCheck()
				{
					if(generation != list.generation)
					{
						throw new InvalidOperationException
							(S._("Invalid_UnderlyingModified"));
					}
				}

		// Implement the IList interface.
		public override int Add(T value)
				{
					UnderlyingCheck();
					list.Insert(index + count, value);
					generation = list.generation;
					return index + count;
				}
		public override void Clear()
				{
					UnderlyingCheck();
					list.Clear();
					generation = list.generation;
				}
		public override bool Contains(T item)
				{
					UnderlyingCheck();
					return (list.IndexOf(item) != -1);
				}
		public override int IndexOf(T value)
				{
					UnderlyingCheck();
					return list.IndexOf(value, index, count);
				}
		public override void Insert(int index, T value)
				{
					UnderlyingCheck();
					if(index < 0 || index >= count)
					{
						throw new ArgumentOutOfRangeException
							("index", S._("ArgRange_Array"));
					}
					list.Insert(index + this.index, value);
					generation = list.generation;
				}
		public override void Remove(T value)
				{
					UnderlyingCheck();
					int ind = list.IndexOf(value, index, count);
					if(ind != -1)
					{
						list.RemoveAt(ind);
					}
					generation = list.generation;
				}
		public override void RemoveAt(int index)
				{
					UnderlyingCheck();
					list.RemoveAt(index + this.index);
					generation = list.generation;
				}
		public override bool IsFixedSize
				{
					get
					{
						return list.IsFixedSize;
					}
				}
		public override bool IsReadOnly
				{
					get
					{
						return list.IsReadOnly;
					}
				}
		public override T this[int index]
				{
					get
					{
						UnderlyingCheck();
						if(index < 0 || index >= count)
						{
							throw new ArgumentOutOfRangeException
								("index", S._("ArgRange_Array"));
						}
						return list[index + this.index];
					}
					set
					{
						UnderlyingCheck();
						if(index < 0 || index >= count)
						{
							throw new ArgumentOutOfRangeException
								("index", S._("ArgRange_Array"));
						}
						list[index + this.index] = value;
						generation = list.generation;
					}
				}

		// Range-related methods.
		public override void AddRange(ICollection<T> c)
				{
					UnderlyingCheck();
					list.InsertRange(index + count, c);
					generation = list.generation;
				}
		public override void InsertRange(int index, ICollection<T> c)
				{
					UnderlyingCheck();
					list.InsertRange(index + this.index, c);
					generation = list.generation;
				}
		public override void RemoveRange(int index, int count)
				{
					UnderlyingCheck();
					list.RemoveRange(index + this.index, count);
					generation = list.generation;
				}
		public override void SetRange(int index, ICollection<T> c)
				{
					UnderlyingCheck();
					list.SetRange(index + this.index, c);
					generation = list.generation;
				}

		// Implement the ICloneable interface.
		public override Object Clone()
				{
					return new RangeWrapper<T>
						((ArrayList<T>)(list.Clone()), index, count);
				}

		// Searching methods.
		public override int BinarySearch(T value)
				{
					UnderlyingCheck();
					return list.BinarySearch
						(index, count, value, (IComparer<T>)null);
				}
		public override int BinarySearch(T value, IComparer<T> comparer)
				{
					UnderlyingCheck();
					return list.BinarySearch(index, count, value, comparer);
				}
		public override int BinarySearch(int index, int count,
									     T value, IComparer<T> comparer)
				{
					UnderlyingCheck();
					return list.BinarySearch(index + this.index, count,
											 value, comparer);
				}
		public override int IndexOf(T value, int startIndex)
				{
					UnderlyingCheck();
					return list.IndexOf(value, startIndex);
				}
		public override int IndexOf(T value, int startIndex, int count)
				{
					UnderlyingCheck();
					return list.IndexOf(value, startIndex, count);
				}
		public override int LastIndexOf(T value)
				{
					UnderlyingCheck();
					return list.LastIndexOf(value);
				}
		public override int LastIndexOf(T value, int startIndex)
				{
					UnderlyingCheck();
					return list.LastIndexOf(value, startIndex);
				}
		public override int LastIndexOf(T value, int startIndex, int count)
				{
					UnderlyingCheck();
					return list.LastIndexOf(value, startIndex, count);
				}

		// Implement the ICollection interface.
		public override void CopyTo(T[] array, int arrayIndex)
				{
					list.CopyTo(array, arrayIndex);
				}
		public override int Count
				{
					get
					{
						UnderlyingCheck();
						return count;
					}
				}
		public override bool IsSynchronized
				{
					get
					{
						return list.IsSynchronized;
					}
				}
		public override Object SyncRoot
				{
					get
					{
						return list.SyncRoot;
					}
				}

		// Copy from this array list to another array.
		public override void CopyTo(T[] array)
				{
					list.CopyTo(array);
				}
		public override void CopyTo(int index, T[] array,
							   	    int arrayIndex, int count)
				{
					list.CopyTo(index, array, arrayIndex, count);
				}

		// Reverse the contents of this array list.
		public override void Reverse()
				{
					list.Reverse();
				}
		public override void Reverse(int index, int count)
				{
					list.Reverse(index, count);
				}

		// Sort the contents of this array list.
		public override void Sort()
				{
					list.Sort();
				}
		public override void Sort(IComparer<T> comparer)
				{
					list.Sort(comparer);
				}
		public override void Sort(int index, int count, IComparer<T> comparer)
				{
					list.Sort(index, count, comparer);
				}

		// Create an array that contains the elements of this array list.
		public override T[] ToArray()
				{
					return list.ToArray();
				}
		public override Array ToArray(Type type)
				{
					return list.ToArray(type);
				}

		// Trim the array list to its actual size.
		public override void TrimToSize()
				{
					list.TrimToSize();
				}

		// Get or set the current capacity of the array list.
		public override int Capacity
				{
					get
					{
						return list.Capacity;
					}
					set
					{
						list.Capacity = value;
					}
				}

		// Get an enumerator for this array list.
		public override IEnumerator<T> GetEnumerator()
				{
					return list.GetEnumerator();
				}
		public override IEnumerator<T> GetEnumerator(int index, int count)
				{
					return list.GetEnumerator(index, count);
				}

		// Get an iterator for this array list.
		public override IIterator<T> GetIterator()
				{
					return list.GetIterator();
				}
		public override IIterator<T> GetIterator(int index, int count)
				{
					return list.GetIterator(index, count);
				}

	}; // class RangeWrapper<T>

	// Adapt an array list to appear to be read-only.
	public static ArrayList<T> ReadOnly<T>(ArrayList<T> list)
			{
				if(list == null)
				{
					throw new ArgumentNullException("list");
				}
				else if(list.IsReadOnly)
				{
					return list;
				}
				else
				{
					return new ReadOnlyWrapper<T>(list);
				}
			}

	// Wrapper class for read-only lists.
	private class ReadOnlyWrapper<T> : PassThroughWrapper
	{
		public ReadOnlyWrapper(ArrayList<T> list)
				: base(list)
				{
				}

		// Implement the IList interface.
		public override int Add(T value)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void Clear()
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void Insert(int index, T value)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void Remove(T value)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void RemoveAt(int index)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override bool IsReadOnly
				{
					get
					{
						return true;
					}
				}
		public override T this[int index]
				{
					get
					{
						return list[index];
					}
					set
					{
						throw new NotSupportedException
							(S._("NotSupp_ReadOnly"));
					}
				}

		// Range-related methods.
		public override void AddRange(ICollection<T> c)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void InsertRange(int index, ICollection<T> c)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void RemoveRange(int index, int count)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void SetRange(int index, ICollection<T> c)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}

		// Implement the ICloneable interface.
		public override Object Clone()
				{
					return new ReadOnlyWrapper<T>((ArrayList<T>)(list.Clone()));
				}

		// Reverse the contents of this array list.
		public override void Reverse()
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void Reverse(int index, int count)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}

		// Sort the contents of this array list.
		public override void Sort()
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void Sort(IComparer<T> comparer)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}
		public override void Sort(int index, int count, IComparer<T> comparer)
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}

		// Trim the array list to its actual size.
		public override void TrimToSize()
				{
					throw new NotSupportedException(S._("NotSupp_ReadOnly"));
				}

		// Get or set the current capacity of the array list.
		public override int Capacity
				{
					get
					{
						return list.Capacity;
					}
					set
					{
						throw new NotSupportedException
							(S._("NotSupp_ReadOnly"));
					}
				}

		// Get an iterator for this array list.
		public override IIterator<T> GetIterator()
				{
					return new ReadOnlyIterator<T>(list.GetIterator());
				}
		public override IIterator<T> GetIterator(int index, int count)
				{
					return new ReadOnlyIterator<T>
						(list.GetIterator(index, count));
				}

	}; // class ReadOnlyWrapper<T>

	// Adapt an array list to appear to be synchronized
	public static ArrayList<T> Synchronized<T>(ArrayList<T> list)
			{
				if(list == null)
				{
					throw new ArgumentNullException("list");
				}
				else if(list.IsSynchronized)
				{
					return list;
				}
				else
				{
					return new SynchronizedWrapper<T>(list);
				}
			}

	// Wrapper class for synchronized lists.
	private class SynchronizedWrapper<T> : ArrayList<T>
	{
		// Internal state.
		private ArrayList<T> list;

		// Constructor.
		public SynchronizedWrapper(ArrayList<T> list)
				{
					this.list = list;
				}

		// Implement the IList interface.
		public override int Add(T value)
				{
					lock(SyncRoot)
					{
						return list.Add(value);
					}
				}
		public override void Clear()
				{
					lock(SyncRoot)
					{
						list.Clear();
					}
				}
		public override bool Contains(T item)
				{
					lock(SyncRoot)
					{
						return list.Contains(item);
					}
				}
		public override int IndexOf(T value)
				{
					lock(SyncRoot)
					{
						return list.IndexOf(value);
					}
				}
		public override void Insert(int index, T value)
				{
					lock(SyncRoot)
					{
						list.Insert(index, value);
					}
				}
		public override void Remove(T value)
				{
					lock(SyncRoot)
					{
						list.Remove(value);
					}
				}
		public override void RemoveAt(int index)
				{
					lock(SyncRoot)
					{
						list.RemoveAt(index);
					}
				}
		public override bool IsFixedSize
				{
					get
					{
						lock(SyncRoot)
						{
							return list.IsFixedSize;
						}
					}
				}
		public override bool IsReadOnly
				{
					get
					{
						lock(SyncRoot)
						{
							return list.IsReadOnly;
						}
					}
				}
		public override T this[int index]
				{
					get
					{
						lock(SyncRoot)
						{
							return list[index];
						}
					}
					set
					{
						lock(SyncRoot)
						{
							list[index] = value;
						}
					}
				}

		// Range-related methods.
		public override void AddRange(ICollection<T> c)
				{
					lock(SyncRoot)
					{
						list.AddRange(c);
					}
				}
		public override void InsertRange(int index, ICollection<T> c)
				{
					lock(SyncRoot)
					{
						list.InsertRange(index, c);
					}
				}
		public override void RemoveRange(int index, int count)
				{
					lock(SyncRoot)
					{
						list.RemoveRange(index, count);
					}
				}
		public override void SetRange(int index, ICollection<T> c)
				{
					lock(SyncRoot)
					{
						list.SetRange(index, c);
					}
				}

		// Searching methods.
		public override int BinarySearch(T value)
				{
					lock(SyncRoot)
					{
						return list.BinarySearch(value);
					}
				}
		public override int BinarySearch(T value, IComparer<T> comparer)
				{
					lock(SyncRoot)
					{
						return list.BinarySearch(value, comparer);
					}
				}
		public override int BinarySearch(int index, int count,
									     T value, IComparer<T> comparer)
				{
					lock(SyncRoot)
					{
						return list.BinarySearch(index, count, value, comparer);
					}
				}
		public override int IndexOf(T value, int startIndex)
				{
					lock(SyncRoot)
					{
						return list.IndexOf(value, startIndex);
					}
				}
		public override int IndexOf(T value, int startIndex, int count)
				{
					lock(SyncRoot)
					{
						return list.IndexOf(value, startIndex, count);
					}
				}
		public override int LastIndexOf(T value)
				{
					lock(SyncRoot)
					{
						return list.LastIndexOf(value);
					}
				}
		public override int LastIndexOf(T value, int startIndex)
				{
					lock(SyncRoot)
					{
						return list.LastIndexOf(value, startIndex);
					}
				}
		public override int LastIndexOf(T value, int startIndex, int count)
				{
					lock(SyncRoot)
					{
						return list.LastIndexOf(value, startIndex, count);
					}
				}

		// Implement the ICollection interface.
		public override void CopyTo(T[] array, int arrayIndex)
				{
					lock(SyncRoot)
					{
						list.CopyTo(array, arrayIndex);
					}
				}
		public override int Count
				{
					get
					{
						lock(SyncRoot)
						{
							return list.count;
						}
					}
				}
		public override bool IsSynchronized
				{
					get
					{
						return true;
					}
				}
		public override Object SyncRoot
				{
					get
					{
						return list.SyncRoot;
					}
				}

		// Copy from this array list to another array.
		public override void CopyTo(T[] array)
				{
					lock(SyncRoot)
					{
						list.CopyTo(array);
					}
				}
		public override void CopyTo(int index, T[] array,
							   	    int arrayIndex, int count)
				{
					lock(SyncRoot)
					{
						list.CopyTo(index, array, arrayIndex, count);
					}
				}

		// Reverse the contents of this array list.
		public override void Reverse()
				{
					lock(SyncRoot)
					{
						list.Reverse();
					}
				}
		public override void Reverse(int index, int count)
				{
					lock(SyncRoot)
					{
						list.Reverse(index, count);
					}
				}

		// Sort the contents of this array list.
		public override void Sort()
				{
					lock(SyncRoot)
					{
						list.Sort();
					}
				}
		public override void Sort(IComparer<T> comparer)
				{
					lock(SyncRoot)
					{
						list.Sort(comparer);
					}
				}
		public override void Sort(int index, int count, IComparer<T> comparer)
				{
					lock(SyncRoot)
					{
						list.Sort(index, count, comparer);
					}
				}

		// Create an array that contains the elements of this array list.
		public override T[] ToArray()
				{
					lock(SyncRoot)
					{
						return list.ToArray();
					}
				}
		public override Array ToArray(Type type)
				{
					lock(SyncRoot)
					{
						return list.ToArray(type);
					}
				}

		// Trim the array list to its actual size.
		public override void TrimToSize()
				{
					lock(SyncRoot)
					{
						list.TrimToSize();
					}
				}

		// Get or set the current capacity of the array list.
		public override int Capacity
				{
					get
					{
						lock(SyncRoot)
						{
							return list.Capacity;
						}
					}
					set
					{
						lock(SyncRoot)
						{
							list.Capacity = value;
						}
					}
				}

		// Get an enumerator for this array list.
		public override IEnumerator<T> GetEnumerator()
				{
					lock(SyncRoot)
					{
						return new SynchronizedEnumerator<T>
							(SyncRoot, list.GetEnumerator());
					}
				}
		public override IEnumerator<T> GetEnumerator(int index, int count)
				{
					lock(SyncRoot)
					{
						return new SynchronizedEnumerator<T>
							(SyncRoot, list.GetEnumerator(index, count));
					}
				}

		// Get an iterator for this array list.
		public override IIterator<T> GetIterator()
				{
					lock(SyncRoot)
					{
						return new SynchronizedIterator<T>
							(SyncRoot, list.GetIterator());
					}
				}
		public override IIterator<T> GetIterator(int index, int count)
				{
					lock(SyncRoot)
					{
						return new SynchronizedIterator<T>
							(SyncRoot, list.GetIterator(index, count));
					}
				}

	}; // class SynchronizedWrapper<T>

}; // class ArrayList<T>

}; // namespace Generics