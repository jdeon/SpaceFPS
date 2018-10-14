using UnityEngine;
using System.Collections;

public class CADisableEnableRender : CustomActionScript {

	public GameObject target;
	public bool setActive = true;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		target.GetComponent<Renderer>().enabled = setActive;
		yield return null;
	}
}
