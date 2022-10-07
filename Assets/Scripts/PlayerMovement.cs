using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	public bool locked = false;
	
	public float speed = 5f;
	public bool touchingGround = false;
	
	public float forceAmount = 0.1f;
	public float torqueAmount = 1.0f;
	
	public float idleDragMultiplier = 3.0f;
	
	private Rigidbody rb;
	private SphereCollider c;
	
	// Tuple holding `drag` and `angularDrag` as items 1 and 2 respectively,
	// to get restored once no longer idle.
	private (float, float) dragSettings;
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		c = GetComponent<SphereCollider>();
		
		dragSettings = (rb.drag, rb.angularDrag);
	}

	void FixedUpdate() {
		touchingGround = Physics.Raycast(
			new Ray(transform.position, Vector3.down),
			c.radius + 0.05f
		);
		// TODO: i forget if this is best practices
		
		float moveHoriz = Input.GetAxis("Horizontal");
		float moveVert  = Input.GetAxis("Vertical");
		
		if (locked) { moveHoriz = 0.0f; moveVert = 0.0f; }
		
		// Higher amount of drag when grounded and no input given, to make ball stop faster.
		// TODO: Even on inclines, I guess.
		if (touchingGround
		&& Mathf.Abs(moveHoriz) < 0.025f
		&& Mathf.Abs(moveVert) < 0.025f) {
			rb.drag = dragSettings.Item1 * idleDragMultiplier;
			rb.angularDrag = dragSettings.Item2 * idleDragMultiplier;
		} else {
			(rb.drag, rb.angularDrag) = dragSettings;
		}
		
		Vector3 movementForce = new Vector3(moveHoriz, 0f, moveVert);
		Vector3 movementTorque = new Vector3(moveVert, 0f, -moveHoriz);
		
		// Small amount of AddForce in midair, to make ball "responsive"
		if (!touchingGround)
			rb.AddForce(movementForce * speed * forceAmount);
		
		rb.AddTorque(movementTorque * speed * torqueAmount);
	}
}
