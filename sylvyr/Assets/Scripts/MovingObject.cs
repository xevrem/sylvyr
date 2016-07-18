using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float move_time = 0.1f;
	public LayerMask blocking_layer;

	private BoxCollider2D box_collider;
	private Rigidbody2D rb2d;
	private float inverse_move_time;


	// Use this for initialization
	protected virtual void Start () {
		box_collider = GetComponent<BoxCollider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();
		inverse_move_time = 1f / move_time;
	}

	protected bool move(int xdir, int ydir, out RaycastHit2D hit){
		Vector2 start = (Vector2)transform.position;
		Vector2 end = start + new Vector2 (xdir, ydir);

		box_collider.enabled = false;
		hit = Physics2D.Linecast (start, end, blocking_layer);
		box_collider.enabled = true;

		if (hit.transform == null) {
			StartCoroutine (smooth_movement (end));
			return true;
		}

		return false;
	}


	protected IEnumerator smooth_movement (Vector3 end){
		float sq_remaining_dist = (transform.position - end).sqrMagnitude;

		while (sq_remaining_dist > float.Epsilon) {
			Vector3 new_pos = Vector3.MoveTowards (rb2d.position, end, inverse_move_time * Time.deltaTime);
			rb2d.MovePosition (new_pos);
			sq_remaining_dist = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void attempt_move<T>(int xdir, int ydir) where T: Component{
		RaycastHit2D hit;
		bool can_move = move (xdir, ydir, out hit);
		if (hit.transform == null)
			return;

		T hit_compoenent = hit.transform.GetComponent<T> ();

		if (!can_move && hit_compoenent != null)
			on_cant_move(hit_compoenent);
	}
		
	protected abstract void on_cant_move<T> (T component) where T: Component;

}
