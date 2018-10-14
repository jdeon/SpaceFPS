using UnityEngine;
using System.Collections;

public class CAApplyForce : CustomActionScript {

	public ForceMode _forceMode = ForceMode.VelocityChange;

	public float _force = 30f;

	public Transform _transformReference;

	public Rigidbody _targetRigidbody = null;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		var bumpedRigidbody = _targetRigidbody;
		if (bumpedRigidbody != null && !bumpedRigidbody.isKinematic) 
		{
			yield return new WaitForFixedUpdate ();
			bumpedRigidbody.AddForce (_transformReference.rotation * Vector3.forward * _force, _forceMode);
		}
		yield return null;
	}
}
