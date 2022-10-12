using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollect : MonoBehaviour {
	[Tooltip("A GameObject holding a list of collectible cubes.")]
	public GameObject collectibles;
	
	[Tooltip("The UI status text for how many cubes the player has collected.")]
	public TMP_Text cubesStatus;
	
	[Tooltip("The UI status text for how long the player has been in control.")]
	public TMP_Text timeStatus;
	
	public bool collectedAllCubes = false;
	private bool wentInsideGoal = false;
	
	private int collectedCubes = 0;
	private int goalCubes = 1;
	
	private AudioSource audioSrc;
	
	private float startTime;
	private float goalTime;
	
	void Start() {
		// Get audio source to make blip sounds later
		audioSrc = GetComponent<AudioSource>();
		
		if (collectibles) {
			// Set the target number of cubes to collect
			// to the number of children the collectibles object has.
			goalCubes = collectibles.transform.childCount;
		}
		
		// Initialize UI cubes display
		UpdateCubesStatus();
		
		startTime = Time.time;
	}
	
	void Update() {
		if (Input.GetAxis("Reset") > 0.05f) Reset();
		
		if (!wentInsideGoal) goalTime = Time.time - startTime;
		timeStatus.text = string.Format("{0}", goalTime.ToString("F2"));
	}
	
	void UpdateCubesStatus() {
		collectedAllCubes = collectedCubes >= goalCubes;
		if (collectedAllCubes) {
			cubesStatus.text = "Reach the goal!";
		} else {
			cubesStatus.text = string.Format("{0}/{1} Cubes", collectedCubes.ToString("D2"), goalCubes.ToString("D2"));
		}
	}
	
	void Reset() {
		collectedAllCubes = false;
		wentInsideGoal = false;
		
		collectedCubes = 0;
		foreach (Transform cube in collectibles.transform) {
			cube.gameObject.SetActive(true);
		}
		UpdateCubesStatus();
		
		GetComponent<PlayerMovement>().Reset();
		
		
		GameObject goal = GameObject.FindWithTag("Goal");
		if (!goal) { Debug.LogWarning("Missing goal!"); return; }
		
		Collider gc = goal.GetComponent<Collider>();
		gc.isTrigger = true;
		
		startTime = Time.time;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Pickup")) {
			Pickup pickup = other.transform.GetChild(0).GetComponent<Pickup>();
			if (!pickup.TryCollect()) return;
			
			// // silly mode
			// Rigidbody r = GetComponent<Rigidbody>();
			// r.AddExplosionForce(99f, other.gameObject.transform.position, 1f, 9f);
			
			// Disable pickup
			other.gameObject.SetActive(false);
			
			// Increment number of collected cubes
			collectedCubes++;
			
			// Update UI text displaying number of collected cubes
			UpdateCubesStatus();
			
			// Play little blip sound effect
			// (pitch raises based on how many of the goal you've collected)
			audioSrc.pitch = Mathf.Lerp(0.8f, 1.2f, collectedCubes / (float)goalCubes);
			audioSrc.Play();
			
			// Unlock goal if you got all the cubes
			if (collectedCubes >= goalCubes) {
				GameObject goal = GameObject.FindWithTag("Goal");
				if (!goal) { Debug.LogWarning("Missing goal!"); return; }
				
				Collider gc = goal.GetComponent<Collider>();
				gc.isTrigger = true;
			}
		} else if (other.gameObject.CompareTag("Goal")) {
			if (collectedCubes >= goalCubes && !wentInsideGoal) {
				GetComponent<PlayerMovement>().GoalMovementLock();
				cubesStatus.text = "Goal!!!";
				wentInsideGoal = true;
			}
		}
	}
}
