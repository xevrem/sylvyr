using UnityEngine;
using System.Collections.Generic;

public class FeatureSpriteController : MonoBehaviour {

	Bag<GameObject> feature_game_objects;

	World world {
		get{return WorldController.instance.world;}
	}

	// Use this for initialization
	void Start () {
		feature_game_objects = new Bag<GameObject> ();

		//when a new feature is created, handle it
		world.on_feature_created += handle_feature_created;
	}

	//handles a change of feature type
	void handle_feature_change(Feature feature){
		GameObject feature_go = feature_game_objects[feature.id];

		switch (feature.type) {
		case FeatureType.WALL:
			int index = feature_neighbor_count (feature);
			feature_go.GetComponent<SpriteRenderer>().sprite = ResourcePool.get_proper_feature_sprite(feature.type, index);
			break;
		default:
			break;
		}
	}
		
	//hande a craeted feature and associate an appropriate game object
	public void handle_feature_created(Feature feature){
		//create a visual GameObject linked to this feature data

		//FIXME: doesnt consider multi-tiles or rotations...

		//create the GO
		GameObject feature_go = new GameObject ();
		feature_go.name = "feature_"+ feature.type + "_" + feature.id;
		feature_go.transform.position = new Vector3 (feature.tile.X, feature.tile.Y, 0f);
		feature_go.transform.SetParent (this.transform, true);

		//add a sprite renderer
		feature_go.AddComponent<SpriteRenderer> ();
		feature_go.GetComponent<SpriteRenderer>().sortingLayerName = "features";
		int index = feature_neighbor_count (feature);
		feature_go.GetComponent<SpriteRenderer>().sprite = ResourcePool.get_proper_feature_sprite(feature.type, index);

		feature_game_objects.set (feature.id, feature_go);

		feature.on_feature_changed += handle_feature_change;
	}

//	//resolves all features to use appropriate sprite based on neighboring features
//	public void resolve_feature_tiles(){
//		//FIXME: should only resolve tiles in immediate vicinity
//		for (int i = 0; i < feature_game_objects.count; i++) {
//			GameObject go = feature_game_objects[i];
//			if (go == null)
//				continue;
//			Feature feature = world.get_feature_by_id (i);
//			int index = feature_neighbor_count (feature);
//			go.GetComponent<SpriteRenderer>().sprite = ResourcePool.get_proper_feature_sprite(feature.type, index);
//		}
//	}
//
	//gives the appropriate binary count of neighboring Features like the given Feature
	int feature_neighbor_count(Feature feature){

		int n = matching_neighbor (0, 1, feature) ? 1 : 0;
		int w = matching_neighbor (-1, 0, feature) ? 2 : 0;
		int s = matching_neighbor (0, -1, feature) ? 4 : 0;
		int e = matching_neighbor (1, 0, feature) ? 8 : 0;

		return n+w+s+e;
	}

	//tests if the offset tile has a matching feature
	bool matching_neighbor(int x, int y, Feature feature){
		Tile t = world.get_tile_at (feature.tile.X + x, feature.tile.Y + y);

		if (t == null)
			return false;

		return t.has_feature (feature.type);
	}

}
