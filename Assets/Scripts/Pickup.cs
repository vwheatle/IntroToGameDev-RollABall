using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
	private Vector2 xzPos;
	
	void Start() {
		xzPos = new Vector2(transform.position.x, transform.position.z);
		// myChild.rotation = Random.rotationUniform;
	}
	
	void Update() {
		float funny = Time.realtimeSinceStartup + (xzPos.x + xzPos.y) / 8f;
		float yOffset = Mathf.Abs(Mathf.Cos(Mathf.PI * funny)) / 4f;
		
		transform.localPosition = Vector3.up * yOffset;
		transform.Rotate(new Vector3(15, 30, 0) * Time.deltaTime);
	}
	
	public virtual bool TryCollect() { return true; }
}
