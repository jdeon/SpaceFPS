using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TableauDeCommandeZoneActivateur : MonoBehaviour {

	public string[] listTypeObjetAActiver;
	public int maxActivationSimultanee;

	private int nbToucheActive;
	private List<Transform> listTransformBoutonActivable;
	private Transform[] tabTranformObjetLie;
	private List<Transform> listObjetDejaActive;

	// Use this for initialization
	void Start () {
		nbToucheActive = 0;
		listTransformBoutonActivable = chercherBoutonActivable();
		tabTranformObjetLie = new Transform[maxActivationSimultanee];
		listObjetDejaActive = new List<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		List<Transform> listTransformObjet = new List<Transform>();
		int nbObjetActivee = 0;
		
		if (other.transform.Find ("ObjetAPorter/MainGauche") != null && other.transform.Find ("ObjetAPorter/MainGauche").childCount != 0) {
			Transform conteneurMainG = other.transform.Find ("ObjetAPorter/MainGauche");
			for (int numChild = 0 ; numChild < conteneurMainG.childCount ; numChild++){
				ObjetPortable objetPortable  = (ObjetPortable)conteneurMainG.GetChild(numChild).GetComponent<ObjetPortable> ();
				if (null != objetPortable && objetPortable.isSameTypeObjectDetector(listTypeObjetAActiver)){
					listTransformObjet.Add(conteneurMainG.GetChild(numChild).transform);
				}
			}
		} 

		if (other.transform.Find ("ObjetAPorter/MainDroite") != null && other.transform.Find ("ObjetAPorter/MainDroite").childCount != 0) {
			Transform conteneurMainD = other.transform.Find ("ObjetAPorter/MainDroite");
			for (int numChild = 0 ; numChild < conteneurMainD.childCount ; numChild++){
				ObjetPortable objetPortable  = (ObjetPortable)conteneurMainD.GetChild(numChild).GetComponent<ObjetPortable> ();
				if (null != objetPortable && objetPortable.isSameTypeObjectDetector(listTypeObjetAActiver)){
					listTransformObjet.Add(conteneurMainD.GetChild(numChild).transform);
				}
			}
		}


		if (listTransformObjet.Count > 0){
			nbObjetActivee = activerObjetPorter(listTransformObjet, maxActivationSimultanee - nbToucheActive);
		}

		if (nbObjetActivee > 0) {
			activerToucheCommande(nbObjetActivee);
		}
	}

	/**Cherche tous les petits enfant avec le script "ObjetActivable"
	 * */
	private List<Transform> chercherBoutonActivable(){
		List<Transform> listBoutonAvecScript = new List<Transform> ();
		for (int numEnfant = 0; numEnfant < transform.childCount; numEnfant++) {
			for (int numPetitEnfant = 0; numPetitEnfant< transform.GetChild(numEnfant).childCount; numPetitEnfant++){
				if (transform.GetChild(numEnfant).GetChild(numPetitEnfant).GetComponent<ObjetActivable>() != null){
					listBoutonAvecScript.Add(transform.GetChild(numEnfant).GetChild(numPetitEnfant));
				}
			}
		}

		return listBoutonAvecScript;
	}




	/**Modifie l'état des objet modifiable
	 * Retourne le nombre d'objet activer
	 * */
	private int activerObjetPorter (List<Transform> listTransformObjetPorter){
		int numObjetModifie = 0;

		foreach(Transform objetPorte in listTransformObjetPorter){
			bool isObjetModifier = false;

			if(!listObjetDejaActive.Contains(objetPorte)){
				isObjetModifier = objetPorte.GetComponent<ObjetPortable>().changerEtat();

				if(isObjetModifier){
					numObjetModifie++;
					listObjetDejaActive.Add(objetPorte);
				}
			}
		}
		return numObjetModifie;
	}

	/**Modifie l'état des objet modifiable, et les rajoute a tabTranformObjetLie
	 * Retourne le nombre d'objet activer
	 * */
	private int activerObjetPorter (List<Transform> listTransformObjetPorter, int maxActivation){
		int numObjetModifie = 0;

		if (maxActivation > 0) {
			foreach (Transform objetPorte in listTransformObjetPorter) {
				bool isObjetModifie = false;

				if(!listObjetDejaActive.Contains(objetPorte)){
					isObjetModifie = objetPorte.GetComponent<ObjetPortable> ().changerEtat ();
			
					if (isObjetModifie) {
						this.tabTranformObjetLie[numObjetModifie+ this.nbToucheActive] = objetPorte;
						numObjetModifie++;
						listObjetDejaActive.Add(objetPorte);
					}

					if (numObjetModifie >= maxActivation) {
						break;
					}
				}
			}
		}
		return numObjetModifie;
	}	
	
	private void activerToucheCommande(int nbToucheAActiver){
		int toucheActiveAvantTraitement = this.nbToucheActive;

		for (int numTouche = toucheActiveAvantTraitement; numTouche < toucheActiveAvantTraitement + nbToucheAActiver; numTouche++) {
			Transform transformTouche = this.listTransformBoutonActivable[numTouche];
			transformTouche.GetComponent<ObjetActivable>().activate();
			this.nbToucheActive++;
		}
	}

	public Transform[] getTabTranformObjetLie(){
		return this.tabTranformObjetLie;
	}
}
