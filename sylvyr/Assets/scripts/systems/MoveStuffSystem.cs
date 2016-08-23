using UnityEngine;
using System.Collections;

public class MoveStuffSystem : EntityProcessingSystem {

	private ComponentMapper position_mapper;

	#region implemented abstract members of EntityProcessingSystem

	protected override void added (Entity entity)
	{
		Debug.Log ("added an entity...");
	}

	protected override void initialize ()
	{
		position_mapper = new ComponentMapper (new Position (), ecs_instance);
	}

	protected override void process (Entity entity)
	{
		Debug.Log ("moving stuff");
		Position p = position_mapper.get<Position> (entity);
		p.position += Vector3.MoveTowards(p.position, new Vector3(25f,25f), 0.1f);
	}

	#endregion



}
