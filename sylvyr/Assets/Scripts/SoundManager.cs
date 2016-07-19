using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource effects_source;
	public AudioSource music_source;

	public static SoundManager instance = null;

	public float low_pitch_range = 0.95f;
	public float high_pitch_range = 1.05f;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	public void play_single(AudioClip clip){
		effects_source.clip = clip;
		effects_source.Play ();
	}

	public void randomize_effects(params AudioClip[] clips){
		int random_index = Random.Range (0, clips.Length);
		float random_pitch = Random.Range (low_pitch_range, high_pitch_range);

		effects_source.pitch = random_pitch;
		effects_source.clip = clips [random_index];
		effects_source.Play ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
