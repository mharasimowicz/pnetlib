/*
 * ArrayQueue.cs - Generic queue class, implemented as an array.
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

public class ArrayQueue<T> : IQueue<T>
{
	// Internal state.
	private T[]   items;
	private int   add, remove, size;
	private float growFactor;
	private int   generation;

	// The default capacity for queues.
	private const int DefaultCapacity = 32;

	// Constructors.
	public ArrayQueue()
			{
				items = new Object [DefaultCapacity];
				add = 0;
				remove = 0;
				size = 0;
				growFactor = 2.0f;
				generation = 0;
			}
	public ArrayQueue(int capacity)
			{
				if(capacity < 0)
				{
					throw new ArgumentOutOfRangeException
						("capacity", S._("ArgRange_NonNegative"));
				}
				items = new T [capacity];
				add = 0;
				remove = 0;
				size = 0;
				growFactor = 2.0f;
				generation = 0;
			}
	public ArrayQueue(int capacity, float growFactor)
			{
				if(capacity < 0)
				{
					throw new ArgumentOutOfRangeException
						("capacity", S._("ArgRange_NonNegative"));
				}
				if(growFactor < 1.0f || growFactor > 10.0f)
				{
					throw new ArgumentOutOfRangeException
						("growFactor", S._("ArgRange_QueueGrowFactor"));
				}
				items = new T [capacity];
				add = 0;
				remove = 0;
				size = 0;
				this.growFactor = growFactor;
				generation = 0;
			}
	public ArrayQueue(ICollection<T> col)
			{
				if(col == null)
				{
					throw new ArgumentNullException("col");
				}
				items = new T [col.Count];
				col.CopyTo(items, 0);
				add = 0;
				remove = 0;
				size = items.Length;
				growFactor = 2.0f;
				generation = 0;
			}

	// Implement the ICollection<T> interface.
	public virtual void CopyTo(T[] array, int index)
			{
				if(array == null)
				{
					throw new ArgumentNullException("array");
				}
				else if(index < 0)
				{
					throw new ArgumentOutOfRangeException
						("index", S._("ArgRange_Array"));
				}
				else if((array.Length - index) < size)
				{
					throw new ArgumentException(S._("Arg_InvalidArrayRange"));
				}
				else if(size > 0)
				{
					if((remove + size) <= items.Length)
					{
						Array.Copy(items, remove, array, index, size);
					}
					else
					{
						Array.Copy(items, remove, array, index,
								   items.Length - remove);
						Array.Copy(items, 0, array,
								   index + items.Length - remove, add);
					}
				}
			}
	public virtual int Count
			{
				get
				{
					return size;
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

	// Implement the ICloneable<T> interface.
	public virtual Object Clone()
			{
				ArrayQueue<T> queue = (ArrayQueue<T>)MemberwiseClone();
				queue.items = (T[])items.Clone();
				return queue;
			}

	// Implement the IEnumerable<T> interface.
	public virtual IEnumerator<T> GetEnumerator()
			{
				return new QueueEnumerator<T>(this);
			}

	// Implement the IIterable<T> interface.
	public virtual IIterable<T> GetIterator()
			{
				return new QueueIterator<T>(this);
			}

	// Determine if this queue is read-only.
	public virtual bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

	// Clear the contents of this queue.
	public virtual void Clear()
			{
				add = 0;
				remove = 0;
				size = 0;
				++generation;
			}

	// Determine if this queue contains a specific object.
	public virtual bool Contains(T obj)
			{
				int index = remove;
				int capacity = items.Length;
				int count = size;
				if(typeof(T).IsValueType)
				{
					while(count > 0)
					{
						if(obj.Equals(items[index]))
						{
							return true;
						}
						index = (index + 1) % capacity;
						--count;
					}
				}
				else
				{
					while(count > 0)
					{
						if(items[index] != null && obj != null)
						{
							if(obj.Equals(items[index]))
							{
								return true;
							}
						}
						else if(items[index] == null && obj == null)
						{
							return true;
						}
						index = (index + 1) % capacity;
						--count;
					}
				}
				return false;
			}

	// Implement the IQueue<T> interface.
	public virtual void Enqueue(T value)
			{
				if(size < items.Length)
				{
					// The queue is big enough to hold the new item.
					items[add] = value;
					add = (add + 1) % items.Length;
					++size;
				}
				else
				{
					// We need to increase the size of the queue.
					int newCapacity = (int)(items.Length * growFactor);
					if(newCapacity <= items.Length)
					{
						newCapacity = items.Length + 1;
					}
					T[] newItems = new T [newCapacity];
					if(remove < size)
					{
						Array.Copy(items, remove, newItems, 0, size - remove);
					}
					if(remove > 0)
					{
						Array.Copy(items, 0, newItems, size - remove, remove);
					}
					items = newItems;
					add = size;
					remove = 0;
					items[add] = value;
					add = (add + 1) % items.Length;
					++size;
				}
				++generation;
			}
	public virtual T Dequeue()
			{
				if(size > 0)
				{
					T value = items[remove];
					remove = (remove + 1) % items.Length;
					--size;
					++generation;
					return value;
				}
				else
				{
					throw new InvalidOperationException
						(S._("Invalid_EmptyQueue"));
				}
			}
	public virtual T Peek()
			{
				if(size > 0)
				{
					return items[remove];
				}
				else
				{
					throw new InvalidOperationException
						(S._("Invalid_EmptyQueue"));
				}
			}
	public virtual T[] ToArray()
			{
				T[] array = new T [size];
				if(size > 0)
				{
					if((remove + size) <= items.Length)
					{
						Array.Copy(items, remove, array, 0, size);
					}
					else
					{
						Array.Copy(items, remove, array, 0,
								   items.Length - remove);
						Array.Copy(items, 0, array,
								   items.Length - remove, add);
					}
				}
				return array;
			}

	// Convert this queue into a synchronized queue.
	public static ArrayQueue<T> Synchronized(ArrayQueue<T> queue)
			{
				if(queue == null)
				{
					throw new ArgumentNullException("queue");
				}
				else if(queue.IsSynchronized)
				{
					return queue;
				}
				else
				{
					return new SynchronizedQueue<T>(queue);
				}
			}

	// Private class that implements synchronized queues.
	private class SynchronizedQueue : Queue
	{
		// Internal state.
		private ArrayQueue<T> queue;

		// Constructor.
		public SynchronizedQueue(ArrayQueue<T> queue)
				{
					this.queue = queue;
				}

		// Implement the ICollection<T> interface.
		public override void CopyTo(T[] array, int index)
				{
					lock(SyncRoot)
					{
						queue.CopyTo(array, index);
					}
				}
		public override int Count
				{
					get
					{
						lock(SyncRoot)
						{
							return queue.Count;
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
						return queue.SyncRoot;
					}
				}

		// Implement the ICloneable interface.
		public override Object Clone()
				{
					return new SynchronizedQueue<T>
						((ArrayQueue<T>)(queue.Clone()));
				}

		// Implement the IEnumerable<T> interface.
		public override IEnumerator<T> GetEnumerator()
				{
					lock(SyncRoot)
					{
						return new SynchronizedEnumerator<T>
							(SyncRoot, queue.GetEnumerator());
					}
				}

		// Clear the contents of this queue.
		public override void Clear()
				{
					lock(SyncRoot)
					{
						queue.Clear();
					}
				}

		// Determine if this queue contains a specific object.
		public override bool Contains(T obj)
				{
					lock(SyncRoot)
					{
						return queue.Contains(obj);
					}
				}

		// Dequeue an item.
		public override T Dequeue()
				{
					lock(SyncRoot)
					{
						return queue.Dequeue();
					}
				}

		// Enqueue an item.
		public override void Enqueue(T obj)
				{
					lock(SyncRoot)
					{
						queue.Enqueue(obj);
					}
				}

		// Peek at the first item without dequeuing it.
		public override T Peek()
				{
					lock(SyncRoot)
					{
						return queue.Peek();
					}
				}

		// Convert the contents of this queue into an array.
		public override T[] ToArray()
				{
					lock(SyncRoot)
					{
						return queue.ToArray();
					}
				}

	}; // class SynchronizedQueue

	// Private class for implementing queue enumeration.
	private class QueueEnumerator<T> : IEnumerator<T>
	{
		// Internal state.
		private ArrayQueue<T> queue;
		private int generation;
		private int position;

		// Constructor.
		public QueueEnumerator(ArrayQueue<T> queue)
				{
					this.queue = queue;
					generation = queue.generation;
					position   = -1;
				}

		// Implement the IEnumerator<T> interface.
		public bool MoveNext()
				{
					if(generation != queue.generation)
					{
						throw new InvalidOperationException
							(S._("Invalid_CollectionModified"));
					}
					++position;
					if(position < queue.size)
					{
						return true;
					}
					position = queue.size;
					return false;
				}
		public void Reset()
				{
					if(generation != queue.generation)
					{
						throw new InvalidOperationException
							(S._("Invalid_CollectionModified"));
					}
					position = -1;
				}
		public T Current
				{
					get
					{
						if(generation != queue.generation)
						{
							throw new InvalidOperationException
								(S._("Invalid_CollectionModified"));
						}
						if(position < 0 || position >= queue.size)
						{
							throw new InvalidOperationException
								(S._("Invalid_BadEnumeratorPosition"));
						}
						return queue.items
							[(queue.remove + position) % queue.size];
					}
				}

	}; // class QueueEnumerator<T>

	// Private class for implementing queue iteration.
	private class QueueIterator<T> : IIterator<T>
	{
		// Internal state.
		private ArrayQueue<T> queue;
		private int position;
		private bool reset;

		// Constructor.
		public QueueIterator(ArrayQueue<T> queue)
				{
					this.queue = queue;
					position = -1;
					reset = true;
				}

		// Implement the IEnumerator<T> interface.
		public bool MoveNext()
				{
					if(reset)
					{
						position = 0;
						reset = false;
					}
					else
					{
						++position;
					}
					return (position < queue.size);
				}
		public void Reset()
				{
					position = -1;
					reset = true;
				}
		T IEnumerator<T>.Current
				{
					get
					{
						if(position < 0 || position >= queue.size)
						{
							throw new InvalidOperationException
								(S._("Invalid_BadIteratorPosition"));
						}
						return queue.items
							[(queue.remove + position) % queue.size];
					}
				}

		// Implement the IIterator<T> interface.
		public bool MovePrev()
				{
					if(reset)
					{
						position = queue.size - 1;
						reset = false;
					}
					else
					{
						--position;
					}
					return (position >= 0);
				}
		public T Current
				{
					get
					{
						if(position < 0 || position >= queue.size)
						{
							throw new InvalidIteratorPosition
								(S._("Invalid_BadEnumeratorPosition"));
						}
						return queue.items
							[(queue.remove + position) % queue.size];
					}
					set
					{
						if(position < 0 || position >= queue.size)
						{
							throw new InvalidOperationException
								(S._("Invalid_BadIteratorPosition"));
						}
						queue.items[(queue.remove + position) % queue.size]
							= value;
					}
				}

	}; // class QueueIterator<T>

}; // class ArrayQueue<T>

}; // namespace Generics