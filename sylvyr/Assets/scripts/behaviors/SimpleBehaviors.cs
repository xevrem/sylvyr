using System;
using UnityEngine;


public static class SimpleBehaviors
{
	public static ECSInstance ecs_instance;

	public static BehaviorReturnCode follow_behavior(Entity entity){
		Debug.Log ("behavior called");

		GOData go = ComponentMapper.get_simple<GOData> (entity);
		Follower f = ComponentMapper.get_simple<Follower> (entity);
		GOData p_go = ComponentMapper.get_simple<GOData> (f.following);

		Heading h = ComponentMapper.get_simple<Heading> (entity);

		//set a default turn rate
		float turn_rate = 180f * ecs_instance.delta_time;

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

		go.game_object.transform.position += h.heading * ecs_instance.delta_time * 4f;

		Debug.Log ("got here...");

		return BehaviorReturnCode.Success;
	}
}

