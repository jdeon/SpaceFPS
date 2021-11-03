using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marteau : ObjetPortable {

	public string[] attackOrder;
	private int indexAttack;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && null != transform.parent && attackOrder.Length > 0){
			if (transform.parent.name == "MainDroite" || transform.parent.name == "MainGauche") {
				
				if (indexAttack >= attackOrder.Length) {
					indexAttack = 0;
				}
					
				frappe (attackOrder [indexAttack]);
				indexAttack++;
			}
		}
	}

	void frappe(string typeAttack){
		anim.enabled = true;
		anim.SetTrigger(typeAttack);
		StartCoroutine (desactivation ());
	}

	IEnumerator desactivation(){
		float timeTodesactivate = .5f;

		while (timeTodesactivate < 0) {
			timeTodesactivate -= Time.deltaTime;
			yield return null;
		}

		while (anim.enabled && !anim.GetCurrentAnimatorStateInfo (0).IsName ("Stable")) {
			anim.enabled = false;
			yield return null;
		}
	}

}
