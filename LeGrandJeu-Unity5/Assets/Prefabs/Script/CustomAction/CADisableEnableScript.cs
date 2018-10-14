using UnityEngine;
using System.Collections;

public class CADisableEnableScript : CustomActionScript {

	public bool toEnable;
	public MonoBehaviour scriptConcerne;
	public string nomObject = "";
	public string nomScript = "";



	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if (scriptConcerne == null) {
			GameObject gameObjetConcerne = GameObject.Find(nomObject);
			((MonoBehaviour) gameObjetConcerne.GetComponent(nomScript)).enabled = toEnable;
		} else {
			scriptConcerne.enabled = toEnable;
		}
		yield return null;
	}
}
