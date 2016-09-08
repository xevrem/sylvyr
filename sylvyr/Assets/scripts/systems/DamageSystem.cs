using UnityEngine;
using System.Collections;

public class DamageSystem : EntityProcessingSystem {



	#region implemented abstract members of EntityProcessingSystem

	protected override void process (Entity entity)
	{
		Damage dmg = ComponentMapper.get_simple<Damage> (entity);
		if (dmg == null)
			return;
		switch (dmg.duration_type) {
		case DurationType.ONE_SHOT:
			do_one_shot (entity, dmg);
			break;
		default:
			break;
		}

	}

	#endregion

	void do_one_shot(Entity entity, Damage damage){
		Health health = ComponentMapper.get_simple<Health> (damage.target);

		//check if it can even be damaged
		if (health == null)
			return;

		health.current_hp -= damage.damage_amount;

		//delete the damage entity
		ecs_instance.delete_entity (entity);
	}

}
