using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAChangeParent : CustomActionScript {

	public Transform parent;
	public Transform enfant;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args){
		enfant.SetParent (parent);
		yield return null;
	}
}
