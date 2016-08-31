using System;


public class Target : IComponent
{
	public Entity target;

	public Target ()
	{
	}

	public Target(Entity entity){
		target = entity;
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

