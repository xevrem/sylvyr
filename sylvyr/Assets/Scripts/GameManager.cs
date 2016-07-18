using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager board_script;
	public int player_food_points;
	[HideInInspector] public bool players_turn = true;

	private int _level = 3;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		board_script = GetComponent<BoardManager> ();
		init_game();
	}


	void init_game(){
		board_script.setup_scene (_level);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
