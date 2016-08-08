using UnityEngine;
using System.Collections;

public class PathEdge<T> {
	public PathNode<T> node;

	public float cost; //cost to traverse this edge 
}
