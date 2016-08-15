using UnityEngine;
using System.Collections.Generic;

public class PathTileGraph {


	Bag<PathNode<Tile>> _nodes;

	public PathTileGraph(World world){
		//loop through all tiles, create a node for each tile
		// dont create nodes for non-floors
		// dont create tiles for walls
		_nodes = new Bag<PathNode<Tile>>();

		for (int x = 0; x < world.width; x++) {
			for (int y = 0; y < world.height; y++) {
				Tile tile = world.get_tile_at (x, y);

				if (tile.movement_cost > 0) { // tiles with movement cost of 0 are unwalkable
					PathNode<Tile> node = new PathNode<Tile>();
					node.data = tile;

					_nodes.set (tile.id, node);
				}
					
			}
		}


		// create edges for each tile
		// create edges only for existing neighbors
		for (int i = 0; i < _nodes.count; i++) {
			List<PathEdge<Tile>> edges = new List<PathEdge<Tile>> ();

			//get tile neighbors for a path node
			//if neighbor is walkable, add an edge pointing to the neighboring node
			PathNode<Tile> node = _nodes[i];

			// ensure we grabbed a good node
			if (node == null)
				continue;
			
			foreach (Tile neighbor in node.data.get_neighbors()) {
				if (neighbor != null & neighbor.movement_cost > 0) {
					// neighbor exists and is walkable
					PathEdge<Tile> edge = new PathEdge<Tile>();
					edge.cost = neighbor.movement_cost;
					//FIXME: may cause issues if node is not in bag
					edge.node = _nodes[neighbor.id];
					edges.Add (edge);
				}
			}

			if (edges.Count > 0)
				node.edges = edges.ToArray ();
		}
	}

	public PathNode<Tile> get_node_by_tile(Tile tile){
		if (_nodes == null || tile == null)
			return null;

		if (tile.id < _nodes.count)
			return _nodes [tile.id];
		else
			return null;
	}

	public PathNode<Tile> get_node_by_id(int id){
		if (_nodes != null && id < _nodes.count)
			return _nodes [id];
		else
			return null;
	}

	public int num_nodes(){
		return _nodes.count;
	}

}
