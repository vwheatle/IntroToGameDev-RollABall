using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollect : MonoBehaviour {
	[Tooltip("A GameObject holding a list of collectible cubes.")]
	public GameObject collectibles;
	
	[Tooltip("The UI status text for how many cubes the player has collected.")]
	public TMP_Text cubesStatus;
	
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
		// cubesStatus.text = $"{collected}/{goal} Cubes";
		cubesStatus.text = string.Format("{0,2}/{1,2} Cubes", collected.ToString("D2"), goal.ToString("D2"));
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Pickup")) {
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
		}
	}
}
