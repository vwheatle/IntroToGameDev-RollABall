using UnityEngine;

public class PickupType2 : Pickup {
	public Material vulnerable;
	public Material shielded;
	
	public bool isClone = false;
	
	public override bool TryCollect(Collider other) {
		if (!pickupsManager.vulnerable) {
			// bonk!!
			other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, 10f, 5f);
			
			PickupType2 n = Instantiate(this, new Vector3(
				Random.Range(-20f, 20f), 1f, Random.Range(-30f, 30f)
			), Quaternion.identity, this.transform.parent);
			n.isClone = true;
		}
		
		return pickupsManager.vulnerable;
	}
	
	public void AllowPickup() {
		GetComponentInChildren<Renderer>().material = vulnerable;
	}
	public void DenyPickup() {
		GetComponentInChildren<Renderer>().material = shielded;
	}
}
