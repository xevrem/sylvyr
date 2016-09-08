using System;
using UnityEngine;

public class ControlSystem : EntityProcessingSystem
{

	private ComponentMapper go_mapper;
	private ComponentMapper heading_mapper;

	private IBehavior simepl_shoot_behavior = SimpleBehaviors.time_to_shoot(0.1f);

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
		#region MOVEMENT
		GOData go = ComponentMapper.get_simple<GOData>(entity);
		Heading h = heading_mapper.get<Heading> (entity);

		//Debug.Log("char position: " + go.game_object.transform.position);

		bool forward = false;
		bool reverse = false;

		float turn_rate = 180f * ecs_instance.delta_time;

		if (Input.GetKey (KeyCode.UpArrow)) {
			//just continue in current direction
			forward = true;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			//toggle reverse flag
			reverse = true;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			//perform a rotate, note: you may just want to turn in place, hence no forward/reverse setting
			go.game_object.transform.Rotate (Vector3.forward, turn_rate);
			h.heading = VectorHelper.roate_vector_degrees (h.heading, turn_rate).normalized;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			//perform a rotate, note: you may just want to turn in place, hence no forward/reverse setting
			go.game_object.transform.Rotate (Vector3.forward, -turn_rate);
			h.heading = VectorHelper.roate_vector_degrees (h.heading, -turn_rate).normalized;
		}

		if (forward) {
			go.game_object.transform.position += h.heading * ecs_instance.delta_time * 5f;
		} else if (reverse) {
			go.game_object.transform.position += h.heading * ecs_instance.delta_time * -5f;
		}

		//update quadrent info
		Quadrent quad = ComponentMapper.get_simple<Quadrent> (entity);
		if(quad.current_node != null)
			quad.current_node.Contents.Remove (entity);
		quad.current_node = WorldController.instance.quad_tree.setContentAtLocation (entity, go.game_object.transform.position);

		#endregion

		#region DAMAGE TEST

		if(Input.GetKeyDown(KeyCode.P)){
			EntityFactory.create_follower(Vector3.zero, entity);
		}

		if(Input.GetKey(KeyCode.Space)){
			//UtilFactory.create_basic_projectile(entity, go.game_object.transform.position, h.heading.normalized);
			simepl_shoot_behavior.Behave(entity);
		}

		#endregion

			
	}

	#endregion
}

