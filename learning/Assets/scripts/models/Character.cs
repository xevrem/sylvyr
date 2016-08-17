using UnityEngine;
using System.Collections;

public delegate void character_created_handler(Character character);
public delegate void character_changed_handler(Character character);

public enum CHARACTER_STATE { IDLE, FINDING_PATH, GRABBING_NODE, MOVING, WORKING }


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

	CHARACTER_STATE _state = CHARACTER_STATE.IDLE;

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
		//Debug.Log (_state);
		switch (_state) {
		case CHARACTER_STATE.IDLE:
			get_job ();
			break;
		case CHARACTER_STATE.FINDING_PATH:
			get_path ();
			break;
		case CHARACTER_STATE.GRABBING_NODE:
			get_node ();
			do_movement (delta_time);//helps smooth out movement to do it once here.
			break;
		case CHARACTER_STATE.MOVING:
			do_movement (delta_time);
			break;
		case CHARACTER_STATE.WORKING:
			do_work (delta_time);
			break;
		default:
			break;
		}
	}

	void set_state(CHARACTER_STATE state){
		_state = state;
	}

	public CHARACTER_STATE get_state(){
		return this._state;
	}

	void get_job(){
		if (current_job != null) {
			set_state (CHARACTER_STATE.FINDING_PATH);
			return;
		}
		
		// try to grab a new job
		current_job = WorldController.instance.world.job_queue.Dequeue ();

		if (current_job == null)
			return;

		//register for events
		current_job.on_job_canceled += handle_job_ended;
		current_job.on_job_complete += handle_job_ended;

		set_state(CHARACTER_STATE.FINDING_PATH);
	}

	void get_path(){
		if (current_job == null) {
			set_state (CHARACTER_STATE.IDLE);
			return;
		}

		//find the closest safe point immediately adjacent to the tile
		end_tile = curr_tile.get_closest_safe_neighbor(current_job.tile, false);

		//find the closest safe point immediately adjacent to the tile
		end_tile = curr_tile.get_closest_safe_neighbor(current_job.tile, false);

		//create the new path instance
		a_star = new PathAStar (WorldController.instance.world, curr_tile, end_tile);

		//can we even get to the job?
		if (a_star.failed) {
			Debug.LogError ("cannot get to job");
			current_job.cancel_job ();
			current_job = null;
			set_state (CHARACTER_STATE.IDLE);
			return;
		}

		set_state (CHARACTER_STATE.GRABBING_NODE);
	}

	void get_node(){
		if (a_star == null) {
			set_state (CHARACTER_STATE.IDLE);
			return;
		}
	
		Tile tile = a_star.get_next_tile ();
		if (tile != null) {
			dest_tile = tile;
		}

		set_state (CHARACTER_STATE.MOVING);
	}

	void do_movement(float delta_time){
		//did we reach our job's work tile?
		if (curr_tile == end_tile) {
			set_state (CHARACTER_STATE.WORKING);
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

		if(movement_percentage >= 1f){
			//we reached destination
			curr_tile = dest_tile;
			movement_percentage = 0f;

			set_state (CHARACTER_STATE.GRABBING_NODE);
		}

		//we did something, so notify stuff
		if(on_character_changed != null)
			on_character_changed (this);
	}

	void do_work(float delta_time){
		if (current_job == null) {
			set_state (CHARACTER_STATE.IDLE);
			return;
		}

		current_job.do_work (delta_time);
	}

//	public void update_old(float delta_time){
//		//Debug.Log ("character updating...");
//
//		if (perform_job (delta_time) == false)
//			return;
//
//		if (perform_movement (delta_time) == false)
//			return;
//	}
//
//	bool perform_movement(float delta_time){
//		//total distkance
//		float dist = Vector2.Distance (curr_tile.get_position2(), dest_tile.get_position2());
//
//		//delta for this frame
//		float delta = speed * delta_time;
//
//		//percent of total distance
//		float percent = delta / dist;
//
//		//update how far we've progressed
//		movement_percentage += percent;
//
//
//		if(movement_percentage >= 1f && curr_tile != end_tile){
//			//we reached destination
//			curr_tile = dest_tile;
//			movement_percentage = 0f;
//
//			//update our path node if we still have a job and a path
//			if (a_star != null && current_job != null){
//				Tile tile = a_star.get_next_tile ();
//				if (tile != null)
//					dest_tile = tile;
//			}
//		}
//
//		//we did something, so notify stuff
//		if(on_character_changed != null)
//			on_character_changed (this);
//
//		return true;
//	}
//
//	bool perform_job(float delta_time){
//		if (current_job == null) {
//			//grab a new job
//			current_job = WorldController.instance.world.job_queue.Dequeue ();
//
//			//if we have a job lets try to go do it
//			if (current_job != null) {
//				//find the closest safe point immediately adjacent to the tile
//				end_tile = curr_tile.get_closest_safe_neighbor(current_job.tile, false);
//
//				//create the new path instance
//				a_star = new PathAStar (WorldController.instance.world, curr_tile, end_tile);
//
//				//can we even get to the job?
//				if (a_star.failed) {
//					Debug.LogError ("cannot get to job");
//					current_job.cancel_job ();
//					current_job = null;
//					return false;
//				}
//
//				// can get to it! so lets get underway
//				Tile tile = a_star.get_next_tile ();
//				if (tile != null)
//					dest_tile = tile;
//
//				//register for events
//				current_job.on_job_canceled += handle_job_ended;
//				current_job.on_job_complete += handle_job_ended;
//			}
//		}
//
//		//did we reach our job's work tile?
//		if (curr_tile == end_tile) {
//			//we did, so so that work!
//			if (current_job != null) {
//				current_job.do_work (delta_time);
//			}
//
//			return false;
//		}
//
//		return true;
//	}

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
		set_state (CHARACTER_STATE.IDLE);
	}
}
