using UnityEngine;
using System.Collections.Generic;

public delegate void feature_created_handler(Feature feature);

public class World {

	Tile[,] tiles;
	Bag<Feature> _features;
	Bag<Character> _characters;

	Dictionary<FeatureType, Feature> feature_prototypes;

	PathTileGraph _tile_graph;

	public int width{ get; protected set; }
	public int height{ get; protected set; }

	public JobQueue job_queue;

	public IdGenerator tile_ids = new IdGenerator();
	public IdGenerator feature_ids = new IdGenerator();
	public IdGenerator job_ids = new IdGenerator();
	public IdGenerator character_ids = new IdGenerator();

	public event feature_created_handler on_feature_created;
	public event tile_changed_handler on_tile_changed;
	public event character_created_handler on_character_created;


	public World(int width=100, int height=100){
		this.width = 100;
		this.height = 100;

		create_feature_prototypes ();
		//FIXME: may need to move/replace this to job controller later
		this.job_queue = new JobQueue ();

		this.tiles = new Tile[width, height];
		this._features = new Bag<Feature> ();

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Tile tile = new Tile (this, x, y);
				tile.id = tile_ids.next ();

				tile.on_tile_changed += handle_tile_type_change;

				tiles [x, y] = tile;
			}
		}

		_characters = new Bag<Character> ();


		Debug.Log ("World created with " + (width * height) + " tiles");
	}

	public void update(float delta_time){

		for (int i = 0; i < _characters.count; i++) {
			_characters[i].update (delta_time);
		}

	}

	void create_feature_prototypes(){
		feature_prototypes = new Dictionary<FeatureType, Feature> ();

		feature_prototypes.Add (FeatureType.WALL, 
			                    Feature.create_prototype (FeatureType.WALL, 0, 1, 1)
		);
	}

	//randomizes all tiles TileType
	public void randomize_tiles(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (Random.Range (0, 2) == 0) {
					tiles [x, y].Type = TileType.EMPTY;
				} else {
					tiles [x, y].Type = TileType.FLOOR; 
				}
			}
		}
	}

	//retrieves the tile at the specified location
	public Tile get_tile_at(int x, int y){
		if (x > width || x < 0 || y > height || y < 0) {
			Debug.Log ("Tile (" +x+ "," +y+ ") is out of rance");
			return null;
		}

		return this.tiles[x,y];
	}

	//creates a feature of FeatureType on the specified Tile
	public void create_feature(Tile tile, FeatureType feature_type){
		//Debug.Log("create_feature run...");

		if (feature_prototypes.ContainsKey (feature_type) == false) {
			Debug.LogError ("feature_prototypes does not contain prototype:" + feature_type.ToString ());
			return;
		}

		Feature proto = feature_prototypes [feature_type];
		Feature feature = Feature.place_feature (proto, tile);

		//check if feature cannot be placed
		if (feature == null) {
			Debug.LogError ("feature cannot be placed here");
			return;
		}

		this._features.set (feature.id, feature);

		if(on_feature_created != null)
			on_feature_created (feature);

		invalidate_tile_graph ();
	}

	//returns the Feature given its ID
	public Feature get_feature_by_id(int id){
		return this._features [id];
	}

	//called when any tile is changed
	void handle_tile_type_change(Tile tile){
		if (on_tile_changed == null)
			return;
		
		on_tile_changed (tile);

		invalidate_tile_graph ();
	}

	public bool is_feature_placement_valid(FeatureType feature_type, Tile tile){
		if (feature_prototypes.ContainsKey (feature_type))
			return feature_prototypes [feature_type].is_placement_valid (tile);
		else
			return false;
	}

	public Character create_character(Tile tile){
		Character character = new Character (tile);

		_characters.set (character.id, character);

		if(on_character_created != null)
			on_character_created (character);

		return character;
	}

	public void build_pathfinding_test(){

		int l = width / 2 - 5;
		int b = height / 2 - 5;

		for (int x = l - 5; x < l + 15; x++) {
			for (int y = b - 5; y < b + 15; y++) {
				tiles [x, y].Type = TileType.FLOOR;

				if (x == l || x == (l + 9) || y == b || y == (b + 9)) {
					if (x != (l + 9) & y != (b + 4))
						create_feature (tiles [x, y], FeatureType.WALL);
				}
			}
		}
			
	}

	public void invalidate_tile_graph(){
		this._tile_graph = null;
	}

	public PathTileGraph get_tile_graph(){
		if(this._tile_graph == null)
			this._tile_graph = new PathTileGraph(this);

		return this._tile_graph;
	}
}
