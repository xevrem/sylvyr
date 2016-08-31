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

}
