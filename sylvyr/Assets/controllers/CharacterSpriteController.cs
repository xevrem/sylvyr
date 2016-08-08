using UnityEngine;
using System.Collections;

public class CharacterSpriteController : MonoBehaviour {

	Bag<GameObject> character_game_objects;

	World world {
		get{return WorldController.instance.world;}
	}

	public Sprite character_sprite;

	// Use this for initialization
	void Start () {
		character_game_objects = new Bag<GameObject> ();

		world.on_character_created += handle_character_created;


		//FIXME: this is just for debugging... remove it.
		Character c = world.create_character (world.get_tile_at (world.width / 2, world.height / 2));
		c.set_destination(world.get_tile_at (c.currTile.X + 5, c.currTile.Y));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void handle_character_created(Character character){
		GameObject char_go = new GameObject ();
		char_go.name = "character_" + character.id;
		char_go.transform.position = new Vector3 (character.x, character.y, 0f);
		char_go.transform.SetParent (this.transform, true);

		//add a sprite renderer
		SpriteRenderer sr = char_go.AddComponent<SpriteRenderer>();
		sr.sortingLayerName = "characters";
		sr.sprite = character_sprite;

		character_game_objects.set (character.id, char_go);

		character.on_character_changed += handle_character_changed;
	}

	void handle_character_changed(Character character){

		GameObject char_go = character_game_objects [character.id];

		if (char_go == null)
			return;//dont try to do anything
		
		char_go.transform.position = new Vector3 (character.x, character.y, 0f);

	}
}
