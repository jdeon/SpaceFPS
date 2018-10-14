using UnityEngine;
using System.Collections;

public class ObjetActivable : MonoBehaviour, IActivable {

	public Material[] listeMateriaux;

	private bool actif;
	private int nivActivation;

	// Use this for initialization
	void Start () {
		actif = false;
		if (listeMateriaux.Length > 0) {
			GetComponent<Renderer>().material = listeMateriaux [0];
		}
		nivActivation = 0;
	}
	
	public void activate(){
		if (!actif && listeMateriaux.Length > 1) {
			GetComponent<Renderer>().material = listeMateriaux [1];
			actif = true;
			nivActivation = 1;
		} else if (listeMateriaux.Length > nivActivation + 1) {
			nivActivation++;
			GetComponent<Renderer>().material = listeMateriaux [nivActivation];
		}
	}

	public void desactivate(){
		if (actif && listeMateriaux.Length > 1) {
			GetComponent<Renderer>().material = listeMateriaux [0];
			actif = false;
			nivActivation=0;
		}
	}

	public void onChange (){
	}

	public bool getIsActif(){
		return actif;
	}

	public int getNivActivation(){
		return this.nivActivation;
	}
}
