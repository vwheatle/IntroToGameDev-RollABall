using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
	void Start() {
		
	}
	
	void Update() {
		transform.Rotate(new Vector3(15, 30, 0) * Time.deltaTime);
	}
}
