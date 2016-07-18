using UnityEngine;
using System.Collections;
using System;

public class Player : MovingObject {

    public int wall_damage = 1;
    public int points_per_food = 10;
    public int points_per_soda = 20;
    public float restart_level_delay = 1f;

    private Animator animator;
    private int food;

    public void lose_food(int loss)
    {
        animator.SetTrigger("player_hit");
        food -= loss;
        check_if_game_over();
    }

    // Use this for initialization
    protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.player_food_points;

        base.Start();
	}

    protected override void attempt_move<T>(int xdir, int ydir)
    {
        food--;

        base.attempt_move<T>(xdir, ydir);

        RaycastHit2D hit;

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
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += points_per_soda;
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
