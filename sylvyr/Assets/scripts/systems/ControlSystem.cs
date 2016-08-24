using System;
using UnityEngine;

public class ControlSystem : EntityProcessingSystem
{

	private ComponentMapper go_mapper;
	private ComponentMapper heading_mapper;

	public ControlSystem ()
	{
	}

	#region implemented abstract members of EntityProcessingSystem

	protected override void initialize ()
	{
		go_mapper = new ComponentMapper (new GOData (), ecs_instance);
		heading_mapper = new ComponentMapper (new Heading (), ecs_instance);
	}

	protected override void process (Entity entity)
	{
		GOData go = go_mapper.get<GOData> (entity);
		Heading h = heading_mapper.get<Heading> (entity);

		bool update = false;
		bool reverse = false;

		float turn_rate = 180f * ecs_instance.delta_time;

		if (Input.GetKey (KeyCode.UpArrow)) {
			//just continue in current direction
			update = true;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			//toggle reverse flag
			reverse = true;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			go.game_object.transform.Rotate (Vector3.forward, turn_rate);
			h.heading = VectorHelper.rotateVectorDegrees (h.heading, turn_rate).normalized;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			go.game_object.transform.Rotate (Vector3.forward, -turn_rate);
			h.heading = VectorHelper.rotateVectorDegrees (h.heading, -turn_rate).normalized;
		}

		if (update) {
			go.game_object.transform.position += h.heading * ecs_instance.delta_time * 5f;
		} else if (reverse) {
			go.game_object.transform.position += h.heading * ecs_instance.delta_time * -5f;
		}


			
	}

	#endregion
}

