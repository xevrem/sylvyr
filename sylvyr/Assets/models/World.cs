using UnityEngine;
using System.Collections.Generic;

public delegate void feature_created_handler(Feature feature);

public class World {

	Tile[,] tiles;
	Bag<Feature> _features;

	Dictionary<FeatureType, Feature> feature_prototypes;

	private int _width;

	public int Width { get{ return _width;} }

	private int _height;

	public int Height { get{ return _height; } }

	public event feature_created_handler on_feature_created;

	public World(int width=100, int height=100){
		this._width = 100;
		this._height = 100;

		this.tiles = new Tile[width, height];
		this._features = new Bag<Feature> ();

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Tile tile = new Tile (this, x, y);
				tile.id = x * height + y;
				tiles [x, y] = tile;

			}
		}

		Debug.Log ("World created with " + (width * height) + " tiles");

		create_feature_prototypes ();
	}

	void create_feature_prototypes(){
		feature_prototypes = new Dictionary<FeatureType, Feature> ();

		feature_prototypes.Add (FeatureType.WALL, 
			                    Feature.create_prototype (FeatureType.WALL, 0, 1, 1)
		);
	}

	//randomizes all tiles TileType
	public void randomize_tiles(){
		for (int x = 0; x < _width; x++) {
			for (int y = 0; y < _height; y++) {
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
		if (x > _width || x < 0 || y > _height || y < 0) {
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
			Debug.LogError ("a feature already exists on this tile");
			return;
		}

		this._features.set (feature.id, feature);

		if(on_feature_created != null)
			on_feature_created (feature);
	}

	//returns the Feature given its ID
	public Feature get_feature_by_id(int id){
		return this._features [id];
	}
}
