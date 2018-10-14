using UnityEngine;
using System.Collections;

public class CADesactivateObject : CustomActionScript {
		
	public GameObject objectAvecIActivable;
	
	public IActivable scriptActivable;

	public override void Start(){
		base.Start ();
		scriptActivable = (IActivable) objectAvecIActivable.GetComponent(typeof(IActivable));
	}
	
	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if(scriptActivable.getIsActif()){
			scriptActivable.desactivate();
		}
		yield return null;
	}
}
