using UnityEngine;

public class PickupType1 : Pickup {
	public override bool TryCollect(Collider other) {
		pickupsManager.vulnerable = true;
		pickupsManager.vulnerabilitySeconds -= 1.0f;
		return true;
	}
}
