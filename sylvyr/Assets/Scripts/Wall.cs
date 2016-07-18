using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public Sprite damage_sprite;
	public int hp = 4;

	private SpriteRenderer sprite_renderer;


	// Use this for initialization
	void Awake () {
		sprite_renderer = GetComponent<SpriteRenderer> ();
	}
	
	public void damage_wall(int loss){
		sprite_renderer.sprite = damage_sprite;
		hp -= loss;

		if (hp <= 0) {
			gameObject.SetActive (false);
		}
	}
}
