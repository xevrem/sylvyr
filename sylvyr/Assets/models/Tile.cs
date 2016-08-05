using UnityEngine;
using System.Collections;
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

	public Tile(World world, int x, int y){
		//this.world = world;
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
}
