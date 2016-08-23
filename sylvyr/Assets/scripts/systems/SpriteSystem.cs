using UnityEngine;
using System.Collections;

public class SpriteSystem : EntityProcessingSystem {

	private Bag<GameObject> game_objects;
	private ComponentMapper sprite_mapper;
	private ComponentMapper position_mapper;


	#region implemented abstract members of EntityProcessingSystem

	protected override void initialize ()
	{
		sprite_mapper = new ComponentMapper(new SpriteData(), ecs_instance);
		position_mapper = new ComponentMapper (new Position (), ecs_instance);
	}

	protected override void added (Entity entity)
	{
		Debug.Log ("added entity: "+entity.id);
		SpriteData sprite_data = sprite_mapper.get<SpriteData> (entity);
		GameObject sprite_go = new GameObject ();
		sprite_go.name = sprite_data.id.ToString();

		Position sprite_pos = position_mapper.get<Position> (entity);
		sprite_pos.position_changed += on_position_changed;
		sprite_go.transform.position = sprite_pos.position;

		SpriteRenderer sr = sprite_go.AddComponent<SpriteRenderer> ();
		sr.sortingLayerName = sprite_data.layer_name;
		sr.sprite = ResourcePool.get_sprite_by_name (sprite_data.asset_name);

		game_objects.set (sprite_data.owner_id, sprite_go);
	}

	protected override void process (Entity entity)
	{
		//do nothing...
	}

	protected override void removed (Entity entity)
	{
		
	}

	#endregion

	public void on_sprite_changed(SpriteData sprite){
	}

	public void on_position_changed(Position position){
		GameObject go = game_objects [position.owner_id];
		go.transform.position = position.position;
	}

}
