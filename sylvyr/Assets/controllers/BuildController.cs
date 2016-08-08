using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public enum InteractionMode{ TILES,
						     FEATURE}

public class BuildController : MonoBehaviour {

	InteractionMode interaction_mode;

	TileType build_tile_type = TileType.FLOOR;
	FeatureType build_feature_type = FeatureType.WALL;

	public GameObject background;
	public float min_orth_size = 3f;
	public float max_orth_size = 15f;

	// Use this for initialization
	void Start () {
		interaction_mode = InteractionMode.TILES;
	}
		
	public void set_mode_build_floor(){
		interaction_mode = InteractionMode.TILES;
		build_tile_type = TileType.FLOOR;
	}

	public void set_mode_bulldoze_floor(){
		interaction_mode = InteractionMode.TILES;
		build_tile_type = TileType.EMPTY;
	}

	public void set_mode_build_feature(string feature){
		interaction_mode = InteractionMode.FEATURE;
		build_feature_type = (FeatureType) Enum.Parse(typeof(FeatureType),feature);
	}

	public void do_build(Tile tile){
		switch (interaction_mode) {
		case InteractionMode.TILES:
			//change the tile type
			tile.Type = build_tile_type;
			break;
		case InteractionMode.FEATURE:
			//can we even build the feature in the tile...
			if (WorldController.instance.world.is_feature_placement_valid (build_feature_type, tile) == false
			    || tile.pending_job != null)
				return;

			create_job (tile, build_feature_type);

			break;
		default:
			break;
		}
	}

	public void create_job(Tile tile, FeatureType feature_type){
		//create the feature build job
		Job job = new Job (tile, feature_type, (the_job) => {
			WorldController.instance.world.create_feature (the_job.tile, feature_type);
			the_job.tile.pending_job = null;
		});

		//FIXME: we shouldnt have to do this here, tiles should not be aware of jobs...
		tile.pending_job = job;

		//FIXME: arrrrrrrg this is dumb
		job.on_job_canceled += (the_job) => {
			the_job.tile.pending_job = null;
		};

		WorldController.instance.world.job_queue.Enqueue (job);
		//Debug.Log ("job queue size: " + WorldController.instance.world.job_queue.Count);
	}
}
