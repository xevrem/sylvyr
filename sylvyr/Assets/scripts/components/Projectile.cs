using UnityEngine;
using System.Collections;

//entity is the entity collided with
public delegate void hit_handler(Entity entity);

public class Projectile : IComponent {

	public Entity creator;
	public float elapsed_time;
	public float time_to_live;
	public float speed;

	private hit_handler _hit_cb;


	public Projectile(){}

	public Projectile(Entity creator, float speed=10f, float time_to_live=1f, hit_handler hit_cb=null ){
		this.creator = creator;
		this.speed = speed;
		this.time_to_live = time_to_live;
		this._hit_cb = hit_cb;
	}

	public void hit(Entity entity){
		if (_hit_cb != null)
			_hit_cb (entity);
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
