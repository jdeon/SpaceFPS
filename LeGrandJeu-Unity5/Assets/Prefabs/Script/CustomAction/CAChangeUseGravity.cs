using UnityEngine;
using System.Collections;

public class CAChangeUseGravity : CustomActionScript {

	public Rigidbody _targetRigidbody;
	public bool _setCheck = true;


	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if (null == _targetRigidbody) {
			_targetRigidbody = gameObject.GetComponent<Rigidbody> ();
		}

		if (_targetRigidbody != null)
		{
			_targetRigidbody.useGravity = _setCheck;

			if (_setCheck) {
				//Si on active la gravité, on desactive aussi isKinematic
				_targetRigidbody.isKinematic = false;
			}

			_targetRigidbody.WakeUp();
		}
		yield return null;
	}
}
