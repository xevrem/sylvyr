using UnityEngine;
using System.Collections.Generic;

public class IdGenerator {

	int next_id = 0;

	Queue<int> old_ids = new Queue<int>();

	public int next(){
		if (old_ids.Count > 0) {
			return old_ids.Dequeue ();
		} else {
			return next_id++;
		}
	}

	public void retire_id(int id){
		old_ids.Enqueue (id);
	}

}
