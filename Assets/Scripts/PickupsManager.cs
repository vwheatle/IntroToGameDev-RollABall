using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsManager : MonoBehaviour {
	public float vulnerabilitySeconds = 10.0f;
	
	bool _vulnerable;
	
	bool vulnerable {
		get { return _vulnerable; }
		set {
			if (_vulnerable == value) return;
			
			// TODO: go through and activate / deactivate all
			// Type-2 cube
			if (value) {
				
			}
			
			_vulnerable = value;
		}
	}
	
	// Don't accumulate floating point inaccuracy!!
	// Instead of storing "time since" a thing, store the
	// timestamp of the original thing and derive the "time since" value!
	float vulnerabilityStartTime = 0.0f;
	
	void Start() {
		
	}
	
	void Update() {
		if (vulnerable) {
			float timeSpent = Time.time - vulnerabilityStartTime;
			if (timeSpent >= vulnerabilitySeconds)
				vulnerable = false;
		}
	}
}
