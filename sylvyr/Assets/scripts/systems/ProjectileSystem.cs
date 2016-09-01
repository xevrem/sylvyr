using UnityEngine;
using System.Collections.Generic;

public class ProjectileSystem : EntityProcessingSystem {

	#region implemented abstract members of EntityProcessingSystem

	protected override void added (Entity entity){
		GOData go = ComponentMapper.get_simple<GOData> (entity);
		Heading heading = ComponentMapper.get_simple<Heading>(entity);

		//set the initial rotation correctly
		float angle = (VectorHelper.getSignedAngle (heading.heading, Vector3.up) / (2f*Mathf.PI)) * 360f;
		go.game_object.transform.Rotate (Vector3.forward, -angle);
	}

	protected override void process (Entity entity){
		Projectile projectile = ComponentMapper.get_simple<Projectile> (entity);
		GOData proj_go = ComponentMapper.get_simple<GOData> (entity);

		List<Entity> ships = WorldController.instance.quad_tree.findAllWithinRange (proj_go.game_object.transform.position, 1f);

		if (ships != null)
		{
			//Debug.Log ("num ships in quad: " + ships.Count);
			foreach (Entity ship in ships) {
				//Debug.Log ("got here...");
				//ignore if for some reason its bad
				if (ship == null) {
				//	Debug.Log ("is null");
					continue;
				}
				
				if (ship.id == projectile.creator.id)
					continue;

				//ok, if you're within collision distance, hit it...
				GOData ship_go = ComponentMapper.get_simple<GOData> (ship);
				if (ship_go == null)
					continue;
				
				if (Vector3.Distance (ship_go.game_object.transform.position, proj_go.game_object.transform.position) < 0.5f) {
					//we hit it... so i guess we should do something...
					//TODO: do something...
					projectile.hit(ship);

					//delete yourself as you did your something
					ecs_instance.delete_entity(entity);
					return;
				}
			}
		}

		projectile.elapsed_time += ecs_instance.delta_time;

		if (projectile.elapsed_time >= projectile.time_to_live) {
			//destroy projectile
			ecs_instance.delete_entity(entity);
			return;
		}

		//we didnt hit anything and we're still alive, so move forward
		Heading heading = ComponentMapper.get_simple<Heading>(entity);

		proj_go.game_object.transform.position += heading.heading * ecs_instance.delta_time * projectile.speed;
	}
	#endregion
	
}
