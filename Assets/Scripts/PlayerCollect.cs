using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollect : MonoBehaviour {
	[Tooltip("The UI status text for how long the player has been in control.")]
	public TMP_Text timeStatus;
	
	private PickupsManager pickupsManager;
	
	private bool stopTimer = false;
	private float startTime;
	private float goalTime;
	
	void Start() {
		startTime = Time.time;
		
		pickupsManager = GameObject.FindWithTag("PickupsManager").GetComponent<PickupsManager>();
	}
	
	void Update() {
		if (Input.GetAxis("Reset") > 0.05f) Reset();
		
		if (!stopTimer) goalTime = Time.time - startTime;
		timeStatus.text = string.Format("{0}", goalTime.ToString("F2"));
	}
	
	void Reset() {
		GetComponent<PlayerMovement>().Reset();
		
		pickupsManager.GetComponent<PickupsManager>().Reset();
		
		GameObject goal = GameObject.FindWithTag("Goal");
		if (!goal) { Debug.LogWarning("Missing goal!"); return; }
		
		Collider gc = goal.GetComponent<Collider>();
		gc.isTrigger = true;
		
		stopTimer = false;
		startTime = Time.time;
	}
	
	void OnTriggerEnter(Collider other) {
		// // silly mode
		// Rigidbody r = GetComponent<Rigidbody>();
		// r.AddExplosionForce(99f, other.gameObject.transform.position, 1f, 9f);
		
		// TODO:
		//  - make player unaware of pickups' state.
		//  - make pickups report to the manager when a player hits one.
		//    -> give player "Player" tag (for `CompareTag`)
		//    -> in turn makes pickups aware of the manager, eliminating
		//       the need for type 2 cubes to keep any additional state.
		//    -> each pickup is now in charge of its own activeness,
		//       eliminating the need for `TryCollect`.
		//  - give pickups a sound source
		//    -> can easily pitch up using `percentCollectedPickups`
		//  - goal could be in charge of timing?
		
		if (other.gameObject.CompareTag("Goal")) {
			if (pickupsManager.achievedGoalPickups && !stopTimer) {
				GetComponent<PlayerMovement>().GoalMovementLock();
				pickupsManager.pickupsStatus.text = "Goal!!!";
				stopTimer = true;
			}
		}
	}
}
