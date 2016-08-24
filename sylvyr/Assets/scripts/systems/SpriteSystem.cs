using UnityEngine;
using System.Collections;

public class SpriteSystem : EntityProcessingSystem {

	private Bag<GameObject> game_objects = new Bag<GameObject>();
	private ComponentMapper sprite_mapper;
	private ComponentMapper position_mapper;
	//private ComponentMapper heading_mapper;

	#region implemented abstract members of EntityProcessingSystem

	protected override void initialize ()
	{
		sprite_mapper = new ComponentMapper(new SpriteData(), ecs_instance);
		position_mapper = new ComponentMapper (new GOData (), ecs_instance);
		//heading_mapper = new ComponentMapper (new Heading (), ecs_instance);
	}

	protected override void added (Entity entity)
	{
		//Debug.Log ("added entity: "+entity.id);
		SpriteData sprite_data = sprite_mapper.get<SpriteData> (entity);
		sprite_data.sprite_changed += on_sprite_changed;

		GOData go_data = position_mapper.get<GOData> (entity);

		go_data.game_object.name = sprite_data.id.ToString();

		SpriteRenderer sr = go_data.game_object.AddComponent<SpriteRenderer> ();
		sr.sortingLayerName = sprite_data.layer_name;
		sr.sprite = ResourcePool.get_sprite_by_name (sprite_data.asset_name);

	}

	protected override void process (Entity entity)
	{
//		if (Input.GetKeyDown (KeyCode.B)) {
//			SpriteData sprite_data = sprite_mapper.get<SpriteData> (entity);
//			sprite_data.asset_name = "playerShip3_blue";
//		} else if (Input.GetKeyDown (KeyCode.R)) {
//			SpriteData sprite_data = sprite_mapper.get<SpriteData> (entity);
//			sprite_data.asset_name = "playerShip3_red";
//		}
	}

	protected override void removed (Entity entity)
	{
		
	}

	#endregion

	public void on_sprite_changed(SpriteData sprite_data){
		//TODO: Update sprite
		GOData go_data = position_mapper.get<GOData> (sprite_data.owner_id);
		SpriteRenderer sr = go_data.game_object.GetComponent<SpriteRenderer> ();
		sr.sprite = ResourcePool.get_sprite_by_name (sprite_data.asset_name);
		sr.sortingLayerName = sprite_data.layer_name;
	}

}
