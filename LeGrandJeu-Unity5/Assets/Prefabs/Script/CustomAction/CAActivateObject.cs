using UnityEngine;
using System.Collections;

public class CAActivateObject : CustomActionScript {

	public GameObject objectAvecIActivable;

	private IActivable scriptActivable;

    public override void Start(){
		base.Start ();
		scriptActivable = (IActivable) objectAvecIActivable.GetComponent(typeof(IActivable));
	}

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if(null != scriptActivable && !scriptActivable.getIsActif()){
			scriptActivable.activate();
		}
		yield return null;
	}
}
