using UnityEngine;
using System.Collections;


public enum FeatureType {EMPTY=-1, WALL=0, DOOR, FURNATURE}

public delegate void feature_changed_handler(Feature feature);
public delegate bool position_validation_handler(Tile tile);

public class Feature {

	int _id = -1;

	public int id{ get{return _id; } set{ _id = value; } }

	public Tile tile{ get; protected set;}

	public event feature_changed_handler on_feature_changed;
	public event position_validation_handler on_position_validation;

	FeatureType _type;

	public FeatureType type{ 
		get{ return _type; }
		set{FeatureType old_type = _type;
			_type = value;

			if(on_feature_changed != null && old_type != _type)
				on_feature_changed (this); }
	}

	//this is a speed multiplier
	public float movement_cost{get; protected set;}

	int width;
	int height;

	bool links_to_neighbor;


	protected Feature(){
	}

	public static Feature create_prototype(FeatureType feature_type, float movement_cost=1f, int width=1, int height=1, bool links_to_neighbor=false){
		Feature feature = new Feature (); 

		feature._type = feature_type;
		feature.movement_cost = movement_cost;
		feature.width = width;
		feature.height = height;
		feature.links_to_neighbor = links_to_neighbor;

		feature.on_position_validation = feature.is_placement_valid; 

		return feature;
	}

	public static Feature place_feature(Feature proto, Tile tile){
		if (proto.on_position_validation (tile) == false) {
			Debug.LogError ("invalid position for feature");
			return null; 
		}

		Feature feature = new Feature (); 

		feature._type = proto._type;
		feature.movement_cost = proto.movement_cost;
		feature.width = proto.width;
		feature.height = proto.height;
		feature.links_to_neighbor = proto.links_to_neighbor;
		feature.tile = tile;
		feature.id = WorldController.instance.world.feature_ids.next ();


		//FIXME: this assumes a 1x1 obj
		if (tile.install_feature(feature) == false) {
			//something was there... we cant overwrite

			//we do not place an object
			return null;
		}

		feature.trigger_neighbor_update (feature);

		return feature;
	}

	void trigger_neighbor_update(Feature feature){
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;
				Feature feat = WorldController.instance.world.get_tile_at (feature.tile.X+x, feature.tile.Y+y).feature;
				if (feat == null)
					continue;
				feat.on_feature_changed (feat);
			}
		}
	}


	public bool is_placement_valid(Tile tile){
		//check type
		switch (tile.Type) {
		case TileType.FLOOR:
			break;
		case TileType.EMPTY:
			return false;
		default:
			return false;
		}

		if (tile.feature != null)
			return false;


		return true;
	}

	public bool is_door_placement_valid(Tile tile){
		if (is_placement_valid (tile) == false)
			return false;

		return true;
	}

}
