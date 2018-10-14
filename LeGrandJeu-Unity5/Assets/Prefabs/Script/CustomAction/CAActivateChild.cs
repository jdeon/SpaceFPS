using UnityEngine;
using System.Collections;

public class CAActivateChild : CustomActionScript {

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		for (int numChild = 0; numChild < transform.childCount; numChild ++) {
			Transform child = transform.GetChild(numChild);
			if( null != child.GetComponent(typeof(IActivable))){
				IActivable activableScript = (IActivable) child.GetComponent(typeof(IActivable));
				if(!activableScript.getIsActif()){
					activableScript.activate();
				}
			}
		}
		yield return null;
	}
}