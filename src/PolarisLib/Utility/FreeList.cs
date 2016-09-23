using System;

namespace Polaris.Lib.Utility
{
	public class FreeList<T>
	{
		private struct FreeNode
		{
			public bool isFree;
			public object data;
		}

		public readonly int MaxSize;
		public int CurrentSize { get; private set; }
		public int FreeSpace { get { return MaxSize - CurrentSize; } }

		private FreeNode[] _data;
		private int _firstFree;
		private int _lastFree;

		public FreeList(int size)
		{
			_data = new FreeNode[size];
			for (int i = 0; i < size; i++)
			{
				_data[i].data = i+1;
				_data[i].isFree = true;
			}

			_firstFree = 0;
			_lastFree = size-1;

			_data[_lastFree].data = -1;

			MaxSize = size;
			CurrentSize = 0;
		}

		/// <summary>
		/// Add an object to the FreeList.
		/// </summary>
		/// <param name="obj">Object of type T to add</param>
		/// <returns>Index of object</returns>
		public int Add(T obj)
		{
			int retVal = _firstFree;
			if (_firstFree >= 0)
			{
				int newLocation = (int)(_data[_firstFree].data);
				_data[_firstFree].data = obj;
				_data[_firstFree].isFree = false;
				_firstFree = newLocation;
				CurrentSize++;
			}
			return retVal;
		}

		/// <summary>
		/// Remove an element at a specific index
		/// </summary>
		/// <param name="idx">Index of the element to remove</param>
		public void Remove(int idx)
		{
			if (_data[idx].isFree)
				return;

			if (_data[_lastFree].isFree)
				_data[_lastFree].data = idx;
			else
				_firstFree = idx;

			_data[idx].isFree = true;
			_data[idx].data = -1;
			_lastFree = idx;
			CurrentSize--;
		}

		public bool IsInitialized(int idx)
		{
			return !(_data[idx].isFree);
		}

		public T this[int idx]
		{
			get
			{
				if (_data[idx].isFree)
					throw new IndexOutOfRangeException("Attempt to access uninitialized object.");
				return (T)_data[idx].data;
			}

			set
			{
				if (_data[idx].isFree)
					throw new IndexOutOfRangeException("Attempt to access uninitialized object. Use Add method to add new object.");
				_data[idx].data = value;
			}
		}


	}
}
