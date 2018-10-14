using UnityEngine;
using System.Collections;

/**
 * Transferer dans /objet et renommer en compteur de vie
 * 
 * */
public class CompteurDeVieEnemie : MonoBehaviour {

	public enum FinVie {Disparition, Chute}
	public FinVie choixFinVie;
	public int pointVie;
	public GameObject effetVisuel;

	void Start (){
	}

	// Update is called once per frame
	void Update () {}

	void OnTriggerEnter(Collider other) {
		pointVie--;
		if (pointVie <= 0) {
			mortEnnemie();
		}
	}

	void mortEnnemie(){
		if (null != effetVisuel) {
			((GameObject) Instantiate(effetVisuel, transform.position, transform.rotation)).SetActive(true);

		}

		if (choixFinVie == FinVie.Disparition) {
			Destroy(transform.parent.gameObject, .25f);
		} else if (choixFinVie == FinVie.Chute) {
			transform.parent.GetComponent<Rigidbody>().useGravity =true;
		}
	}
}
