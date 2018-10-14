using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CADestroyObjectParRetrecissement : CustomActionScript {

	public GameObject target;

	public float tempsDeChargement;


	protected GameObject instanciateObject;

	protected float proportionDeBase = .001f;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)	{
		if (target != null)
		{
			float tempsRestant = tempsDeChargement;
			while (tempsRestant < 0 ) {
				target.transform.localScale = target.transform.localScale * (tempsRestant / this.tempsDeChargement);
				tempsRestant -= Time.deltaTime;
				yield return null;
			}
			Destroy (target);
		}
		yield return null;
	}
}
