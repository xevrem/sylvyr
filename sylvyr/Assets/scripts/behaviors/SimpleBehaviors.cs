using System;
using System.Collections.Generic;
using UnityEngine;


public static class SimpleBehaviors
{
	public static ECSInstance ecs_instance;

	public static Behavior simple_enemy_behavior(){
		//enemy should wanter until it detects something with a reputation below .5f
		//it should then start following it and shoot at it

		Selector have_target = new Selector (low_faction_nearby_conditional (), wander_composite());

		IndexSelector behave = new IndexSelector (has_target_func, have_target, 
			new Sequence(simple_follow_and_shoot_composite(), loose_interest_timer(5f)));
		
		return new Behavior (behave);
	}

	public static Behavior wander_behavior(){
		return new Behavior (wander_composite());;
	}

	public static IBehavior wander_composite(){
		Sequence move_and_change = new Sequence (move_along_heading_action(), 
			new Timer (delta_time_func, 2f, randomly_shift_heading_action()));

		Selector no_out_of_bounds = new Selector (new Inverter(out_of_bounds_conditional()), reverse_heading_action());

		return new Sequence (no_out_of_bounds, move_and_change, reorient_go_action ());
	}

	public static Timer loose_interest_timer(float time){
		return new Timer (delta_time_func, time, loose_interest_action ()); 
	}

	public static BehaviorAction loose_interest_action(){
		return new BehaviorAction (loose_interest);
	}

	public static BehaviorReturnCode loose_interest(Entity entity){
		Target t = ComponentMapper.get_simple<Target> (entity);
		if (t == null)
			return BehaviorReturnCode.Success;

		//remove this component from its owner
		ecs_instance.remove_component (t);
		return BehaviorReturnCode.Success;
	}

	public static BehaviorAction reorient_go_action(){
		return new BehaviorAction (reorient_go);
	}

	public static BehaviorReturnCode reorient_go(Entity entity){
		GOData go = ComponentMapper.get_simple<GOData> (entity);

		if (go == null)
			return BehaviorReturnCode.Failure;

		Heading h = ComponentMapper.get_simple<Heading> (entity);

		if (h == null)
			return BehaviorReturnCode.Failure;

		float angle = (VectorHelper.get_signed_angle (Vector3.up, h.heading) * 360) / (2*Mathf.PI);

		go.game_object.transform.eulerAngles = new Vector3 (0, 0, angle);

		//Debug.Log(""+go.game_object.transform.eulerAngles);

		return BehaviorReturnCode.Success;
	}

	public static Conditional out_of_bounds_conditional(){
		return new Conditional (check_out_of_bounds);
	}

	public static bool check_out_of_bounds(Entity entity){
		GOData go = ComponentMapper.get_simple<GOData> (entity);

		if (go == null)
			return false;

		//check to see if we've left the bounds of the game area
		if (WorldController.instance.bounds.Contains (go.game_object.transform.position) == false) {
			return true;
		}

		return false;
	}


	public static BehaviorAction move_along_heading_action(){
		return new BehaviorAction (move_along_heading);
	}

	public static BehaviorReturnCode move_along_heading(Entity entity){ 
		GOData go = ComponentMapper.get_simple<GOData> (entity);

		if (go == null)
			return BehaviorReturnCode.Failure;

		Heading head = ComponentMapper.get_simple<Heading>(entity);

		if (head == null)
			return BehaviorReturnCode.Failure;

		go.game_object.transform.position += head.heading * ecs_instance.delta_time * 4f;
		head.heading.Normalize ();
		//update quadtree
		Quadrent quad = ComponentMapper.get_simple<Quadrent> (entity);
		if (quad == null)
			return BehaviorReturnCode.Failure;

		if(quad.current_node != null)
			quad.current_node.Contents.Remove (entity);
		
		quad.current_node = WorldController.instance.quad_tree.setContentAtLocation(entity, go.game_object.transform.position);

		return BehaviorReturnCode.Success;
	}



	public static BehaviorAction randomly_shift_heading_action(){
		return new BehaviorAction (randomly_shift_heading);
	}

	public static BehaviorReturnCode randomly_shift_heading(Entity entity){
		//get this entities heading
		Heading heading = ComponentMapper.get_simple<Heading>(entity);

		if (heading == null)
			return BehaviorReturnCode.Failure;

		//choose random 10-degree alteration
		float angle = UnityEngine.Random.Range (-10, 10);	

		//roate the heading by that random value
		heading.heading = VectorHelper.roate_vector_degrees (heading.heading, angle);

		return BehaviorReturnCode.Success;
	}

	public static BehaviorAction reverse_heading_action(){
		return new BehaviorAction (reverse_heading);
	}

	public static BehaviorReturnCode reverse_heading(Entity entity){
		//get this entities heading
		Heading h = ComponentMapper.get_simple<Heading>(entity);

		if (h == null)
			return BehaviorReturnCode.Failure;

		//reverse heading add choose a random degree of alteration
		Vector2 newvec = VectorHelper.roate_vector_degrees(h.heading * -1f, UnityEngine.Random.Range(-45,45));

		GOData go = ComponentMapper.get_simple<GOData> (entity);
		go.game_object.transform.position -= h.heading * ecs_instance.delta_time * 4f;

		h.heading = newvec;
		//Debug.Log ("reversing?");

		return BehaviorReturnCode.Success;
	}
		

	public static int has_target_func(Entity entity){
		Target t = ComponentMapper.get_simple<Target> (entity);
		if (t == null)
			return 0;

		return 1;
	}

	public static Conditional low_faction_nearby_conditional(){
		return new Conditional (low_faction_nearby);
	}

	public static bool low_faction_nearby(Entity entity){
		GOData go = ComponentMapper.get_simple<GOData> (entity);
		FieldOfView fov = ComponentMapper.get_simple<FieldOfView> (entity);

		List<Entity> entities = 
			WorldController.instance.quad_tree.findAllWithinRange (go.game_object.transform.position, 
																   fov.radius);

		if(entities == null)
			return false;

		//get this entities faciton
		Faction faction = ComponentMapper.get_simple<Faction>(entity);
		if (faction == null)
			return false;//cant tell friends from foes


		foreach(Entity e in entities){
			if (e == null)
				continue;

			GOData e_go = ComponentMapper.get_simple<GOData> (e);
			if (e_go == null)
				continue;

			if (Vector3.Distance (e_go.game_object.transform.position, go.game_object.transform.position) > fov.radius)
				continue;
			
			Reputation rep = ComponentMapper.get_simple<Reputation> (e);

			if (rep == null)
				continue;

			if (rep.reputations [faction.faction] < 0.5f) {
				//Debug.Log ("setting faction...");

				//set it to this entities target
				ecs_instance.add_component(entity, new Target(e));

				return true;
			}
		}

		return false;
	}



	public static Behavior simple_follow_and_shoot_behavior(){
		return new Behavior(simple_follow_and_shoot_composite());
	}

	public static IBehavior simple_follow_and_shoot_composite(){
		return new Sequence (follow_behavior_action (), 
			new Selector (new Inverter (is_facing_target_condition ()), time_to_shoot (0.25f)));
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
		if ( VectorHelper.get_signed_angle (h.heading,
			t_go.game_object.transform.position - 
			go.game_object.transform.position) > 0) {
			h.heading = VectorHelper.roate_vector_degrees (h.heading, turn_rate);
			go.game_object.transform.Rotate (Vector3.forward, turn_rate);
		} else {
			h.heading = VectorHelper.roate_vector_degrees (h.heading, -turn_rate);
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

	public static Timer time_to_shoot(float timing){ 
		return new Timer (delta_time_func, timing, fire_weapon_action ());
	}

	public static float delta_time_func(){
		return ecs_instance.delta_time;
	}
		
}

