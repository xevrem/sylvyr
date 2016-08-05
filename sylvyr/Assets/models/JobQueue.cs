using UnityEngine;
using System.Collections.Generic;
using System;


public delegate void job_created_handler(Job job);

public class JobQueue {

	Queue<Job> job_queue;

	public event job_created_handler on_job_created;

	public JobQueue(){
		job_queue = new Queue<Job> ();
	}

	public void Enqueue(Job job){
		job_queue.Enqueue (job);

		if(on_job_created != null)
			on_job_created (job);
	}
}
