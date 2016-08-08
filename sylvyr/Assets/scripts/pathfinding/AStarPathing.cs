using UnityEngine;
using System.Collections.Generic;

public class Cell
{
	public Tile tile;
	public Vector2 position;
	public Cell parent;
	public int F = 0;
	public int G = 0;
	public int H = 0;
}


public class AStarPathing
{

	private PathTileGraph _tile_graph;
	private Vector2 _start,_destination;
	private Cell _start_cell, _destination_cell;
	/*private List<Cell> _OpenSet = new List<Cell>();
        
        public List<Cell> OpenSet
        {
            get { return _OpenSet; }
            set { _OpenSet = value; }
        }*/

	private BinaryHeap<Cell> _open_set = new BinaryHeap<Cell>(16);

	private List<Cell> _closed_set = new List<Cell>();

	private List<Cell> _blocking_set = new List<Cell>();

	private int linear_cost = 10;
	private int diagonal_cost = 14;
	private int _max_loops = 1000;

	private bool _failed = false;

	// did it fail?
	public bool failed
	{
		get { return _failed; }
		set { _failed = value; }
	}

	private bool _is_found = false;

	/// <summary>
	/// did it complete?
	/// </summary>
	public bool is_found
	{
		get { return _is_found; }
		set { _is_found = value; }
	}

	/// <summary>
	/// constructor
	/// </summary>
	/// <param name="start">starting point</param>
	/// <param name="finish">ending point</param>
	/// <param name="map">map to path through</param>
	public AStarPathing(Tile start, Tile destination, PathTileGraph tile_graph) 
	{
		_tile_graph = tile_graph;
		_start = start.get_position2 ();
		_destination = destination.get_position2();
		_start_cell = create_cell(start);
		_destination_cell = create_cell(destination);
		_open_set.add(_start_cell.F, _start_cell);

		List<Cell> temp = find_edges (_start_cell);//find_adjacent_cells(_start_cell);
		for (int i = 0; i < temp.Count; i++)
		{
			_open_set.add(temp[i].F,temp[i]);
		}

		_closed_set.Add(_open_set.remove_first().data);
	}

	/// <summary>
	/// main pathing loop
	/// </summary>
	public void find_path()
	{
		int loopCount = 0;
		List<Cell> working_list = new List<Cell>();
		Cell temp;

		while (loopCount < _max_loops)
		{
			//have we failed to find it?
			if (_open_set.size == 0)
			{
				if (contains(_destination_cell, _closed_set) < 0)
				{
					_failed = true;
					return;
				}
			}

			//find the current loswest cost square
			temp = _open_set.remove_first().data;

			_closed_set.Add(temp);

			int dest = contains(_destination_cell, _closed_set);

			//did we find it?
			if (dest >= 0)
			{
				_is_found = true;
				temp = _closed_set[dest];
				temp.parent = _closed_set[dest - 1];
				_closed_set[dest] = temp;
				return;
			}

			//get adjacent squares
			//working_list = find_adjacent_cells(temp);
			working_list = find_edges (temp);

			for (int i = 0; i < working_list.Count; i++)
			{
				_open_set.add(working_list[i].F,working_list[i]);
			}

			loopCount++;
		}
	}

	/// <summary>
	/// return the current path
	/// </summary>
	/// <returns>the current path</returns>
	public List<Cell> get_path()
	{
		int loop_count = 0;

		List<Cell> path = new List<Cell>();

		Cell next = _closed_set[contains(_destination_cell,_closed_set)];
		path.Add(next);

		while (loop_count < _closed_set.Count)
		{
			next = next.parent;

			if (next.position == _start_cell.position)
			{
				break;
			}
			path.Add(next);
		}

		List<Cell> final_path = new List<Cell>();

		for (int i = 0; i < path.Count; i++)
		{
			final_path.Add(path[path.Count - 1 - i]);
		}

		return final_path;
	}

	/// <summary>
	/// finds the least cost cell in the list
	/// </summary>
	/// <param name="list">list to search</param>
	/// <returns>least cost cell</returns>
	private Cell find_least_cost_cell(List<Cell> list)
	{
		Cell minCell = new Cell();
		int min = int.MaxValue;

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].F < min)
			{
				minCell = list[i];
				min = minCell.F;
			}
		}
		return minCell;
	}

	/// <summary>
	/// determines F cost for a cell
	/// </summary>
	/// <param name="cell">cell to determine</param>
	/// <returns>the F cost</returns>
	private int find_cost(Cell cell)
	{
		cell.F = cell.G + heuristic_cost(cell);

		return cell.F;
	}

	/// <summary>
	/// determines the Heuristic Cost for the cell
	/// </summary>
	/// <param name="cell">cell to determine</param>
	/// <returns>the Heuristic cost</returns>
	private int heuristic_cost(Cell cell)
	{
		int x, y;

		x = (int) Mathf.Abs(_destination.x - cell.position.x);
		y = (int) Mathf.Abs(_destination.y - cell.position.y);

		cell.H = (x + y) * 10;

		return cell.H;
	}

	private List<Cell> find_edges(Cell cell){
		PathEdge<Tile>[] edges = _tile_graph.get_node_by_tile (cell.tile).edges;
		Cell new_cell = new Cell ();

		for (int index = 0; index < edges.Length; index++) {
			PathEdge<Tile> edge = edges [index];

			//find relative position of edge
			int i = edge.node.data.X - cell.tile.X;
			int j = edge.node.data.Y - cell.tile.Y;

			//set its G cost since we can do that here
			if ((i == -1 && j == -1) || (i == -1 && j == 1) || (i == 1 && j == -1) || (i == 1 && j == 1))
				new_cell.G = diagonal_cost + cell.G;
			else
				new_cell.G = linear_cost + cell.G;	
		}


		return null;
	}

	/// <summary>
	/// finds the adjacent cells
	/// </summary>
	/// <param name="cell"></param>
	/// <returns></returns>
//	private List<Cell> find_adjacent_cells(Cell cell)
//	{
//		//setup temps
//		Tile tile;
//		List<Cell> good_adjacents = new List<Cell>();
//
//		//search ajacent tiles
//		for (int i = -1; i <= 1; i++)
//		{
//			for (int j = -1; j <= 1; j++)
//			{
//				//skip this cell since we already have it
//				if ((i == 0) && (j == 0))
//					continue;
//
//				//get the tile
//				//tile = _tile_graph.getTerrain((int) cell.position.X + i, (int)cell.position.Y + j);//);
//				tile = this._tile_graph;
//
//				//is terrain valid?
//				if (tile == null)
//					continue;
//
//				//create the cell
//				Cell new_cell = create_cell((int)cell.position.X + i, (int)cell.position.Y + j);
//
//				//if we've already got it, ignore it
//				if (contains(new_cell, _closed_set) >= 0)
//					continue;
//
//				//set its parent
//				new_cell.parent = cell;
//
//				//set its G cost since we can do that here
//				if ((i == -1 && j == -1) || (i == -1 && j == 1) || (i == 1 && j == -1) || (i == 1 && j == 1))
//					new_cell.G = diagonal_cost + cell.G;
//				else
//					new_cell.G = linear_cost + cell.G;	
//
//				//finds this cell's cost
//				new_cell.F = find_cost(new_cell);
//
//				//check to see if we already have this one on an open list
//				int pos = heap_contains(new_cell, _open_set);
//				if (pos >= 0)
//				{
//					Cell temp = _open_set[pos].data;
//
//					//is cell a better path?
//					if (cell.G + (temp.G - temp.parent.G) < temp.G)
//					{
//						temp.G -= temp.parent.G;
//
//						//yes, so update it, re-calc cost, and continue;
//						temp.parent = cell;
//
//						temp.G += temp.parent.G;
//
//						temp.F = find_cost(temp);
//
//						_open_set[pos] = new HeapCell<Cell>(temp.F,temp);
//						continue;
//					}
//					else
//						continue;
//				}
//
//				//add its location to the list as a good candidate
//				good_adjacents.Add(new_cell);
//			}
//		}
//
//		//return list
//		return good_adjacents;
//	}

	private Cell create_cell(Tile tile)
	{
		Cell cell = new Cell();
		cell.tile = tile;
		cell.position.x = tile.X;
		cell.position.y = tile.Y;
		return cell;
	}

	private int contains(Cell cell, List<Cell> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (cell.position == list[i].position)
				return i;
		}

		return -1;
	}

	private int heap_contains(Cell cell, BinaryHeap<Cell> heap)
	{
		for (int i = 1; i < heap.size; i++)
		{
			if (cell.position == heap[i].data.position)
				return i;
		}

		return -1;
	}

	private Cell get_cell(Vector2 position, List<Cell> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (position == list[i].position)
				return list[i];
		}
		return null;
	}

	private void remove_cell(Vector2 position, List<Cell> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (position == list[i].position)
				list.RemoveAt(i);
		}
	}


//	private bool cell_is_blocking(int x, int y)
//	{
//		Terrain terrain;
//		terrain = _tile_graph.getTerrain(x, y);
//		//is terrain valid?
//		if (terrain == null)
//			return true;
//		//is it blocking?
//		if (terrain.IsBlocking)
//			return true;
//		return false;
//	}

	/// <summary>
	/// reset
	/// </summary>
	/// <param name="start">starting point</param>
	/// <param name="finish">ending point</param>
	/// <param name="map">map to path through</param>
	public void reset(Tile start, Tile destination, PathTileGraph tile_graph)
	{
		reset();

		_tile_graph = tile_graph;
		_start = start.get_position2();
		_destination = destination.get_position2();
		_start_cell = create_cell(start);
		_destination_cell = create_cell(destination);

		_open_set.add(_start_cell.F,_start_cell);

		List<Cell> temp = find_edges (_start_cell);//find_adjacent_cells(_start_cell);
		for (int i = 0; i < temp.Count; i++)
		{
			_open_set.add(temp[i].F, temp[i]);
		}

		//_OpenSet.Remove(_StartCell);
		//_ClosedSet.Add(_StartCell);
		_closed_set.Add(_open_set.remove_first().data);

	}

	/// <summary>
	/// resets all lists and flags
	/// </summary>
	private void reset()
	{
		_open_set.clear();
		_closed_set.Clear();
		_blocking_set.Clear();


		_failed = false;
		_is_found = false;
	}

}