using UnityEngine;
using System.Collections;

public class CADestroyGameObject : CustomActionScript {

	public GameObject _go;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		yield return new WaitForFixedUpdate();
		Destroy(_go);
		yield return null;
	}
}
