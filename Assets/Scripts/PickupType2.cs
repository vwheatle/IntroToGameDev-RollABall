using UnityEngine;

public class PickupType2 : Pickup {
	public override bool TryCollect() { return allowsPickup; }
	
	private bool allowsPickup = false;
	public void AllowPickup() { allowsPickup = true; }
	public void DenyPickup() { allowsPickup = false; }
}
