using UnityEngine;
using System.Collections;

public class DamageSystem : EntityProcessingSystem {



	#region implemented abstract members of EntityProcessingSystem

	protected override void process (Entity entity)
	{
		Damage dmg = ComponentMapper.get_simple<Damage> (entity);

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
		health.current_hp -= damage.damage_amount;
		Debug.Log (string.Format("dealt {0} damage",damage.damage_amount));
		ecs_instance.delete_entity (entity);
	}

}
