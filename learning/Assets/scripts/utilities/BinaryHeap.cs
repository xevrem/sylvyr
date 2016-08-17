using UnityEngine;
using System.Collections.Generic;
using System;

public class BinaryHeap<T>
{
	/// <summary>
	/// data structure
	/// </summary>
	private HeapCell<T>[] _data = new HeapCell<T>[0];

	private int _length;

	public int size{ get; private set;}

	public BinaryHeap()
	{
		size = 0;
		_length = 16 * 2 + 2;
		_data = new HeapCell<T>[_length];


		for (int i = 0; i < _length; i++)
		{
			_data[i] = new HeapCell<T>();
		}
	}

	public BinaryHeap(int length)
	{
		size = 0;
		_length = length * 2 + 2;
		_data = new HeapCell<T>[_length];

		for (int i = 0; i < _length; i++)
		{
			_data[i] = new HeapCell<T>();
		}
	}

	public HeapCell<T> this[int index]
	{
		get { return _data[index]; }
		set { _data[index] = value; }
	}

	/// <summary>
	/// adds data to the heap using the given sort-value
	/// </summary>
	/// <param name="value">value used to determine proper sort</param>
	/// <param name="data">data package to store</param>
	public void add(float value, T data)
	{
		add(new HeapCell<T>(value, data));
		return;
	}

	/// <summary>
	/// adds heapcell to the heap using the given sort-value
	/// </summary>
	/// <param name="cell">heapcell to be used</param>
	private void add(HeapCell<T> cell)
	{
		size++;

		if ((size * 2 + 1) >= _length)
			grow(size);

		_data[size] = cell;

		int i = size;

		//do any needed swapping
		while (i != 1)
		{
			//compare cells
			if (_data[i].value <= _data[i / 2].value)
			{
				//if i is less than i/2, swap
				HeapCell<T> temp = _data[i / 2];
				_data[i / 2] = _data[i];
				_data[i] = temp;
				i = i / 2;
			}
			else//otherwise break
				break;
		}
	}

	public HeapCell<T> remove_first()
	{
		HeapCell<T> retVal = _data[1];

		//move last item to 1st position, reduce size by 1
		_data[1] = _data[size];
		_data[size] = null;
		size--;

		int u, v;
		v = 1;

		//sort the heap
		while (true)
		{
			u = v;

			//if both children exist
			if ((2 * u + 1) <= size)
			{
				//select lowest child
				if (_data[u].value >= _data[2 * u].value)
					v = 2 * u;
				if (_data[v].value >= _data[2 * u + 1].value)
					v = 2 * u + 1;
			}//if only one child exists
			else if (2 * u <= size)
			{
				if (_data[u].value >= _data[2 * u].value)
					v = 2 * u;
			}

			//do we need to swap or exit?
			if (u != v)
			{
				HeapCell<T> temp = _data[u];
				_data[u] = _data[v];
				_data[v] = temp;
			}
			else
			{
				break;//we've re-sorted the heap, so exit
			}
		}

		return retVal;
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="size"></param>
	private void grow(int size)
	{
		int length = size * 2 + 2;
		HeapCell<T>[] data = new HeapCell<T>[length];

		Array.Copy(_data, data, _length);

		_data = data;
		_length = length;

		return;
	}

	public void clear()
	{
		for (int i = 0; i < size; i++)
		{
			_data[i] = null;
		}

		size = 0;
	}

	public override String ToString()
	{
		String str = "";

		for (int i = 1; i < size + 1; i++)
		{
			str += _data[i].value + ", ";
		}

		return str;
	}
}
