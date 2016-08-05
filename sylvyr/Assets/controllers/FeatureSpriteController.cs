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
		SpriteRenderer sr = feature_go.AddComponent<SpriteRenderer> ();
		sr.sortingLayerName = "features";
		sr.sprite = get_feature_sprite (feature);

		feature_game_objects.set (feature.id, feature_go);

		feature.on_feature_changed += handle_feature_change;
	}


	public static Sprite get_feature_sprite(Feature feature){
		int index = feature_neighbor_count (feature);
		return ResourcePool.get_proper_feature_sprite(feature.type, index);
	}

	//gives the appropriate binary count of neighboring Features like the given Feature
	static int feature_neighbor_count(Feature feature){

		int n = matching_neighbor (0, 1, feature) ? 1 : 0;
		int w = matching_neighbor (-1, 0, feature) ? 2 : 0;
		int s = matching_neighbor (0, -1, feature) ? 4 : 0;
		int e = matching_neighbor (1, 0, feature) ? 8 : 0;

		return n+w+s+e;
	}

	//tests if the offset tile has a matching feature
	static bool matching_neighbor(int x, int y, Feature feature){
		Tile t = WorldController.instance.world.get_tile_at (feature.tile.X + x, feature.tile.Y + y);

		if (t == null)
			return false;

		return t.has_feature (feature.type);
	}

	public static Sprite get_basic_feature_sprite(FeatureType feature_type){
		switch (feature_type) {
		case FeatureType.WALL:
			return ResourcePool.tile_sprites [(int)feature_type];
		default:
			return null;
		}
	}

}
