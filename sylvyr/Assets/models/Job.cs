using UnityEngine;
using System.Collections;
using System;

public delegate void job_complete_handler(Job job);
public delegate void job_canceled_handler(Job job);

public class Job {
	//holds info on a queued job to perform actions in the game
	public int id;
	public Tile tile { get; protected set; }
	float job_time = 1f;

	//FIXME: fix this...
	public FeatureType feature_type;

	public event job_complete_handler on_job_complete;
	public event job_canceled_handler on_job_canceled;

	public Job(Tile tile, FeatureType feature_type,  job_complete_handler complete_handler, float job_time=1f){
		this.tile = tile;
		this.id = tile.id;
		this.feature_type = feature_type;
		this.on_job_complete += complete_handler;
	}

	public void do_work(float work_time){
		job_time -= work_time;

		if(job_time <= 0){
			if(on_job_complete != null)
				on_job_complete(this);
		}
	}

	public void cancel_job(){
		if (on_job_canceled != null)
			on_job_canceled (this);
	}

	public void clear_all_events(){
		on_job_canceled = null;
		on_job_complete = null;
	}
}
