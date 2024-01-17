using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BetterList<T>
{
	[CompilerGenerated]
	private sealed class GetEnumeratorcIterator9<T> : IEnumerator, IDisposable, IEnumerator<T>
	{
		internal T Scurrent;

		internal int SPC;

		internal BetterList<T> fthis;

		internal int i0;

		T IEnumerator<T>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.Scurrent;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.Scurrent;
			}
		}

		[DebuggerHidden]
		public void Dispose()
		{
			this.SPC = -1;
		}

		public bool MoveNext()
		{
			uint sPC = (uint)this.SPC;
			this.SPC = -1;
			if (sPC != 0)
			{
				if (sPC != 1)
				{
					goto IL_007a;
				}
				this.i0++;
			}
			else
			{
				if (this.fthis.buffer == null)
				{
					goto IL_0073;
				}
				this.i0 = 0;
			}
			if (this.i0 < this.fthis.size)
			{
				this.Scurrent = this.fthis.buffer[this.i0];
				this.SPC = 1;
				return true;
			}
			goto IL_0073;
			IL_0073:
			this.SPC = -1;
			goto IL_007a;
			IL_007a:
			return false;
		}

		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public T[] buffer;

	public int size;

	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		this.buffer[this.size++] = item;
	}

	private void AllocateMore()
	{
		T[] array = ((this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)]);
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	public void Clear()
	{
		this.size = 0;
	}

	public bool Contains(T item)
	{
		if (this.buffer != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				if (this.buffer[i].Equals(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	[DebuggerHidden]
	public IEnumerator<T> GetEnumerator()
	{
		//yield-return decompiler failed: Could not find currentField
		return new GetEnumeratorcIterator9<T>
		{
			fthis = this
		};
	}

	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index < this.size)
		{
			for (int num = this.size; num > index; num--)
			{
				this.buffer[num] = this.buffer[num - 1];
			}
			this.buffer[index] = item;
			this.size++;
		}
		else
		{
			this.Add(item);
		}
	}

	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			T result = this.buffer[--this.size];
			this.buffer[this.size] = default(T);
			return result;
		}
		return default(T);
	}

	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if (this.buffer != null && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
		}
	}

	public void Sort(Comparison<T> comparer)
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 1; i < this.size; i++)
			{
				if (comparer(this.buffer[i - 1], this.buffer[i]) > 0)
				{
					T val = this.buffer[i];
					this.buffer[i] = this.buffer[i - 1];
					this.buffer[i - 1] = val;
					flag = true;
				}
			}
		}
	}

	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
			}
		}
		else
		{
			this.buffer = null;
		}
	}
}
