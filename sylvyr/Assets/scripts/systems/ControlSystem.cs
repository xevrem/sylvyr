using System;
using UnityEngine;

public class ControlSystem : EntityProcessingSystem
{

	private ComponentMapper position_mapper;
	private ComponentMapper heading_mapper;

	public ControlSystem ()
	{
	}

	#region implemented abstract members of EntityProcessingSystem

	protected override void initialize ()
	{
		position_mapper = new ComponentMapper (new Position (), ecs_instance);
		heading_mapper = new ComponentMapper (new Heading (), ecs_instance);
	}

	protected override void process (Entity entity)
	{
		Position position = position_mapper.get<Position> (entity);
		Heading heading = heading_mapper.get<Heading> (entity);

		bool changed = false;

		if (Input.GetKey (KeyCode.UpArrow))
			heading.heading += Vector2.up;
		if (Input.GetKey (KeyCode.DownArrow))
			heading.heading += Vector2.down;
		if (Input.GetKey (KeyCode.LeftArrow))
			heading.heading += VectorHelper.rotateVectorRadians (heading.heading, 0.1f);
		if (Input.GetKey(KeyCode.RightArrow))
			heading.heading += VectorHelper.rotateVectorRadians (heading.heading, -0.1f);

		heading.heading.Normalize (); 

		position.position += heading.heading * 0.1f;
	}

	#endregion
}

