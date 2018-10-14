using UnityEngine;
using System.Collections;

public class CASetRigidBodyKinematicProperty : CustomActionScript {

	public Rigidbody _targetRigidbody = null;

	public bool _changeToKinematic = false;

	
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (_targetRigidbody != null)
			Debug.DrawLine (this.transform.position, _targetRigidbody.transform.position, Color.blue);
	}

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		yield return new WaitForFixedUpdate();
		_targetRigidbody.isKinematic = _changeToKinematic;
		if (!_changeToKinematic)
			_targetRigidbody.WakeUp();

		yield return null;
	}
}
