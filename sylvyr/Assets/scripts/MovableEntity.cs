using UnityEngine;
using System.Collections;

public abstract class MovableEntity : Entity {

	public float move_time = 0.1f;
	public LayerMask blocking_layer;


	private BoxCollider2D box_collider;
	private Rigidbody2D rigid_body;
	private float inv_move_time;

	void Awake (){
		inv_move_time = 1f / move_time;
	}

	// Use this for initialization
	void Start () {
		//get any latest changes to the box collider
		box_collider = GetComponent<BoxCollider2D> ();

		//get any latest changes to the rigid body
		rigid_body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected bool move(int x, int y, out RaycastHit2D hit){
		//determine start and end positions
		Vector2 start = (Vector2) this.transform.position;
		Vector2 end = start + new Vector2 (x, y);	

		//perform collision detection
		box_collider.enabled = false;
		hit = Physics2D.Linecast (start, end, blocking_layer);
		box_collider.enabled = true;

		//if no hit, move to that location
		if (hit.transform == null) {
			StartCoroutine (smooth_movement (end));
			return true;
		}

		return false;
	}

	protected IEnumerator smooth_movement (Vector3 end){
		float sq_remaining_dist = (transform.position - end).sqrMagnitude;

		while (sq_remaining_dist > float.Epsilon) {
			Vector3 new_pos = Vector3.MoveTowards (rigid_body.position, end, inv_move_time * Time.deltaTime);
			rigid_body.MovePosition (new_pos);
			sq_remaining_dist = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void attempt_move<T>(int x, int y) where T: Component{
		RaycastHit2D hit;
		bool can_move = move (x, y, out hit);
		if (hit.transform == null)
			return;

		//we hit something
		T component = hit.transform.GetComponent<T>();

		if (component != null && !can_move)
			on_cant_move (component);
	}

	protected abstract void on_cant_move<T> (T component) where T: Component;
}
