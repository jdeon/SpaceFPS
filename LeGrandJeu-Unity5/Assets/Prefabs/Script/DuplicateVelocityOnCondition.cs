using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateVelocityOnCondition : MonoBehaviour {

	public ConditionEventAbstract conditon;
	public Rigidbody rigidTarget;
	public bool dupltcateXVelocity;
	public bool dupltcateYVelocity;
	public bool dupltcateZVelocity;

	public Rigidbody thisRigid;

	// Use this for initialization
	void Start () {
		//thisRigid = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(null != thisRigid && null!= conditon && conditon.getIsActive()){
			Vector3 newVelocity = thisRigid.velocity;

			if (dupltcateXVelocity) {
				newVelocity.x = rigidTarget.velocity.x;
			}
			if (dupltcateYVelocity) {
				newVelocity.y = rigidTarget.velocity.y;
			}
			if (dupltcateZVelocity) {
				newVelocity.z = rigidTarget.velocity.z;
			}

			thisRigid.velocity = newVelocity;
		}
	}
}
