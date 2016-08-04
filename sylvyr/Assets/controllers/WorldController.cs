using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {


	public static WorldController instance{ get; protected set; }

	public Sprite floor_sprite;

	public World world{ get; protected set; }

	Bag<GameObject> tile_game_objects;
	Bag<GameObject> feature_game_objects;

	// Use this for initialization
	void Start () {
		if (instance != null) {
			Debug.LogError("duplicate world controllers...");
		}
		instance = this;

		//create an empty world
		world = new World ();

		//when a new feature is created, handle it
		world.on_feature_created += handle_feature_created;

		//pre-size tile_game_objects
		tile_game_objects = new Bag<GameObject>();
		feature_game_objects = new Bag<GameObject> ();

		ResourcePool.load_all ();

		//create GameObject for each Tile
		create_tiles();

		world.randomize_tiles ();
	}

	void create_tiles(){
		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				//get the tile at this location
				Tile tile_data = world.get_tile_at (x, y);

				//create the GO
				GameObject tile_go = new GameObject ();
				tile_go.name = "tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (tile_data.X, tile_data.Y, 0f);
				tile_go.transform.SetParent (this.transform, true);

				//add a sprite renderer
				tile_go.AddComponent<SpriteRenderer> ();
				tile_go.GetComponent<SpriteRenderer>().sortingLayerName = "tiles";

				tile_game_objects.set(tile_data.id, tile_go);

				tile_data.on_tile_type_change += handle_tile_type_change;

			}
		}
	}



	//to be called when a Tile's TileType is changed
	void handle_tile_type_change(Tile tile_data){

		GameObject tile_go = tile_game_objects [tile_data.id];

		switch (tile_data.Type){
		case TileType.FLOOR:
			tile_go.GetComponent<SpriteRenderer> ().sprite = ResourcePool.tile_sprites [(int)TileType.FLOOR];
			break;
		case TileType.EMPTY:
			tile_go.GetComponent<SpriteRenderer> ().sprite = null;
			break;
		default:
			Debug.Log ("on_tile_type_changed - error bad TileType");
			break;
		}
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

	public Tile get_tile_at_world_coordinate(Vector3 coord){
		int x = Mathf.FloorToInt (coord.x);
		int y = Mathf.FloorToInt (coord.y);

		return WorldController.instance.world.get_tile_at (x, y); 
	}

	public void destroy_all_tile_game_objects(){
		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {

				//get the tile and its associated GO at this location
				Tile tile_data = world.get_tile_at (x, y);
				GameObject go = tile_game_objects.remove (tile_data.id);

				//de-register the handler
				tile_data.on_tile_type_change -= handle_tile_type_change;

				//destroy the GO
				Destroy (go);
			}
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

		feature.on_feature_change += handle_feature_change;
	}

	//resolves all features to use appropriate sprite based on neighboring features
	public void resolve_feature_tiles(){
		//FIXME: should only resolve tiles in immediate vicinity
		for (int i = 0; i < feature_game_objects.count; i++) {
			GameObject go = feature_game_objects[i];
			if (go == null)
				continue;
			Feature feature = world.get_feature_by_id (i);
			int index = feature_neighbor_count (feature);
			go.GetComponent<SpriteRenderer>().sprite = ResourcePool.get_proper_feature_sprite(feature.type, index);
		}
	}

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

	// Update is called once per frame
	void Update () {

	}
}
