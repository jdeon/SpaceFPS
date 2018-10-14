using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjetClignotant : MonoBehaviour, IActivable {

	public Material[] listCouleurClignotant;
	public float delaiEntreClignotant;

	private bool isActif;
	private bool isFini;


	// Use this for initialization
	void Start () {
		isActif = false;
		isFini = false;
		if (listCouleurClignotant.Length > 0) {
			StartCoroutine(clignotement());
		}
	}

	private IEnumerator clignotement(){
		float tempsAvantChangement = delaiEntreClignotant;
		int indexCouleurActuel = 0;
		while(!this.isFini){
			if(this.isActif){
				tempsAvantChangement -= Time.deltaTime;
				if (tempsAvantChangement < 0){
					indexCouleurActuel++;
					if (indexCouleurActuel < listCouleurClignotant.Length){
						transform.GetComponent<Renderer>().sharedMaterial = listCouleurClignotant[indexCouleurActuel];
					} else {
						indexCouleurActuel= 0;
						transform.GetComponent<Renderer>().sharedMaterial = listCouleurClignotant[indexCouleurActuel];
					}
					tempsAvantChangement = this.delaiEntreClignotant;
				}
			}
			yield return null;
		}
		yield return null;
	}



	// Update is called once per frame
	void Update () {
	
	}

	public void activate (){
		this.isActif = true;
	}
	
	public void desactivate(){
		this.isActif = false;
	}
	
	public bool getIsActif(){
		return this.isActif;
	}

	public void setIsFini(bool isFini){
		this.isFini = isFini;
	}
}
