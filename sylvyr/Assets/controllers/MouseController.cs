using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	Vector3 last_frame_position;
	Vector3 curr_frame_position;
	Vector3 drag_start_position;


	public GameObject cursor_prefab;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		curr_frame_position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		curr_frame_position.z = 0;

		update_cursor ();
		update_dragging ();
		update_camera_movement ();

		last_frame_position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		last_frame_position.z = 0;
	}



	void update_cursor(){
		//update cursor position to the tile under the cursor
		Tile tile = WorldController.instance.get_tile_at_world_coordinate(curr_frame_position);

		if(tile != null){
			Vector3 cursor_pos = new Vector3 (tile.X, tile.Y, 0);
			cursor_prefab.transform.position = cursor_pos;
			cursor_prefab.SetActive (true);
		}
		else{
			cursor_prefab.SetActive (false);
		}
	}

	void update_dragging(){
		//start drag
		if (Input.GetMouseButtonDown (0)) {
			drag_start_position = curr_frame_position;
		}

		//end drag
		if (Input.GetMouseButtonUp (0)) {
			int start_x = Mathf.FloorToInt (drag_start_position.x);
			int end_x = Mathf.FloorToInt (curr_frame_position.x);
			if (end_x < start_x) {
				int tmp = end_x;
				end_x = start_x;
				start_x = tmp;
			}

			int start_y = Mathf.FloorToInt (drag_start_position.y);
			int end_y = Mathf.FloorToInt (curr_frame_position.y);
			if (end_y < start_y) {
				int tmp = end_y;
				end_y = start_y;
				start_y = tmp;
			}

			for (int x = start_x; x <= end_x; x++) {
				for (int y = start_y; y <= end_y; y++) {
					Tile t = WorldController.instance.world.get_tile_at (x, y);
					if (t != null) {
						switch (t.Type) {
						case TileType.EMPTY:
							t.Type = TileType.FLOOR;
							break;
						case TileType.FLOOR:
							t.Type = TileType.EMPTY;
							break;
						default:
							break;
						}
					}
				}
			}

		}
	}

	void update_camera_movement(){
		//handle screend ragging
		if (Input.GetMouseButton (1)) {
			Vector3 diff = last_frame_position - curr_frame_position;
			Camera.main.transform.Translate (diff);
		}
	}
}
