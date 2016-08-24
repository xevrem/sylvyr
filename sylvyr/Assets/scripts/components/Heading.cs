using System;
using UnityEngine;

public class Heading : IComponent
{

	public Vector3 heading;

	public Heading ()
	{
	}

	public Heading(Vector3 heading){
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

