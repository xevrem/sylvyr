using UnityEngine;
using System.Collections;


public delegate void sprite_changed_handler(SpriteData sprite);

public class SpriteData : IComponent {

	//public  GameObject game_object;
	private string _asset_name;
	public string asset_name{
		get{ return _asset_name; }
		set{
			if (_asset_name == value)
				return;
			_asset_name = value;
			if(sprite_changed != null)
				sprite_changed (this);
		}
	}
	public string layer_name;

	public event sprite_changed_handler sprite_changed;

	public SpriteData(){}

	public SpriteData(string asset_name, string layer){
		this._asset_name = asset_name;
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
