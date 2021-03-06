﻿using System;
using System.Collections.Generic;

public class Reputation : IComponent
{
	public Dictionary<string, float> reputations;

	public Reputation (){}

	public Reputation (Dictionary<string, float> reputations)
	{
		this.reputations = reputations;
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

