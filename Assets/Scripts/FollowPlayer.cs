using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	[Tooltip("The player for this camera to follow.")]
	public GameObject player;
	
	[Tooltip("If false, will follow the player without rotating the camera.\nIf true, will use LookAt to look at the player.")]
	private bool _lookAt = false;
	
	// Love to overengineer!!
	public bool lookAt {
		get { return _lookAt; }
		set {
			if (value == false) {
				// Restore initial rotation, as `lookAt` probably changed it.
				transform.rotation = initialRotation;
			}
		}
	}
	
	[Tooltip("The initial rotation of the camera. Will be restored if camera taken out of `lookAt` mode.")]
	private Quaternion initialRotation;
	
	[Tooltip("The positional offset from the camera to the player.\nUnless in `lookAt` mode, this is added to the player's position every frame to get the camera's new position.")]
	private Vector3 offsetPosition;
	
	[Tooltip("The rotational offset from the camera to the player.\nUsed in `lookAt` mode to maintain the offset of the player from the center of the camera's view.")]
	private Quaternion offsetRotation;
	
	void Start() {
		offsetPosition = transform.position - player.transform.position;
		initialRotation = transform.rotation;
		
		// Why does `transform.forward` need to be inverted?
		// No idea!!
		offsetRotation = Quaternion.FromToRotation(offsetPosition.normalized, -transform.forward);
	}
	
	void LateUpdate() {
		if (lookAt) {
			// Look at the player's position.
			transform.LookAt(player.transform.position, Vector3.up);
			
			// Apply difference between player rotation and how the camera was originally rotated.
			transform.rotation *= offsetRotation;
		} else {
			// Position the camera behind the player, using a set distance implied by the initial position of the camera.
			transform.position = player.transform.position + offsetPosition;
		}
	}
}
