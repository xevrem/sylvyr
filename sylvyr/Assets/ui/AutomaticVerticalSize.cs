using UnityEngine;
using System.Collections;

public class AutomaticVerticalSize : MonoBehaviour {

	public float child_height = 35f;

	// Use this for initialization
	void Start () {
		adjust_size ();
	}


	void foo(){
	}

	public void adjust_size ()
	{
		Vector2 size = this.GetComponent<RectTransform> ().sizeDelta;
		size.y = this.transform.childCount * child_height;
		this.GetComponent<RectTransform> ().sizeDelta = size;
	}
}
