using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationContinueWithRigidB : MonoBehaviour, IActivable {

	public bool actifAuDebut;

	public float AngleXParSec = 10f;
	public float AngleYParSec = 10f;
	public float AngleZParSec = 10f;

	private Rigidbody objectRignidB;

	private bool actif;

	void Start (){
		this.actif = this.actifAuDebut;
		objectRignidB = transform.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (this.actif && null != objectRignidB) {
			//Probleme avec axxe locaux
			//objectRignidB.AddTorque((AngleXParSec - objectRignidB.angularVelocity.x) * transform.right + (AngleYParSec - objectRignidB.angularVelocity.y) * transform.up + (AngleZParSec - objectRignidB.angularVelocity.z) * transform.forward,ForceMode.VelocityChange);
			objectRignidB.isKinematic = false;
			//objectRignidB.MoveRotation(transform.rotation * Quaternion.Euler(AngleXParSec, AngleYParSec, AngleZParSec));
			objectRignidB.AddTorque((Mathf.Deg2Rad * AngleXParSec - objectRignidB.angularVelocity.x), (Mathf.Deg2Rad * AngleYParSec - objectRignidB.angularVelocity.y), (Mathf.Deg2Rad * AngleZParSec - objectRignidB.angularVelocity.z),ForceMode.VelocityChange);
			//objectRignidB.angularVelocity = new Vector3 (Mathf.Deg2Rad * AngleXParSec, Mathf.Deg2Rad * AngleYParSec , Mathf.Deg2Rad * AngleZParSec);
		}
	}

	public bool getIsActif(){
		return this.actif;
	}

	public void activate(){
		this.actif = true;
	}

	public void desactivate(){
		this.actif = false;
	}
}