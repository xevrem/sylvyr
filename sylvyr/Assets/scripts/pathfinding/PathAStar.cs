using UnityEngine;
using System.Collections.Generic;

public class PathAStar {

	Stack<PathNode<Tile>> path;

	public bool failed = false;

	public PathAStar(World world, Tile start_tile, Tile end_tile){

		//all walkable nodes
		PathTileGraph tile_graph = world.get_tile_graph ();

		if (tile_graph == null) {
			Debug.LogError ("the tile graph does not exist");
			this.failed = true;
			return;
		}

		PathNode<Tile> start_node = tile_graph.get_node_by_tile (start_tile);
		PathNode<Tile> end_node = tile_graph.get_node_by_tile (end_tile);

		//ensure start end end tiles are in the graph
		if (start_node == null) {
			Debug.LogError ("tile graph does not contain starting tile");
			this.failed = true;
			return;
		}

		if (end_node == null) {
			this.failed = true;
			Debug.LogError ("tile graph does not contain ending tile");
			return;
		}

		List<PathNode<Tile>> closed_set = new List<PathNode<Tile>> ();
		//List<PathNode<Tile>> open_set = new List<PathNode<Tile>> ();
		BinaryHeap<PathNode<Tile>> open_set = new BinaryHeap<PathNode<Tile>>();

		Dictionary<PathNode<Tile>, PathNode<Tile>> came_from = new Dictionary<PathNode<Tile>, PathNode<Tile>> ();

		//set the base g-score for each node
		Bag<float> g_scores = new Bag<float> ();
		for (int i = 0; i < tile_graph.num_nodes(); i++) {
			PathNode<Tile> node = tile_graph.get_node_by_id(i);

			if (node == null)
				continue;

			g_scores.set (node.data.id, Mathf.Infinity);
		}
		//set g-score of starting node
		g_scores.set(start_tile.id, 0f);

		//set the base f-scores for each node
		Bag<float> f_scores = new Bag<float> ();
		for (int i = 0; i < tile_graph.num_nodes(); i++) {
			PathNode<Tile> node = tile_graph.get_node_by_id(i);

			if (node == null)
				continue;

			f_scores.set (node.data.id, Mathf.Infinity);
		}

		//set f-score of starting node
		f_scores.set(start_node.data.id, heuristic_cost_estimate(start_node, end_node));

		//add start node to open set
		open_set.add (f_scores [start_node.data.id], start_node);

		while (open_set.size > 0) {
			HeapCell<PathNode<Tile>> current = open_set.remove_first ();

			if (current.data.data == end_node.data) {
				//we found the path, build it and return
				reconstruct_path (came_from, end_node);
				return;
			}

			//add current to closed set
			closed_set.Add (current.data);

			if (current.data.edges == null) {
				Debug.LogError ("PathAStar :: Bad Edges...");
				continue;
			}
			
			foreach (PathEdge<Tile> neighbor in current.data.edges) {
				//continue if this edge is already in closed set
				if (closed_set.Contains (neighbor.node) == true)
					continue;

				float tentative_g_score = g_scores [current.data.data.id] + dist_between (current.data, neighbor.node);

				int pos = heap_contains (neighbor.node, open_set);
				if (pos < 0) {
					//f_scores.set (neighbor.node.data.id, tentative_g_score + heuristic_cost_estimate (neighbor.node, end_node));
					//g_scores.set (neighbor.node.data.id, tentative_g_score);
					open_set.add (f_scores [neighbor.node.data.id], neighbor.node);
				} else if (tentative_g_score >= g_scores [neighbor.node.data.id])
					continue; // not the best path

				//set where you came from
				if (came_from.ContainsKey (neighbor.node) == false)				
					came_from.Add (neighbor.node, current.data);
				else {
					came_from [neighbor.node] = current.data;
				}
				
				g_scores.set (neighbor.node.data.id, tentative_g_score);
				f_scores.set (neighbor.node.data.id, tentative_g_score + heuristic_cost_estimate (neighbor.node, end_node));
				open_set.add (f_scores [neighbor.node.data.id], neighbor.node);
			}

		}

		//failure
		failed = true;
	}

	public Tile get_next_tile(){
		if (path.Count > 0)
			return path.Pop ().data;
		else
			return null;
	}

	float heuristic_cost_estimate(PathNode<Tile> node, PathNode<Tile> goal){
		return Vector2.Distance (node.data.get_position2 (), goal.data.get_position2 ());
	}

	float dist_between(PathNode<Tile> a, PathNode<Tile> b){
		int i = a.data.X - b.data.X;
		int j = a.data.Y - b.data.Y;

		if ((i == -1 && j == -1) || (i == -1 && j == 1) || (i == 1 && j == -1) || (i == 1 && j == 1))
			return 1.41f;
		else
			return 1f;
	}

	int heap_contains(PathNode<Tile> node, BinaryHeap<PathNode<Tile>> heap)
	{
		for (int i = 1; i < heap.size; i++)
		{
			if (node.data.id == heap[i].data.data.id)
				return i;
		}

		return -1;
	}

	void reconstruct_path(Dictionary<PathNode<Tile>, PathNode<Tile>> came_from, PathNode<Tile> current){
		Stack<PathNode<Tile>> total_path = new Stack<PathNode<Tile>>();
		total_path.Push (current);

		while (came_from.ContainsKey (current)) {
			total_path.Push (came_from [current]);
			current = came_from [current];
		}

		this.path = total_path;
	}
}
