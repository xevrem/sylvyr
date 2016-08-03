using UnityEngine;
using System.Collections;

public class World {

	Tile[,] tiles;
	private int _width;

	public int Width { get{ return _width;} }

	private int _height;

	public int Height { get{ return _height; } }

	public World(int width=100, int height=100){
		this._width = 100;
		this._height = 100;

		this.tiles = new Tile[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Tile tile = new Tile (this, x, y);
				tile.id = x * height + y;
				tiles [x, y] = tile;

			}
		}

		Debug.Log ("World created with " + (width * height) + " tiles");
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
}
