using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacerAutourWithRigidB : MonoBehaviour {

	public bool actifAuDebut;

	/*
	 * y : normal de rotation
	 * */
	public Transform centre;
	public float degreeParSec;

	/**
	 * param x et z
	 * */
	public Vector2 coefOval;

	private Rigidbody objectRignidB;
	private bool actif;
	private float rayon;
	private float angle;

	void Start (){
		this.actif = this.actifAuDebut;
		objectRignidB = transform.GetComponent<Rigidbody> ();
		Vector3 positionEnLocal = centre.InverseTransformPoint (transform.position);
		if (coefOval.x != 0 && coefOval.y != 0) {
			rayon = positionEnLocal.x / coefOval.x + positionEnLocal.z / coefOval.y;
			angle = Mathf.Acos(positionEnLocal.x/(new Vector2 (positionEnLocal.x,positionEnLocal.z).magnitude)) * Mathf.Rad2Deg;
		} else {
			this.actif = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (this.actif && null != objectRignidB) {
			if (coefOval.x == 0 || coefOval.y == 0) {
				this.actif = false;
				return;
			}

			Vector3 positionEnLocal = new Vector3 (rayon * coefOval.x * Mathf.Cos (angle * Mathf.Deg2Rad), 0, rayon * coefOval.y * Mathf.Sin (angle * Mathf.Deg2Rad));
			Vector3 positionEnWorld = centre.TransformPoint (positionEnLocal);

			if (Time.deltaTime > 0) {
				objectRignidB.velocity = (positionEnWorld - transform.position) / Time.deltaTime;
			}

			angle += degreeParSec * Time.deltaTime;
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
