using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Player : MovingObject {

    public int wall_damage = 1;
    public int points_per_food = 10;
    public int points_per_soda = 20;
    public float restart_level_delay = 1f;
	public Text food_text;

	public AudioClip move_sound1;
	public AudioClip move_sound2;
	public AudioClip eat_sound1;
	public AudioClip eat_sound2;
	public AudioClip drink_sound1;
	public AudioClip drink_sound2;
	public AudioClip game_over_sound;

    private Animator animator;
    private int food;

    public void lose_food(int loss)
    {
        animator.SetTrigger("player_hit");
        food -= loss;
		food_text.text = "-" + loss + " Food: " + food;
        check_if_game_over();
    }

    // Use this for initialization
    protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.player_food_points;
		food_text.text = "Food: " + food;
        base.Start();
	}

    protected override void attempt_move<T>(int xdir, int ydir)
    {
        food--;
		food_text.text = "Food: " + food;

        base.attempt_move<T>(xdir, ydir);

        RaycastHit2D hit;

		if (move (xdir, ydir, out hit)) {
			SoundManager.instance.randomize_effects (move_sound1, move_sound2);
		}

        check_if_game_over();

        GameManager.instance.players_turn = false;
    }

    protected override void on_cant_move<T>(T component)
    {
        Wall hit_wall = component as Wall;
        hit_wall.damage_wall(wall_damage);
        animator.SetTrigger("player_chop");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("restart", restart_level_delay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += points_per_food;
			food_text.text = "+" + points_per_food + " Food: " + food;
			SoundManager.instance.randomize_effects (eat_sound1, eat_sound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += points_per_soda;
			food_text.text = "+"+ points_per_soda + " Food: " + food;
			SoundManager.instance.randomize_effects (drink_sound1, drink_sound2);
            other.gameObject.SetActive(false);
        }
    }

    private void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void OnDisable()
    {
        GameManager.instance.player_food_points = food;
    }

    private void check_if_game_over()
    {
        if (food <= 0)
        {
			SoundManager.instance.randomize_effects (game_over_sound);
			SoundManager.instance.music_source.Stop ();
            GameManager.instance.game_over();
        }
    }

	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.players_turn)
            return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            attempt_move<Wall>(horizontal, vertical);
	}
}
