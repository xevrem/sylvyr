using System;
using UnityEngine;
using System.Collections.Generic;

public static class ResourcePool
{

	private static Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

	public static void load_all(){
		//playerShip3_red
		Sprite[] loaded_sprites  = Resources.LoadAll<Sprite>("sprites");
		foreach (Sprite sprite in loaded_sprites) {
			_sprites.Add (sprite.name, sprite);
		}
	}

	public static Sprite get_sprite_by_name(string name){
		if (_sprites.ContainsKey (name))
			return _sprites [name];
		
		return null;
	}
}


