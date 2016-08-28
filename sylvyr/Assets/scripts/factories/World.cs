using UnityEngine;
using System.Collections;


public delegate void sprite_created_handler(SpriteData sprite);

public class World {

	private ECSInstance ecs_instance;

	public World(ECSInstance instance){
		ecs_instance = instance;
	}


	public Entity create_ship(){
		Entity e = ecs_instance.create ();

		ecs_instance.add_component(e, new SpriteData ("playerShip3_red", "character"));
		ecs_instance.add_component (e, new GOData (Vector3.zero));
		ecs_instance.add_component (e, new Heading (Vector3.up));
		ecs_instance.add_component (e, new Controllable ());

		ecs_instance.tag_manager.tag_entity ("player", e);
		ecs_instance.group_manager.add_entity_to_group ("ships", e);

		ecs_instance.resolve (e);
		return e;
	}

	public Entity create_follower(Vector3 position, Entity entity){
		Entity e = ecs_instance.create ();

		ecs_instance.add_component(e, new SpriteData ("playerShip3_blue", "character"));
		ecs_instance.add_component (e, new GOData (position));
		ecs_instance.add_component (e, new Heading (Vector3.up));
		//ecs_instance.add_component (e, new Follower (entity));
		ecs_instance.add_component(e, new Behavior(new BehaviorAction(SimpleBehaviors.follow_behavior)));

		//ecs_instance.tag_manager.tag_entity ("follower", e);
		ecs_instance.group_manager.add_entity_to_group ("ships", e);

		ecs_instance.resolve (e);
		return e;
	}



}
