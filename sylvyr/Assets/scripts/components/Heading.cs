using System;
using UnityEngine;

public delegate void heading_changed_handler(Heading heading);

public class Heading : IComponent
{
	private Vector3 _heading;
	public Vector3 heading{
		get{ return _heading; }
		set{
			if (_heading == value)
				return;
			_heading = value;
			if (heading_changed != null)
				heading_changed (this);
		}
	}

	public event heading_changed_handler heading_changed;

	public Heading ()
	{
	}

	public Heading(Vector2 heading){
		this.heading = heading;
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

