using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marteau : ObjetPortable {

	public int multipliBy;

	public string[] attackOrder;
	private int indexAttack;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && null != transform.parent && attackOrder.Length > 0 && Time.timeScale != 0){
			if (transform.parent.name == "MainDroite" || transform.parent.name == "MainGauche") {
				
				if (indexAttack >= attackOrder.Length) {
					indexAttack = 0;
				}
					
				frappe (attackOrder [indexAttack]);
				indexAttack++;
			}
		}
	}

	void OnTriggerEnter(Collider other){
		Rigidbody rigidB = other.gameObject.GetComponent<Rigidbody>();
		if (null != rigidB && other.gameObject.tag == "TargetZone" && anim.enabled) {
			if (rigidB.isKinematic) {
				rigidB.isKinematic = false;
			}

			rigidB.AddForce (multipliBy * transform.right * -1, ForceMode.Impulse);
		}
	}

	void frappe(string typeAttack){
		anim.enabled = true;
		anim.SetTrigger(typeAttack);
		StartCoroutine (desactivation ());
	}

	IEnumerator desactivation(){
		float timeTodesactivate = .5f;

		//Mini delta pour attendre le changement de state
		while (timeTodesactivate > 0) {
			timeTodesactivate -= Time.deltaTime;
			yield return null;
		}

		//Desactiver quand retour au state stable
		while (anim.enabled && !anim.GetCurrentAnimatorStateInfo (0).IsName ("Stable")) {
			yield return null;
		}

		anim.enabled = false;
	}

}
