using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollect : MonoBehaviour {
	[Tooltip("A GameObject holding a list of collectible cubes.")]
	public GameObject collectibles;
	
	[Tooltip("The UI status text for how many cubes the player has collected.")]
	public TMP_Text cubesStatus;
	
	public bool achievedGoal = false;
	
	private int collected = 0;
	private int goal = 1;
	
	private AudioSource audioSrc;
	
	void Start() {
		// Get audio source to make blip sounds later
		audioSrc = GetComponent<AudioSource>();
		
		if (collectibles) {
			// Set the target number of cubes to collect
			// to the number of children the collectibles object has.
			goal = collectibles.transform.childCount;
		}
		
		// Initialize UI cubes display
		UpdateCubesStatus();
	}
	
	void UpdateCubesStatus() {
		achievedGoal = collected >= goal;
		if (achievedGoal) {
			cubesStatus.text = "Reach the goal!";
		} else {
			cubesStatus.text = string.Format("{0}/{1} Cubes", collected.ToString("D2"), goal.ToString("D2"));
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Pickup")) {
			// // silly mode
			// Rigidbody r = GetComponent<Rigidbody>();
			// r.AddExplosionForce(99f, other.gameObject.transform.position, 1f, 9f);
			
			// Disable pickup
			other.gameObject.SetActive(false);
			
			// Increment number of collected cubes
			collected++;
			
			// Update UI text displaying number of collected cubes
			UpdateCubesStatus();
			
			// Play little blip sound effect
			// (pitch raises based on how many of the goal you've collected)
			audioSrc.pitch = Mathf.Lerp(0.8f, 1.2f, collected / (float)goal);
			audioSrc.Play();
			
			// Unlock goal if you got all the cubes
			if (collected >= goal) {
				GameObject goal = GameObject.FindWithTag("Goal");
				if (!goal) { Debug.LogWarning("Missing goal!"); return; }
				
				Collider gc = goal.GetComponent<Collider>();
				gc.isTrigger = true;
			}
		} else if (other.gameObject.CompareTag("Goal")) {
			if (collected >= goal) {
				GetComponent<PlayerMovement>().GoalMovementLock();
				cubesStatus.text = "Goal!!!";
			}
		}
	}
}
