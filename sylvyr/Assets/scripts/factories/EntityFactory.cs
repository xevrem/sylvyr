using UnityEngine;
using System.Collections.Generic;

public delegate void sprite_created_handler(SpriteData sprite);

public static class EntityFactory {

	public static  ECSInstance ecs_instance;


	public static Entity create_player_ship(Vector3 position){
		Entity e = ecs_instance.create ();

		ecs_instance.add_component(e, new SpriteData ("playerShip3_red", "player"));
		ecs_instance.add_component (e, new GOData (position));
		ecs_instance.add_component (e, new Heading (Vector3.up));

		//allows this entity to be controlled by player
		ecs_instance.add_component (e, new Controllable ());

		//give the player health
		ecs_instance.add_component(e, new Health(10f, 2f, 0.5f, on_player_zero_hp));

		//add the player faction to the player
		ecs_instance.add_component(e, new Faction("player"));

		Dictionary<string, float> reputations = new Dictionary<string, float> ();
		reputations.Add ("enemy", 0f);
		reputations.Add ("player", 1f);
		ecs_instance.add_component (e, new Reputation (reputations));

		//setup quadrent
		Quadrent quad = new Quadrent();
		quad.current_node = WorldController.instance.quad_tree.setContentAtLocation (e, position);
		ecs_instance.add_component (e, quad);

		ecs_instance.tag_manager.tag_entity ("player", e);
		ecs_instance.group_manager.add_entity_to_group ("ships", e);

		ecs_instance.resolve (e);
		return e;
	}

	public static Entity create_follower(Vector3 position, Entity entity){
		Entity e = ecs_instance.create ();

		ecs_instance.add_component(e, new SpriteData ("playerShip3_blue", "characters"));
		ecs_instance.add_component (e, new GOData (position));
		ecs_instance.add_component (e, new Heading (Vector3.up));
		//ecs_instance.add_component (e, new Target (entity));
		//perform a very simple behavior: follow a targeted entity
		//ecs_instance.add_component(e, new Behavior(new BehaviorAction(SimpleBehaviors.follow_behavior)));
		//ecs_instance.add_component(e, SimpleBehaviors.simple_follow_and_shoot());
		//ecs_instance.add_component(e, SimpleBehaviors.wander_behavior());
		ecs_instance.add_component(e, SimpleBehaviors.simple_enemy_behavior());

		ecs_instance.add_component(e, new Health(10f, 1f, 1f, on_zero_hp));

		//add the enemy faction to the enemt
		ecs_instance.add_component(e, new Faction("enemy"));

		Dictionary<string, float> reputations = new Dictionary<string, float> ();
		reputations.Add ("enemy", 1f);
		reputations.Add ("player", 0f);
		ecs_instance.add_component (e, new Reputation (reputations));

		//add field of view
		ecs_instance.add_component (e, new FieldOfView (3f));

		//setup quadrent
		Quadrent quad = new Quadrent();
		quad.current_node = WorldController.instance.quad_tree.setContentAtLocation (entity, position);
		ecs_instance.add_component (e, quad);

		//ecs_instance.tag_manager.tag_entity ("follower", e);
		ecs_instance.group_manager.add_entity_to_group ("ships", e);

		ecs_instance.resolve (e);
		return e;
	}

	public static void on_player_zero_hp(Entity entity){
		Health health = ComponentMapper.get_simple<Health> (entity);
		health.current_hp = health.max_hp;
	}

	public static void on_zero_hp(Entity entity){
		ecs_instance.delete_entity(entity);
	}



}
