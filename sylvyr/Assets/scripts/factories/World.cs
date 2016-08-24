using UnityEngine;
using System.Collections;


public delegate void sprite_created_handler(SpriteData sprite);

public class World {

	private ECSInstance ecs_instance;

	public World(ECSInstance instance){
		ecs_instance = instance;
	}


	public void create_ship(){
		Entity e = ecs_instance.create ();

		ecs_instance.add_component(e, new SpriteData ("playerShip3_red", "character"));
		ecs_instance.add_component (e, new Position (new Vector3 (0f,0f)));
		ecs_instance.add_component (e, new Heading ());
		ecs_instance.add_component (e, new Controllable ());

		ecs_instance.tag_manager.tag_entity ("player", e);
		ecs_instance.group_manager.add_entity_to_group ("ships", e);

		ecs_instance.resolve (e);
	}



}
