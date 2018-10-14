using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAModifyRotationConstraint : CustomActionScript {

	public Rigidbody rigidTarget;

	public bool rotX;
	public bool rotY;
	public bool rotZ;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)	{

		if (rotX) {
			rigidTarget.constraints = rigidTarget.constraints | RigidbodyConstraints.FreezeRotationX;
		} else {
			rigidTarget.constraints &= ~(RigidbodyConstraints.FreezeRotationX);
		}

		if (rotY) {
			rigidTarget.constraints = rigidTarget.constraints | RigidbodyConstraints.FreezeRotationY;
		} else {
			rigidTarget.constraints &= ~(RigidbodyConstraints.FreezeRotationY);
		}

		if (rotZ) {
			rigidTarget.constraints = rigidTarget.constraints | RigidbodyConstraints.FreezeRotationZ;
		} else {
			rigidTarget.constraints &= ~(RigidbodyConstraints.FreezeRotationZ);
		}


		yield return null;
	}
}
