using UnityEngine;
using System.Collections;

public class HeapCell<T>
{
	public T data;

	public float value;

	public HeapCell(float value, T data)
	{
		this.value = value;
		this.data = data;
	}

	public HeapCell() { }
}