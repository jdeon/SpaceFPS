using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneADegats : MonoBehaviour {

	public int degatParPeriod;
	public float dureeDePeriode;

	private bool inZone;


	// Use this for initialization
	void Start () {
		inZone = false;
	}
	
	void OnTriggerEnter(Collider other) {
		inZone = true;
		ISystemVie gestionVie = other.GetComponent<ISystemVie> ();
		if (null != gestionVie) {
			StartCoroutine ("degatInZone", gestionVie);
		}
	}

	void OnTriggerExit(Collider other) {
		inZone = false;
	}

	private IEnumerator degatInZone (ISystemVie gestionVie){
		while (inZone) {
			gestionVie.blesse (this.degatParPeriod);
			yield return new WaitForSeconds (dureeDePeriode);
		}
		yield return null;
	}
}
