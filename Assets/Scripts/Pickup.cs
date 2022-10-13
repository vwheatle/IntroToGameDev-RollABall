using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
	private Vector2 xzPos;
	
	public PickupsManager pickupsManager;
	
	private AudioSource audioSrc;
	
	private bool collected = false;
	
	void Start() {
		audioSrc = GetComponent<AudioSource>();
		
		xzPos = new Vector2(transform.position.x, transform.position.z);
	}
	
	public void Reset() {
		collected = false;
		GetComponentInChildren<Renderer>().enabled = true;
	}
	
	void Update() {
		float funny = Time.realtimeSinceStartup + (xzPos.x + xzPos.y) / 8f;
		float yOffset = Mathf.Abs(Mathf.Cos(Mathf.PI * funny)) / 4f;
		
		Transform myChild = transform.GetChild(0);
		
		myChild.localPosition = Vector3.up * yOffset;
		myChild.Rotate(new Vector3(15, 30, 0) * Time.deltaTime);
	}
	
	void OnTriggerEnter(Collider other) {
		if (collected) return;
		
		if (other.gameObject.CompareTag("Player")) {
			if (TryCollect(other)) {
				collected = true;
				GetComponentInChildren<Renderer>().enabled = false;
				
				pickupsManager.collectedPickups++;
				
				audioSrc.pitch = Mathf.Lerp(0.8f, 1.2f, pickupsManager.percentCollectedPickups);
				audioSrc.Play();
			}
		}
	}
	
	public virtual bool TryCollect(Collider other) { return true; }
}
