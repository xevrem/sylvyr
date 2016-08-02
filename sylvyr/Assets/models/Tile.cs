using UnityEngine;
using System.Collections;
using System;


public enum TileType { EMPTY = -1,
					   FLOOR = 3};

public delegate void tile_type_change_handler(Tile tile);

public class Tile {

	public int id{ get; set; }

	TileType type = TileType.EMPTY;

	public event tile_type_change_handler on_tile_type_change;

	public TileType Type {
		get {
			return type;
		}
		set {
			TileType old_type = type;
			type = value;

			if(on_tile_type_change != null && old_type != type)
				on_tile_type_change (this);
		}
	}

	LooseObject loose_object;
	InstsalledObject installed_object;

	World world;

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
		this.world = world;
		this.x = x;
		this.y = y;
	}
}
