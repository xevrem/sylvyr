using UnityEngine;
using System.Collections;

public delegate void character_created_handler(Character character);
public delegate void character_changed_handler(Character character);

public class Character {

	public float x {
		get{ 
			return Mathf.Lerp (currTile.X, destTile.X, movement_percentage);
		}
	}
	public float y{
		get{ 
			return Mathf.Lerp (currTile.Y, destTile.Y, movement_percentage);
		}
	}

	public int id = -1;

	public Tile currTile{ get; protected set;}
	public Tile destTile{ get; protected set;}

	float movement_percentage;
	float speed = 2f;

	Job current_job;

	public event character_changed_handler on_character_changed;

	public Character(Tile tile){
		this.currTile = tile;
		this.destTile = tile;
		this.id = WorldController.instance.world.character_ids.next ();
	}

	public void set_destination(Tile tile){
		if (currTile.is_neighbor (tile) == false) {
			Debug.Log ("destination tile is not adjacent");
		}

		destTile = tile;
	}

	public void update(float delta_time){
		//Debug.Log ("character updating...");

		if (current_job == null) {
			//grab a new job
			current_job = WorldController.instance.world.job_queue.Dequeue ();

			if (current_job != null) {
				destTile = current_job.tile;
				//register for events
				current_job.on_job_canceled += handle_job_ended;
				current_job.on_job_complete += handle_job_ended;
			}
		}


		if (currTile == destTile) {
			if (current_job != null) {
				current_job.do_work (delta_time);
			}

			return;
		}

		//total distkance
		float dist = Vector2.Distance (currTile.get_position2(), destTile.get_position2());

		//delta for this frame
		float delta = speed * delta_time;

		//percent of total distance
		float percent = delta / dist;

		//update how far we've progressed
		movement_percentage += percent;

		if(movement_percentage >= 1f){
			//we reached destination
			currTile = destTile;
			movement_percentage = 0f;
		}

		if(on_character_changed != null)
			on_character_changed (this);
	}

	void handle_job_ended(Job job){
		//job completed or cancelled
		if (job != current_job) {
			Debug.LogError ("yikes... phantom job!");
			return;
		}

		current_job.on_job_canceled -= handle_job_ended;
		current_job.on_job_complete -= handle_job_ended;

		current_job = null;

	}
}
