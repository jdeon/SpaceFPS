using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CASetActiveAllChild : CustomActionScript {

	public float delayBetweenEachChild;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{

		for (int index = 0; index < transform.childCount; index++) {
			Transform child = transform.GetChild (index);
			if (!child.gameObject.activeSelf) {
				child.gameObject.SetActive (true);

				if (delayBetweenEachChild > 0) {
					yield return new WaitForSeconds (delayBetweenEachChild);
				}
			}
		}
		yield return null;
	}
}
