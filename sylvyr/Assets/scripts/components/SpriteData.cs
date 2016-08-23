using UnityEngine;
using System.Collections;


public delegate void sprite_changed_handler(SpriteData ship);

public class SpriteData : IComponent {

	//public  GameObject game_object;

	public string asset_name;
	public string layer_name;

	public SpriteData(){}

	public SpriteData(string asset_name, string layer){
		this.asset_name = asset_name;
		this.layer_name = layer;
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
