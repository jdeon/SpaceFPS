using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CACursorView : CustomActionScript {

	public bool isActivation;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args){
		CursorCustom.Activate = isActivation;
		yield return null;
	}
}
