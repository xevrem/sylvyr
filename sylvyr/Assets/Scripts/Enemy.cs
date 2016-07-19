using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	public int player_damage;

	public AudioClip enemy_attack1;
	public AudioClip enemy_attack2;

	private Animator animator;
	private Transform target;
	private bool skip_move;


	// Use this for initialization
	protected override void Start () {
		GameManager.instance.add_enemy_to_list (this);
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}
		


	protected override void attempt_move<T> (int xdir, int ydir)
	{
		if (skip_move) {
			skip_move = false;
			return;
		}
		base.attempt_move<T> (xdir, ydir);

		skip_move = true;
	}

	public void move_enemy(){
		int xdir = 0;
		int ydir = 0;

		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
			ydir = target.position.y > transform.position.y ? 1 : -1;
		} else {
			xdir = target.position.x > transform.position.x ? 1 : -1;
		}

		attempt_move<Player> (xdir, ydir);
	}

	protected override void on_cant_move<T> (T component)
	{
		Player hit_player = component as Player;
		hit_player.lose_food (player_damage);
		animator.SetTrigger ("enemy_attack");

		SoundManager.instance.randomize_effects (enemy_attack1, enemy_attack2);
	}
}
