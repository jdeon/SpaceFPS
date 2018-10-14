using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeActivationZoneDansLOrdre : EnigmeAbstract {

	//List sert uniquel
	public List<MonoBehaviour> listZoneActivation;

	private List<IActivable> listIActivation;

	void Start(){
		this.enigmeResolu = false;

		listIActivation = new List<IActivable> ();
		foreach (MonoBehaviour zoneActivable in listZoneActivation) {
			IActivable activableObject = zoneActivable.GetComponent<IActivable> ();
			if (null != activableObject) {
				listIActivation.Add (activableObject);
			}
		}

		listZoneActivation.Clear ();
	}


	// Use this for initialization
	void OnTriggerEnter(Collider other) {
		if (null != listIActivation && !this.enigmeResolu){
			bool isBonneOrdre = checkActivationDansOrdre();
			if (!isBonneOrdre) {
				StartCoroutine ("desactiverToutObjet");
			} else {
				this.enigmeResolu = checkAllActivation ();
			}
		}
		
	}

	/**On vérifie qu'il n'y a pas d'objet désactif pris en sandwich entre 2 objet actif
	 * */
	private bool checkActivationDansOrdre(){
		bool objetDesactifTrouver = false;
		for (int index = 0; index < this.listIActivation.Count; index++) {
			if (!this.listIActivation [index].getIsActif()) {
				objetDesactifTrouver = true;
				continue;
			}

			if (objetDesactifTrouver && this.listIActivation [index].getIsActif()) {
				return false;
			}
		}

		return true;
	}

	/**
	 * On vérifie si tous les objet son actif
	 * */
	private bool checkAllActivation (){
		for (int index = 0; index < this.listIActivation.Count; index++) {
			if (!this.listIActivation [index].getIsActif()) {
				return false;
			}
		}
		return true;
	}

	private IEnumerator desactiverToutObjet(){
		yield return new WaitForSeconds (.5f);
		foreach (IActivable iActivable in listIActivation) {
			iActivable.desactivate ();
		}
		yield return null;
	}
}
