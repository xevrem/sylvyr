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

		if (Input.GetKey (KeyCode.UpArrow))
			heading.heading += Vector3.up;
		if (Input.GetKey (KeyCode.DownArrow))
			heading.heading += Vector3.down;
		if (Input.GetKey (KeyCode.LeftArrow))
			heading.heading += Vector3.left;
		if (Input.GetKey(KeyCode.RightArrow))
			heading.heading += Vector3.right;

		heading.heading.Normalize ();

		position.position += heading.heading * 0.1f;
	}

	#endregion
}

