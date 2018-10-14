using UnityEngine;
using System.Collections;

public class CAChangeUseGravity : CustomActionScript {

	public Rigidbody _targetRigidbody;
	public bool _setCheck = true;


	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if (_targetRigidbody != null)
		{
			_targetRigidbody.useGravity = _setCheck;
			_targetRigidbody.WakeUp();
		}
		yield return null;
	}
}
