using System;
using UnityEngine;

public delegate void position_changed_handler(Position position);

public class Position : IComponent
{
	public event position_changed_handler position_changed;

	private Vector3 _position;
	public Vector3 position{ 
		get{ return _position; } 
		set { 
			if (_position == value)
				return;//same value

			_position = value;

			if (position_changed != null)
				position_changed (this);
		}
	}

	public Position(){
	}

	public Position(Vector3 position){
		this.position = position;
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


