using UnityEngine;

public class PickupType1 : Pickup {
	public override bool TryCollect() {
		Debug.Log("indeed, tyou cannot collect me because i am type 1!!");
		return false;
		// return base.TryCollect();
	}
}