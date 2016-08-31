using UnityEngine;
using System.Collections;

public enum DamageType{COMMON
}

public enum DurationType{ONE_SHOT
}

public class Damage : IComponent {

	public Entity target;
	public float damage_amount;
	public DamageType damage_type;
	public DurationType duration_type;
	public float lifetime;
	public float elapsed_time;


	public Damage(){}

	public Damage(Entity target, float damage_amount, DamageType damage_type=DamageType.COMMON,
				  DurationType duration_type=DurationType.ONE_SHOT){
		this.target = target;
		this.damage_amount = damage_amount;
		this.damage_type = damage_type;
		this.duration_type = duration_type;
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
