using UnityEngine;
using System.Collections;

public static class UtilFactory {

	public static ECSInstance ecs_instance;

	public static void create_damage(Entity target, float damage_amount, DamageType damage_type=DamageType.COMMON,
		DurationType duration_type=DurationType.ONE_SHOT){
	
		Entity e = ecs_instance.create ();

		ecs_instance.add_component (e, new Damage (target, damage_amount, damage_type, duration_type));

		ecs_instance.resolve (e);
	}

	public static void create_basic_projectile (Entity creator, Vector3 position, Vector2 heading){

		Entity e = ecs_instance.create ();

		ecs_instance.add_component (e, new SpriteData ("laserGreen11", "character"));
		Vector3 new_pos = position + new Vector3 (heading.x, heading.y) * 0.75f;
		ecs_instance.add_component (e, new GOData (new_pos));
		ecs_instance.add_component (e, new Heading(heading));
		ecs_instance.add_component (e, new Projectile (creator, 10f, 1f, on_hit));

		ecs_instance.resolve (e);
	}

	//basic damage on projectile hit
	public static void on_hit (Entity hit){
		create_damage (hit, 1f);
	}

}
