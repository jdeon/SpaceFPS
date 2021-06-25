using UnityEngine;
using System.Collections;

public class RotationContinue : MonoBehaviour, IActivable {

	public bool actifAuDebut;

	public float AngleXParSec = 10f;
	public float AngleYParSec = 10f;
	public float AngleZParSec = 10f;

	private bool actif;

	void Start (){
		this.actif = this.actifAuDebut;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (this.actif) {
			transform.Rotate (AngleXParSec * Time.deltaTime, AngleYParSec * Time.deltaTime, AngleZParSec * Time.deltaTime);
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
