using System;
using UnityEngine;

public delegate void go_changed_handler(GOData go);

public class GOData : IComponent
{
	public event go_changed_handler go_changed;

	private GameObject _game_object;
	public GameObject game_object{ 
		get{ return _game_object; } 
		set { 
			if (_game_object == value)
				return;//same value

			_game_object = value;

			if (go_changed != null)
				go_changed (this);
		}
	}

	public GOData(){
	}

	public GOData(Vector3 position){
		_game_object = new GameObject ();
		_game_object.transform.position = position;
	}

	#region IComponent implementation

	public int id { get; set; }

	public int owner_id { get; set; }

	private static int _type_id;
	public int type_id {
		get{ return _type_id; }
		set{ _type_id = value; }
	}
	#endregion
}


