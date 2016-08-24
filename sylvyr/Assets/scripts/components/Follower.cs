using System;


public class Follower : IComponent
{
	public Entity following;

	public Follower ()
	{
	}

	public Follower(Entity entity){
		following = entity;
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

