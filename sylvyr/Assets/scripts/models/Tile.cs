using UnityEngine;
using System.Collections.Generic;
using System;


public enum TileType { EMPTY = -1,
					   FLOOR = 3};

public enum TileObjectType{EMPTY= -1,
						   WALL = 0}

public delegate void tile_changed_handler(Tile tile);

public class Tile {

	private int _id = -1;

	public int id{ get{return _id; } set{ _id = value; } }


	TileType _type = TileType.EMPTY;

	public event tile_changed_handler on_tile_changed;

	public TileType Type {
		get {
			return _type;
		}
		set {
			TileType old_type = _type;
			_type = value;

			if(on_tile_changed != null && old_type != _type)
				on_tile_changed (this);
		}
	}

	LooseObject loose_object;
	Feature _feature = null;

	public float movement_cost{
		get{
			if (this._type == TileType.EMPTY)
				return 0f;// 0 is unwalkable
			if (this._feature == null)
				return 1f;
			else
				return 1f * this._feature.movement_cost;
		}
	}

	//FIXME: this is a horrible idea...
	public Job pending_job;

	public Feature feature{ get{return _feature; } protected set{feature = value; }}


	//World world;

	int x;

	public int X {
		get {
			return x;
		}
	}

	int y;

	public int Y {
		get {
			return y;
		}
	}

	World _world;

	public Tile(World world, int x, int y){
		this._world = world;
		this.x = x;
		this.y = y;
	}

	public bool install_feature(Feature feature){
		if (feature == null) {
			this._feature = null;
			return true;
		}

		if (this._feature != null) {
			Debug.LogError ("already existing feature: "+ feature.type.ToString());
			return false;
		}

		this._feature = feature;
		return true;
	}

	public bool has_feature(FeatureType feature_type){
		if (this._feature == null)
			return false;
		
		return this._feature.type == feature_type;
	}


	public bool is_neighbor(Tile tile, bool check_diagonal=false){

		if (Mathf.Abs (this.x - tile.x) + Mathf.Abs (this.y - tile.y) == 1)
			return true;
		
		if (check_diagonal) {
			if ((Mathf.Abs (this.x - tile.x) == 1) && (Mathf.Abs (this.y - tile.y) == 1))
				return true;
		}

		return false;
	}

	public Vector2 get_position2(){
		return new Vector2 (this.x, this.y);
	}

	public List<Tile> get_neighbors(){//
		List<Tile> neighbors = new List<Tile> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue; //ignore yourself

				Tile tile = this._world.get_tile_at (this.x + x, this.y + y);

				if (tile == null)
					continue; //ignore non-existing/out of bounds tiles

				// neighbor exists, so add it to list
				neighbors.Add (tile);
			}
		}

		return neighbors;
	}

	public List<Tile> get_safe_neighbors(bool check_diagonal=false){
		List<Tile> neighbors = this.get_neighbors ();
		List<Tile> safe = new List<Tile> ();
		foreach (Tile neighbor in neighbors) {
			//FIXME: may be broken in future
			if (neighbor.has_feature (FeatureType.WALL) == true)
				continue;
			//validate the type of neighbor
			if (this.is_neighbor (neighbor, check_diagonal))
				safe.Add (neighbor);
		}

		return safe;
	}

	public Tile get_closest_safe_neighbor(Tile destination, bool check_diagonal=false){
		//get our safe neighbors
		List<Tile> safe_tiles = destination.get_safe_neighbors (check_diagonal);
		Tile closest = safe_tiles[0];//set it to first just in case
		float min_dist = Mathf.Infinity;

		//find the closest tile in the safe tiles
		foreach (Tile tile in safe_tiles) {
			float new_dist = Vector2.Distance (tile.get_position2 (), this.get_position2 ());
			if (new_dist < min_dist) {
				closest = tile;
				min_dist = new_dist;
			}
		}
		//return whatever we found
		return closest;
	}
}
