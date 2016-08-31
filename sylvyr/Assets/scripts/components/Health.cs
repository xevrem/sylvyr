using System;

public class Health : IComponent
{

	public float current_hp;
	public float max_hp;

	//rate in seconds
	public float regen_rate;
	public float regen_amount;

	public float elapsed_regen_time = 0f;

	public Health ()
	{
	}

	public Health(float max_hp, float regen_amount=1f, float regen_rate=1f){
		this.max_hp = max_hp;
		this.current_hp = max_hp;
		this.regen_amount = regen_amount;
		this.regen_rate = regen_rate;
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

