using UnityEngine;
using System.Collections;

public class JobSpriteController : MonoBehaviour {

	Bag<GameObject> job_game_objects;

	// Use this for initialization
	void Start () {
		WorldController.instance.world.job_queue.on_job_created += handle_job_created;
		job_game_objects = new Bag<GameObject> ();
	}

	void handle_job_created(Job job){
		
		//FIXME: doesnt consider multi-tiles or rotations...

		//create the GO
		GameObject job_go = new GameObject ();
		job_go.name = "job_"+ job.feature_type + "_" + job.tile.X + "_" + job.tile.Y;
		job_go.transform.position = new Vector3 (job.tile.X, job.tile.Y, 0f);
		job_go.transform.SetParent (this.transform, true);

		//add a sprite renderer
		SpriteRenderer sr = job_go.AddComponent<SpriteRenderer>();
		sr.sortingLayerName = "features";
		sr.sprite = FeatureSpriteController.get_basic_feature_sprite(job.feature_type);
		sr.color = new Color (1f, 0.5f, 0.5f, 0.25f);

		job_game_objects.set (job.id, job_go);

		job.on_job_canceled += handle_job_finished;
		job.on_job_complete += handle_job_finished;
	}

	void handle_job_finished(Job job){
		//TODO: delete sprites

		GameObject job_go = job_game_objects [job.id];
		job_game_objects [job.id] = null;

		job.clear_all_events ();

		Destroy (job_go);
	}


	// Update is called once per frame
	void Update () {
		
	}


}
