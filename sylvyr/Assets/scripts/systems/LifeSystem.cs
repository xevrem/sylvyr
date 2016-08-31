using UnityEngine;
using System.Collections;

public class LifeSystem : EntityProcessingSystem {


	#region implemented abstract members of EntityProcessingSystem

	protected override void process (Entity entity)
	{
		Health health = ComponentMapper.get_simple<Health> (entity);

		if (health.current_hp <= 0) {
			Debug.Log (string.Format("ENTITY {0} at 0 health!!!", entity.id));
			//TODO: what do we do when we hit 0 HP? and how do we handle other entity refs?
		}

		//perform regeneration
		health.elapsed_regen_time += ecs_instance.delta_time;

		if (health.elapsed_regen_time >= health.regen_rate) {
			health.elapsed_regen_time = 0f;
			health.current_hp += health.regen_amount;

			if (health.current_hp > health.max_hp)
				health.current_hp = health.max_hp;
			//Debug.Log ("Regened HP ticked...");
		}
	}

	#endregion



}
