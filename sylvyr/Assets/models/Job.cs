using UnityEngine;
using System.Collections;
using System;

public delegate void job_complete_handler(Job job);
public delegate void job_canceled_handler(Job job);

public class Job {
	//holds info on a queued job to perform actions in the game

	public Tile tile { get; protected set; }
	float job_time = 1f;
	public event job_complete_handler on_job_complete;
	public event job_canceled_handler on_job_canceled;

	public Job(Tile tile, job_complete_handler complete_handler, float job_time=1f){
		this.tile = tile;
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
}
