using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CADestroyChild : CustomActionScript {

	public Transform parent;
	public float delaiEntreEnfant;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if (delaiEntreEnfant > 0) {
			while (parent.childCount > 0) {
				GameObject.Destroy (parent.GetChild (0).gameObject);
				yield return new WaitForSeconds (delaiEntreEnfant);
			}
				

		} else {
			for (int index = 0; index < parent.childCount; index++) {
				GameObject.Destroy (parent.GetChild (index).gameObject);
			}
		}
		yield return null;
	}
}
