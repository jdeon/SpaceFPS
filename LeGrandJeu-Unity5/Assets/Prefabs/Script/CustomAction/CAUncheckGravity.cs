using UnityEngine;
using System.Collections;

public class CAUncheckGravity : CustomActionScript {

	public Rigidbody _targetRigidbody;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if (_targetRigidbody != null)
		{
			_targetRigidbody.useGravity = false;
			_targetRigidbody.WakeUp();
		}
		yield return null;
	}
}
