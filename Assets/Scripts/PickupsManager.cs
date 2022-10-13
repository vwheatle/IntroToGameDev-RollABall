using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupsManager : MonoBehaviour {
	[Header("GameObject Connections")]
	[Tooltip("The UI status text for how many cubes have been collected.")]
	public TMP_Text pickupsStatus;
	
	
	[Header("Typical Pickups")]
	// Don't accumulate floating point inaccuracy!!
	// Instead of storing "time since" a thing, store the
	// timestamp of the original thing and derive the "time since" value!
	private float vulnerabilityStartTime = 0.0f;
	public float vulnerabilitySeconds = 10.0f;
	
	
	[Header("Pickup Goal")]
	private int _collectedPickups = 0;
	public int goalPickups = -1;
	public GameObject goal;
	public int collectedPickups {
		get => _collectedPickups;
		set {
			_collectedPickups = value;
			if (achievedGoalPickups) {
				pickupsStatus.text = "Reach the goal!";
				
				goal.GetComponent<Goal>().UnlockGoal();
			} else {
				pickupsStatus.text = string.Format(
					"{0}/{1} Cubes",
					_collectedPickups.ToString("D2"), goalPickups.ToString("D2")
				);
			}
		}
	}
	public float percentCollectedPickups {
		get => (float)collectedPickups / goalPickups;
	}
	public bool achievedGoalPickups {
		get => collectedPickups >= goalPickups;
	}
	
	private bool _vulnerable = false;
	bool vulnerable {
		get => _vulnerable;
		set {
			if (_vulnerable == value) return;
			
			foreach (Transform pickup in transform) {
				PickupType2 type2 = pickup.GetComponent<PickupType2>();
				if (type2 == null) continue;
				
				if (value) type2.AllowPickup();
				else       type2.DenyPickup();
			}
			
			_vulnerable = value;
		}
	}
	
	public void Reset() {
		// Also resets the UI text.
		collectedPickups = 0;
		
		// Make every pickup interactive again.
		foreach (Transform pickup in transform) {
			Pickup pickupComponent = pickup.gameObject.GetComponent<Pickup>();
			
			if (pickupComponent != null) {
				pickupComponent.pickupsManager = this;
				pickupComponent.Reset();
			} else {
				Debug.LogWarning("unexpected item in bagging area");
			}
		}
	}
	
	void Start() {
		// Set the target number of pickups to collect
		// to the number of children this gameobject has,
		// if the automatic sentinel value is used.
		// Gosh I wish enums with payloads were in C#...
		if (goalPickups < 0)
			goalPickups = transform.childCount;
		
		Reset();
	}
	
	void Update() {
		if (vulnerable) {
			float timeSpent = Time.time - vulnerabilityStartTime;
			if (timeSpent >= vulnerabilitySeconds)
				vulnerable = false;
		}
	}
}
