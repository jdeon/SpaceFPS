using UnityEngine;
using System.Collections;

public class CATeleport : CustomActionScript {

	public Transform _teleportTransform;

	public bool _applyRotation = true;

	public bool _resetVelocityOfRigidBodies = false;
	
	public bool _resetAngularVelocityOfRigidBodies = false;

	public bool _immediate = false;

	public GameObject _targetGameObject = null;

	public override void OnDrawGizmos() 
	{
		base.OnDrawGizmos();
		if (_teleportTransform != null)
			Debug.DrawLine (this.transform.position, _teleportTransform.position, Color.blue);
	}

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if (!_immediate)
			yield return new WaitForFixedUpdate();
		_targetGameObject.transform.position = _teleportTransform.position;
		if (_applyRotation)
			_targetGameObject.transform.rotation = _teleportTransform.rotation;
		var rb = _targetGameObject.GetComponent<Rigidbody>();

		if (rb != null && !rb.isKinematic && _resetVelocityOfRigidBodies)
			rb.velocity = Vector3.zero;
		
		if (rb != null && !rb.isKinematic && _resetAngularVelocityOfRigidBodies)
			rb.angularVelocity = Vector3.zero;

		yield return null;
	}

}
