using System;


public class Faction : IComponent
{
	public string faction;


	public Faction ()
	{
	}

	public Faction(string faction){
		this.faction = faction;
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
