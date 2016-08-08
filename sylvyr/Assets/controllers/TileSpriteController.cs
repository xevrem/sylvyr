using UnityEngine;
using System.Collections.Generic;

public class TileSpriteController : MonoBehaviour {

	public Sprite floor_sprite;
	public Sprite empty_sprite;

	Bag<GameObject> tile_game_objects;

	World world {
		get{return WorldController.instance.world;}
	}

	// Use this for initialization
	void Start () {
		//pre-size tile_game_objects
		tile_game_objects = new Bag<GameObject>();

		create_tile_game_objects ();

		//when a new tile is created, handle it
		world.on_tile_changed += handle_tile_changed;
	}

	void create_tile_game_objects(){
		for (int x = 0; x < world.width; x++) {
			for (int y = 0; y < world.height; y++) {
				//get the tile at this location
				Tile tile_data = world.get_tile_at (x, y);

				//create the GO
				GameObject tile_go = new GameObject ();
				tile_go.name = "tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (tile_data.X, tile_data.Y, 0f);
				tile_go.transform.SetParent (this.transform, true);

				//add a sprite renderer
				SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
				sr.sprite = empty_sprite;
				sr.sortingLayerName = "tiles";

				tile_game_objects.set(tile_data.id, tile_go);
			}
		}
	}


	//to be called when a Tile's TileType is changed
	void handle_tile_changed(Tile tile_data){

		GameObject tile_go = tile_game_objects [tile_data.id];

		switch (tile_data.Type){
		case TileType.FLOOR:
			tile_go.GetComponent<SpriteRenderer> ().sprite = ResourcePool.tile_sprites [(int)TileType.FLOOR];
			break;
		case TileType.EMPTY:
			tile_go.GetComponent<SpriteRenderer> ().sprite = empty_sprite;
			break;
		default:
			Debug.Log ("handle_tile_changed - error bad TileType");
			break;
		}
	}


	public void destroy_all_tile_game_objects(){
		for (int x = 0; x < world.width; x++) {
			for (int y = 0; y < world.height; y++) {

				//get the tile and its associated GO at this location
				Tile tile_data = world.get_tile_at (x, y);
				GameObject go = tile_game_objects.remove (tile_data.id);

				//de-register the handler
				tile_data.on_tile_changed -= handle_tile_changed;

				//destroy the GO
				Destroy (go);
			}
		}
	}
}
