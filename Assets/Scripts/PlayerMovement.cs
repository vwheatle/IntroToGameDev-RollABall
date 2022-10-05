using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	public float speed = 5f;
	
	public float forceAmount = 0.1f;
	public float torqueAmount = 1.0f;
	
	public float idleDragMultiplier = 3.0f;
	
	private Rigidbody rb;
	private SphereCollider c;
	
	private (float, float) dragSettings;
	
	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
		c = GetComponent<SphereCollider>();
		
		dragSettings = (rb.drag, rb.angularDrag);
	}

	// Update is called once per frame
	void FixedUpdate() {
		RaycastHit _hit;
		bool touchingGround = c.Raycast(new Ray(transform.position, Vector3.down), out _hit, c.radius);
		
		float moveHoriz = Input.GetAxis("Horizontal");
		float moveVert  = Input.GetAxis("Vertical");
		
		if (touchingGround
		&& Mathf.Abs(moveHoriz) < 0.025f
		&& Mathf.Abs(moveVert) < 0.025f) {
			// TODO: this doesn't actually do anything
			rb.drag = dragSettings.Item1 * idleDragMultiplier;
			rb.angularDrag = dragSettings.Item2 * idleDragMultiplier;
		} else {
			(rb.drag, rb.angularDrag) = dragSettings;
		}
		
		Vector3 movementForce = new Vector3(moveHoriz, 0f, moveVert);
		Vector3 movementTorque = new Vector3(moveVert, 0f, -moveHoriz);
		
		if (!touchingGround)
			rb.AddForce(movementForce * speed * forceAmount);
		
		rb.AddTorque(movementTorque * speed * torqueAmount);
		
		// Supremely Monkeyless Ball
		//
		// Ideas:
		// [x] small amount of AddForce in midair, to make ball "responsive"
		// [x] higher amount of drag when no input given, to make ball stop faster
		
		// transform.Rotate(new Vector3(15, 0, 0) * Time.deltaTime);
		// transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime);
	}
}
