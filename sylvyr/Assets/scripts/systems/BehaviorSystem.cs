using System;

public class BehaviorSystem : EntityProcessingSystem
{
	ComponentMapper behavior_mapper;

	public BehaviorSystem ()
	{
	}
		
	#region implemented abstract members of EntityProcessingSystem

	protected override void initialize ()
	{
		behavior_mapper = new ComponentMapper (new Behavior (), ecs_instance);
	}

	protected override void process (Entity entity)
	{
		Behavior behavior = behavior_mapper.get<Behavior> (entity);

		behavior.behavior.Behave (entity);
	}
	#endregion
}


