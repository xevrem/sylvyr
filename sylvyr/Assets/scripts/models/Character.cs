using UnityEngine;
using System.Collections;

public delegate void character_created_handler(Character character);
public delegate void character_changed_handler(Character character);

public class Character {

	public float x {
		get{ 
			return Mathf.Lerp (curr_tile.X, dest_tile.X, movement_percentage);
		}
	}
	public float y{
		get{ 
			return Mathf.Lerp (curr_tile.Y, dest_tile.Y, movement_percentage);
		}
	}

	public int id = -1;

	public Tile curr_tile{ get; protected set;}
	public Tile dest_tile{ get; protected set;}
	Tile end_tile;

	float movement_percentage;
	float speed = 5f;

	Job current_job;

	PathAStar a_star = null;

	public event character_changed_handler on_character_changed;

	public Character(Tile tile){
		this.curr_tile = tile;
		this.dest_tile = tile;
		this.id = WorldController.instance.world.character_ids.next ();
	}

	public void set_destination(Tile tile){
		if (curr_tile.is_neighbor (tile, true) == false) {
			Debug.Log ("destination tile is not adjacent");
		}

		dest_tile = tile;
	}

	public void update(float delta_time){
		//Debug.Log ("character updating...");

		if (current_job == null) {
			//grab a new job
			current_job = WorldController.instance.world.job_queue.Dequeue ();

			//if we have a job lets try to go do it
			if (current_job != null) {
				//find the closest safe point immediately adjacent to the tile
				end_tile = curr_tile.get_closest_safe_neighbor(current_job.tile, false);

				//create the new path instance
				a_star = new PathAStar (WorldController.instance.world, curr_tile, end_tile);

				//can we even get to the job?
				if (a_star.failed) {
					Debug.LogError ("cannot get to job");
					current_job = null;
					return;
				}

				//get can get to it! so lets get underway
				Tile tile = a_star.get_next_tile ();
				if (tile != null)
					dest_tile = tile;
				
				//register for events
				current_job.on_job_canceled += handle_job_ended;
				current_job.on_job_complete += handle_job_ended;
			}
		}

		//did we reach our job's work tile?
		if (curr_tile == end_tile) {
			//we did, so so that work!
			if (current_job != null) {
				current_job.do_work (delta_time);
			}

			return;
		}

		//total distkance
		float dist = Vector2.Distance (curr_tile.get_position2(), dest_tile.get_position2());

		//delta for this frame
		float delta = speed * delta_time;

		//percent of total distance
		float percent = delta / dist;

		//update how far we've progressed
		movement_percentage += percent;


		if(movement_percentage >= 1f && curr_tile != end_tile){
			//we reached destination
			curr_tile = dest_tile;
			movement_percentage = 0f;

			//update our path node if we still have a job and a path
			if (a_star != null && current_job != null){
				Tile tile = a_star.get_next_tile ();
				if (tile != null)
					dest_tile = tile;
			}
		}

		//we did something, so notify stuff
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

		//reset the jobs and pathing
		current_job = null;
		a_star = null;
	}
}
