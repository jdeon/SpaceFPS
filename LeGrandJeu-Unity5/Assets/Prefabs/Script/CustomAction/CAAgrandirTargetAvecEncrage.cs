using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAAgrandirTargetAvecEncrage : CustomActionScript {

	//Si négatif, autre sensZ
	public Vector3 tailleFinal;

	public float duree;


	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)	{

		Vector3 difference = Vector3.zero;
		//Tab de 1 direction positive ou -1 direction negativee
		int[] direction = new int[3];

		for (int i = 0; i < 3; i++) {
			if (tailleFinal [i] < 0) {
				direction [i] = -1;
			} else {
				direction [i] = 1;
			}
			difference [i] = ((float) direction[i]) * tailleFinal [i] - transform.localScale [i];
		}

		float tempsRestant = duree;
		while(tempsRestant>0){
			Vector3 rajoutFrame = difference * Time.deltaTime /  duree;
			transform.localScale += rajoutFrame;
			Vector3 mouvement = Vector3.zero;

			for (int i = 0 ; i < 3 ; i++){
				mouvement[i]  = direction [i] * rajoutFrame [i] / 2;
			}

			transform.Translate (transform.right * mouvement [0] + transform.up * mouvement [1] + transform.forward * mouvement [2],Space.World);


			tempsRestant -= Time.deltaTime ;
			yield return null;
			}
		yield return null;
	}
}