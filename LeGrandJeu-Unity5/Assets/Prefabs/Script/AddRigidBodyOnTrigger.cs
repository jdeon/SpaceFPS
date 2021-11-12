using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigidBodyOnTrigger : MonoBehaviour {

	public Vector3 volume;
	public Vector3 offset;
	public bool isKinectic;

	void Start () {
		BoxCollider detectBox = gameObject.AddComponent<BoxCollider> ();
		detectBox.center = offset;
		detectBox.isTrigger = true;

		//Seul le controller declenche l effet
		gameObject.layer = Constantes.LAYER_DETECT_ZONE;

		if (Vector3.zero.Equals (volume)) {
			volume = transform.localScale;
			detectBox.size = new Vector3 (2, 2, 2);
		} else {
			detectBox.size = 2 * volume;
		}
	}

	public void OnTriggerEnter (Collider other){
		if (null == gameObject.GetComponent<Rigidbody> ()) {
			Rigidbody rigidtarget = gameObject.AddComponent<Rigidbody> ();
			rigidtarget.useGravity = false;
			rigidtarget.freezeRotation = true;
			rigidtarget.mass = 100*volume.x * volume.y * volume.z;
			rigidtarget.isKinematic = isKinectic;
		}
	}
}
