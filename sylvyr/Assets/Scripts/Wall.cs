using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public Sprite damage_sprite;
	public int hp = 4;

	public AudioClip chop_sound1;
	public AudioClip chop_sound2;

	private SpriteRenderer sprite_renderer;


	// Use this for initialization
	void Awake () {
		sprite_renderer = GetComponent<SpriteRenderer> ();
	}
	
	public void damage_wall(int loss){
		SoundManager.instance.randomize_effects (chop_sound1, chop_sound2);
		sprite_renderer.sprite = damage_sprite;
		hp -= loss;

		if (hp <= 0) {
			gameObject.SetActive (false);
		}
	}
}
