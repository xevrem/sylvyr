using UnityEngine;
using System.Collections;


public enum FeatureType {EMPTY=-1, WALL=0, DOOR, FURNATURE}

public delegate void feature_change_handler(Feature feature);

public class Feature {

	int _id = -1;

	public int id{ get{return _id; } set{ _id = value; } }

	public Tile tile{ get; protected set;}

	public event feature_change_handler on_feature_change;

	FeatureType _type;

	public FeatureType type{ 
		get{ return _type; }
		set{FeatureType old_type = _type;
			_type = value;

			if(on_feature_change != null && old_type != _type)
				on_feature_change (this); }
	}

	//this is a speed multiplier
	float movement_cost;

	int width;
	int height;


	protected Feature(){
	}

	public static Feature create_prototype(FeatureType feature_type, float movement_cost=1f, int width=1, int height=1){
		Feature feature = new Feature (); 

		feature._type = feature_type;
		feature.movement_cost = movement_cost;
		feature.width = width;
		feature.height = height;

		return feature;
	}

	public static Feature place_feature(Feature proto, Tile tile){
		Feature feature = new Feature (); 

		feature._type = proto._type;
		feature.movement_cost = proto.movement_cost;
		feature.width = proto.width;
		feature.height = proto.height;
		feature.tile = tile;
		feature.id = tile.id;

		//FIXME: this assumes a 1x1 obj
		if (tile.install_feature(feature) == false) {
			//something was there... we cant overwrite

			//we do not place an object
			return null;
		}

		return feature;
	}

}
