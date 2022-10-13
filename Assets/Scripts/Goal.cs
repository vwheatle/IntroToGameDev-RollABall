using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
	private Collider me;
	
	void Start() {
		me = GetComponent<Collider>();
	}
	
	public void LockGoal() { me.isTrigger = false; }
	public void UnlockGoal() { me.isTrigger = true; }
}
