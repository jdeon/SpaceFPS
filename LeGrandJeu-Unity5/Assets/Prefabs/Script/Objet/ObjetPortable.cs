using UnityEngine;
using System.Collections;

public class ObjetPortable : MonoBehaviour {

	public int niveauMinPourDeposer = 0;
	public int niveauMaxPourDeposer = 100;
	public string[] typeOfObject;
	public float tempsPourAttraperDeposer = 2f;
	public Material[] listeVisuelEtat;
	
	private int numEtat;
	private Vector3 tailleIntial;
	// Use this for initialization
	void Start () {
		tailleIntial = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
		numEtat = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 getTailleInitial(){
		return this.tailleIntial;
	}

	public bool changerEtat(){
		if (numEtat + 1 < listeVisuelEtat.Length) {
			numEtat ++;
			GetComponent<Renderer>().material = listeVisuelEtat [numEtat];
			return true;
		}
		return false;
	}

	/**Change vers l'etat désirer
	 * */
	public bool changerEtat(int etatDesire){
		//si on est déja dans l'état modifier, on ne change rien
		if (numEtat == etatDesire) {
			return false;
		}

		numEtat = etatDesire;
		if (numEtat < listeVisuelEtat.Length) {
			GetComponent<Renderer>().material = listeVisuelEtat [numEtat];
			return true;
		}
		return false;
	}

	public int getNumEtat(){
		return this.numEtat;
	}

	public bool isDeposable(){
		if (this.numEtat >= this.niveauMinPourDeposer && this.numEtat <= this.niveauMaxPourDeposer) {
			return true;
		}
		return false;
	}

	/**Verifie que l'ObjetPortable de la transform a un type similaire à un type contenue dans la liste des type de detection du detecteur
	 * */
	public bool isSameTypeObjectDetector (string[] typesDetection){
		if  (this.typeOfObject.Length != 0) {
			for (int numTypeObject = 0; numTypeObject < this.typeOfObject.Length; numTypeObject++) {
				string typeOfObject = this.typeOfObject [numTypeObject];
				for (int numTypeDetection = 0; numTypeDetection < typesDetection.Length; numTypeDetection++) {
					if (typeOfObject == typesDetection [numTypeDetection]) {
						return true;
					}
				}
			}
		}
		return false;
	}
}
