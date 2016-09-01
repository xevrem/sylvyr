using System;
using UnityEngine;


public static class SimpleBehaviors
{
	public static ECSInstance ecs_instance;

	public static Behavior simple_basic_enemy(){
		Behavior behavior = new Behavior (new Sequence (follow_behavior_action(), 
			new Selector (new Inverter(is_facing_target_condition()), time_to_shoot())));

		return behavior;
	}

	//make it easy to wrap
	public static BehaviorAction follow_behavior_action(){
		return new BehaviorAction (follow_behavior);
	}

	public static BehaviorReturnCode follow_behavior(Entity entity){
		//Debug.Log ("behavior called");

		GOData go = ComponentMapper.get_simple<GOData> (entity);
		Target f = ComponentMapper.get_simple<Target> (entity);
		GOData t_go = ComponentMapper.get_simple<GOData> (f.target);


		Heading h = ComponentMapper.get_simple<Heading> (entity);

		//set a default turn rate
		float turn_rate = 180f * ecs_instance.delta_time;

		//determine if the angle is positive or negative
		if ( VectorHelper.getSignedAngle (h.heading,
			t_go.game_object.transform.position - 
			go.game_object.transform.position) > 0) {
			h.heading = VectorHelper.rotateVectorDegrees (h.heading, turn_rate);
			go.game_object.transform.Rotate (Vector3.forward, turn_rate);
		} else {
			h.heading = VectorHelper.rotateVectorDegrees (h.heading, -turn_rate);
			go.game_object.transform.Rotate (Vector3.forward, -turn_rate);
		}

		h.heading.Normalize ();

		go.game_object.transform.position += h.heading * ecs_instance.delta_time * 4f;

		//update quadtree
		Quadrent quad = ComponentMapper.get_simple<Quadrent> (entity);
		if(quad.current_node != null)
			quad.current_node.Contents.Remove (entity);
		quad.current_node = WorldController.instance.quad_tree.setContentAtLocation(entity, go.game_object.transform.position);

		//Debug.Log ("got here...");

		return BehaviorReturnCode.Success;
	}

	public static BehaviorAction fire_weapon_action(){
		return new BehaviorAction (fire_weapon);
	}

	public static BehaviorReturnCode fire_weapon(Entity entity){
		GOData go = ComponentMapper.get_simple<GOData> (entity);
		Heading h = ComponentMapper.get_simple<Heading> (entity);

		UtilFactory.create_basic_projectile (entity, go.game_object.transform.position, h.heading);

		return BehaviorReturnCode.Success;
	}

	public static Conditional is_facing_target_condition(){
		return new Conditional(is_facing_target);
	}

	public static bool is_facing_target(Entity entity){
		GOData go = ComponentMapper.get_simple<GOData> (entity);
		Target f = ComponentMapper.get_simple<Target> (entity);
		GOData t_go = ComponentMapper.get_simple<GOData> (f.target);
		Heading h = ComponentMapper.get_simple<Heading> (entity);

		Vector3 to_target = t_go.game_object.transform.position - go.game_object.transform.position;

		float angle = Vector3.Angle (h.heading, to_target);

		if (angle < 10f)
			return true;

		return false;
	}

	public static Timer time_to_shoot(){ 
		return new Timer (delta_time_func, 0.25f, fire_weapon_action ());
	}

	public static float delta_time_func(){
		return ecs_instance.delta_time;
	}
		
}

