using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	float sound_cooldown = 0f;

	// Use this for initialization
	void Start () {
		WorldController.instance.world.on_feature_created += handle_feature_created;
		WorldController.instance.world.on_tile_changed += handle_tile_change;
	}
	
	// Update is called once per frame
	void Update () {
		sound_cooldown -= Time.deltaTime;
	}

	void handle_tile_change(Tile tile_data){
		if (sound_cooldown>0) {
			return;
		}

		AudioClip clip = ResourcePool.get_audio_clip_by_name ("floor_on_create");
		AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position);
		sound_cooldown = 0.1f;
	}

	public void handle_feature_created(Feature feature){
		if (sound_cooldown>0) {
			return;
		}

		AudioClip clip = ResourcePool.get_audio_clip_by_name ("wall_on_create");
		AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position);
		sound_cooldown = 0.1f;
	}


}
