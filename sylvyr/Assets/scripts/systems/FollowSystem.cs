using System;
using UnityEngine;


public class FollowSystem : EntityProcessingSystem
{
	ComponentMapper go_mapper;
	ComponentMapper heading_mapper;
	ComponentMapper follower_mapper;
	Entity player;


	public FollowSystem ()
	{
	}
		
	#region implemented abstract members of EntitySystem

	protected override void initialize ()
	{
		go_mapper = new ComponentMapper (new GOData (), ecs_instance);
		heading_mapper = new ComponentMapper (new Heading (), ecs_instance);
		follower_mapper = new ComponentMapper (new Follower (), ecs_instance);

	}

	protected override void pre_load_content (Bag<Entity> entities)
	{
		player = ecs_instance.tag_manager.get_entity_by_tag ("player");
	}

	protected override void process (Entity entity)
	{
		GOData go = go_mapper.get<GOData> (entity);
		Follower f = follower_mapper.get<Follower> (entity);
		GOData p_go = go_mapper.get<GOData> (f.following);

		Heading h = heading_mapper.get<Heading> (entity);

		//set a default turn rate
		float turn_rate = 90f * ecs_instance.delta_time;

		//determine if the angle is positive or negative
		if ( VectorHelper.getSignedAngle (h.heading,
										  p_go.game_object.transform.position - 
										  go.game_object.transform.position) > 0) {
			h.heading = VectorHelper.rotateVectorDegrees (h.heading, turn_rate);
			go.game_object.transform.Rotate (Vector3.forward, turn_rate);
		} else {
			h.heading = VectorHelper.rotateVectorDegrees (h.heading, -turn_rate);
			go.game_object.transform.Rotate (Vector3.forward, -turn_rate);
		}

		h.heading.Normalize ();

		go.game_object.transform.position += h.heading * ecs_instance.delta_time * 2f;

	}
	#endregion
}

