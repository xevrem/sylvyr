using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ResourcePool {


	static Object[] creature_sprites;

	public static Object[] Creature_Sprites {
		get {
			return ResourcePool.creature_sprites;
		}
	}

	static Object[] tile_sprites;

	public static Object[] Tile_Sprites {
		get {
			return ResourcePool.tile_sprites;
		}
	}

	public static void load_all(){
		ResourcePool.tile_sprites  = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/sprites/tiles.png"); 
		Debug.Log (ResourcePool.tile_sprites.Length + " Tile Sprites loaded");

		ResourcePool.creature_sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath ("Assets/sprites/creatures.png");
		Debug.Log (ResourcePool.creature_sprites.Length + " Creture Sprites loaded");
	}

}
