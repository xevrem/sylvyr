using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {


	public static WorldController instance{ get; protected set; }

	public World world{ get; protected set; }

	// Use this for initialization
	void OnEnable () {
		if (instance != null) {
			Debug.LogError("duplicate world controllers...");
		}
		instance = this;

		//create an empty world
		world = new World ();

		//load all resources
		ResourcePool.load_all ();

		//center camera
		center_camera();
	}

	void center_camera(){
		Camera.main.transform.position = new Vector3 (world.Width / 2, world.Height / 2, Camera.main.transform.position.z);
	}

	public Tile get_tile_at_world_coordinate(Vector3 coord){
		int x = Mathf.FloorToInt (coord.x);
		int y = Mathf.FloorToInt (coord.y);

		return WorldController.instance.world.get_tile_at (x, y); 
	}

}
