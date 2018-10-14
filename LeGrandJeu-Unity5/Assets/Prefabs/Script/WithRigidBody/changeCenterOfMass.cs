using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCenterOfMass : MonoBehaviour {

	public Rigidbody rigidTarget;
	public Transform newCenterOfMass;


	void Start () {
		rigidTarget.centerOfMass = newCenterOfMass.position;
	}
}
