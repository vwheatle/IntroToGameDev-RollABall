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
	
	void Start() {
		// UNITY WHY IS THIS UNDER TRANSFORM???
		// THIS HAS NO RELATION TO SPATIAL SHIT!
		goal = collectibles.transform.childCount;
		
		// Anyway,
		UpdateCubesStatus();
	}
	
	void UpdateCubesStatus() {
		// cubesStatus.text = $"{collected}/{goal} Cubes";
		cubesStatus.text = string.Format("{0,2}/{1,2} Cubes", collected.ToString("D2"), goal.ToString("D2"));
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Pickup")) {
			other.gameObject.SetActive(false);
			collected++;
			UpdateCubesStatus();
		}
	}
}
