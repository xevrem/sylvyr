using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {


	public static WorldController instance{ get; protected set; }

	public Sprite floor_sprite;

	public World world{ get; protected set; }

	GameObject[] tile_game_objects;

	// Use this for initialization
	void Start () {
		if (instance != null) {
			Debug.LogError("duplicate world controllers...");
		}
		instance = this;

		//create an empty world
		world = new World ();

		//pre-size tile_game_objects
		tile_game_objects = new GameObject[world.Height*world.Width];

		ResourcePool.load_all ();

		//create GameObject for each Tile
		create_tiles();


		world.randomize_tiles ();
	}

	void create_tiles(){
		int count = 0;
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

				tile_game_objects [tile_data.id] = tile_go;

				tile_data.on_tile_type_change += handle_tile_type_change;

			}
		}
	}

	//to be called when a Tile's TileType is changed
	void handle_tile_type_change(Tile tile_data){

		GameObject tile_go = tile_game_objects [tile_data.id];

		switch (tile_data.Type){
		case TileType.FLOOR:
			tile_go.GetComponent<SpriteRenderer> ().sprite = (Sprite) ResourcePool.Tile_Sprites [(int)TileType.FLOOR];
			break;
		case TileType.EMPTY:
			tile_go.GetComponent<SpriteRenderer> ().sprite = null;
			break;
		default:
			Debug.Log ("on_tile_type_changed - error bad TileType");
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
				GameObject go = tile_game_objects [tile_data.id];

				//nullify the GO
				tile_game_objects [tile_data.id] = null;

				//de-register the handler
				tile_data.on_tile_type_change -= handle_tile_type_change;

				//destroy the GO
				Destroy (go);
			}
		}

	}

	// Update is called once per frame
	void Update () {

	}
}
