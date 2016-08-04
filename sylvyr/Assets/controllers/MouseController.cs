using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public enum BuildMode{ TILES,
						FEATURE}

public class MouseController : MonoBehaviour {

	Vector3 last_frame_position;
	Vector3 curr_frame_position;
	Vector3 drag_start_position;

	BuildMode build_mode;
	List<GameObject> drag_cursors;
	TileType build_tile_type = TileType.FLOOR;
	FeatureType build_feature_type = FeatureType.WALL;

	public GameObject cursor_prefab;
	public GameObject background;
	public float min_orth_size = 3f;
	public float max_orth_size = 15f;

	// Use this for initialization
	void Start () {
		build_mode = BuildMode.TILES;
		drag_cursors = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		curr_frame_position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		curr_frame_position.z = 0;

		//update_cursor ();
		update_dragging ();
		update_camera_movement ();

		last_frame_position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		last_frame_position.z = 0;
	}



//	void update_cursor(){
//		//update cursor position to the tile under the cursor
//		Tile tile = WorldController.instance.get_tile_at_world_coordinate(curr_frame_position);
//
//		if(tile != null){
//			Vector3 cursor_pos = new Vector3 (tile.X, tile.Y, 0);
//			cursor_go.transform.position = cursor_pos;
//			cursor_go.SetActive (true);
//		}
//		else{
//			cursor_go.SetActive (false);
//		}
//	}

	void update_dragging(){
		//ignore if we've drug over a UI element
		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}


		//start drag
		if (Input.GetMouseButtonDown (0)) {
			drag_start_position = curr_frame_position;
		}

		//determine correct x selection
		int start_x = Mathf.FloorToInt (drag_start_position.x);
		int end_x = Mathf.FloorToInt (curr_frame_position.x);
		if (end_x < start_x) {
			int tmp = end_x;
			end_x = start_x;
			start_x = tmp;
		}
		//determine correct y selection
		int start_y = Mathf.FloorToInt (drag_start_position.y);
		int end_y = Mathf.FloorToInt (curr_frame_position.y);
		if (end_y < start_y) {
			int tmp = end_y;
			end_y = start_y;
			start_y = tmp;
		}

		//clean old drag previews
		while (drag_cursors.Count > 0) {
			GameObject go = drag_cursors [0];
			drag_cursors.RemoveAt (0);
			ResourcePool.Despawn (go);
		}
			

		if (Input.GetMouseButton (0)) {
			//display drag area preview
			for (int x = start_x; x <= end_x; x++) {
				for (int y = start_y; y <= end_y; y++) {
					Tile t = WorldController.instance.world.get_tile_at (x, y);
					if (t != null) {
						//display drag area hint
						GameObject go = ResourcePool.Spawn(cursor_prefab, new Vector3(x,y,0),Quaternion.identity);
						go.transform.SetParent (this.transform, true);
						drag_cursors.Add (go);
					}
				}
			}
		}

		//end drag
		if (Input.GetMouseButtonUp (0)) {
			
			//change tiles in dragged area
			for (int x = start_x; x <= end_x; x++) {
				for (int y = start_y; y <= end_y; y++) {
					Tile t = WorldController.instance.world.get_tile_at (x, y);
					if (t != null) {
						switch (build_mode) {
						case BuildMode.TILES:
							//change the tile type
							t.Type = build_tile_type;
							break;
						case BuildMode.FEATURE:
							//create the build mode object
							WorldController.instance.world.create_feature (t, build_feature_type);
							break;
						default:
							break;
						}
					}
				}
			}
			//FIXME: should only resolve tiles in immediate vicinity and after each is placed
			if(build_mode == BuildMode.FEATURE)
				WorldController.instance.resolve_feature_tiles ();
		}
	}

	void update_camera_movement(){
		//handle screend ragging
		if (Input.GetMouseButton (1)) {
			Vector3 diff = last_frame_position - curr_frame_position;
			Camera.main.transform.Translate (diff);
		}

		//zoom or unzoom the camera
		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis ("Mouse ScrollWheel");
		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, min_orth_size, max_orth_size);

		//adjust the background so it stays withing the camera at the proper scale
		Vector3 cam_pos = Camera.main.transform.position;
		background.transform.position = new Vector3 (cam_pos.x, cam_pos.y, 0f);
		float scale = Camera.main.orthographicSize / max_orth_size;
		background.transform.localScale = new Vector3 (3*scale, 3*scale, 1f);

	}

	public void set_mode_build_floor(){
		build_mode = BuildMode.TILES;
		build_tile_type = TileType.FLOOR;
	}

	public void set_mode_bulldoze_floor(){
		build_mode = BuildMode.TILES;
		build_tile_type = TileType.EMPTY;
	}

	public void set_mode_build_feature(string feature){
		build_mode = BuildMode.FEATURE;
		build_feature_type = (FeatureType) Enum.Parse(typeof(FeatureType),feature);
	}
}
