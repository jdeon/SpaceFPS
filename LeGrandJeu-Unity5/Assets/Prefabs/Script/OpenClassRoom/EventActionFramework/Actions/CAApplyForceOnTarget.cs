using UnityEngine;
using System.Collections;

public class CAApplyForceOnTarget : CustomActionScript {

	public ForceMode _forceMode = ForceMode.VelocityChange;

	public float _force;

	public Transform _transformReference;

	public bool resetStartVelocity = true;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		var bumpedRigidbody = args.GetComponent<Rigidbody>();
		if (bumpedRigidbody != null && !bumpedRigidbody.isKinematic) 
		{
			if (resetStartVelocity) {
				bumpedRigidbody.velocity = Vector3.zero;
			}
			yield return new WaitForFixedUpdate ();
			bumpedRigidbody.AddForce (_transformReference.rotation * Vector3.forward * _force, _forceMode);
		}
		yield return null;
	}
}
