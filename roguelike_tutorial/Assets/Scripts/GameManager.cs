using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float level_start_delay = 2f;
	public float level_restart_delay = 3f;
	public static GameManager instance = null;
	public BoardManager board_script;
	public int player_food_points = 100;
	[HideInInspector] public bool players_turn = true;

	public float turn_delay = 0.1f;


	private int _level = 1;
	private List<Enemy> _enemies;
	private bool _enemies_moving;
	private Text _level_text;
	private GameObject _level_image;
	private bool _doing_setup;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		_enemies = new List<Enemy> ();
		board_script = GetComponent<BoardManager> ();
		init_game();
	}

	private void OnLevelWasLoaded(int index){
		_level++;

		init_game ();
	}


	void init_game(){
		_doing_setup = true;

		_level_image = GameObject.Find ("LevelImage");
		_level_text = GameObject.Find ("LevelText").GetComponent<Text>();
		_level_text.text = "Day " + _level;
		_level_image.SetActive (true);
		Invoke ("hide_level_image", level_start_delay);

		_enemies.Clear ();
		board_script.setup_scene (_level);
	}

	private void hide_level_image(){
		_level_image.SetActive (false);
		_doing_setup = false;
	}

    public void game_over()
    {
		_level_text.text = "After " + _level + " days, you starved";
		_level_image.SetActive (enabled);
        //enabled = false;
		_level = 1;
		player_food_points = 100;
		_enemies = new List<Enemy> ();
		board_script = GetComponent<BoardManager> ();
		Invoke ("init_game", level_restart_delay);
    }

	// Update is called once per frame
	void Update () {
		if(players_turn || _enemies_moving || _doing_setup)
			return;

		StartCoroutine (move_enemies());
	}

	public void add_enemy_to_list(Enemy script){
		_enemies.Add (script);
	}

	IEnumerator move_enemies(){
		_enemies_moving = true;

		yield return new WaitForSeconds (turn_delay);
		if (_enemies.Count == 0) {
			yield return new WaitForSeconds (turn_delay);
		}

		for (int i = 0; i < _enemies.Count; i++) {
			_enemies [i].move_enemy ();
			yield return new WaitForSeconds (_enemies [i].move_time);
		}

		players_turn = true;
		_enemies_moving = false;
	}
}
