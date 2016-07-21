using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public Sprite floor_sprite;

	World world;

	// Use this for initialization
	void Start () {

		//create an empty world
		world = new World ();


		ResourcePool.load_all ();

		//create GameObject for each Tile

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

				tile_data.on_tile_type_change += (tile) => { tile_type_change_handler(tile, tile_go);};
			}
		}

		world.randomize_tiles ();
	}
		

	//to be called when a Tile's TileType is changed
	void tile_type_change_handler(Tile tile_data, GameObject tile_go){
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

	// Update is called once per frame
	void Update () {

	}
}
