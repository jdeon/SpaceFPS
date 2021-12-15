using UnityEngine;
using System.Collections;

public class CATeleport : CustomActionScript {

	public Transform _teleportTransform;

	public bool _applyRotation = true;

	public bool _resetVelocityOfRigidBodies = false;

	public VelocityType velocity;
	
	public bool _resetAngularVelocityOfRigidBodies = false;

	public bool _immediate = false;

	public GameObject _targetGameObject = null;

	public enum VelocityType {
		IGNORE_TYPE,
		KEEP_OLD,
		RESET,
		TAKE_NEW
	}

	public override void OnDrawGizmos() 
	{
		base.OnDrawGizmos();
		if (_teleportTransform != null)
			Debug.DrawLine (this.transform.position, _teleportTransform.position, Color.blue);
	}

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if(null == _targetGameObject || null == _teleportTransform)
        {
			yield break;
        }

		if (!_immediate) {
			yield return new WaitForFixedUpdate ();
		}

		_targetGameObject.transform.position = _teleportTransform.position;

		if (_applyRotation) {
			_targetGameObject.transform.rotation = _teleportTransform.rotation;
		}

		var rb = _targetGameObject.GetComponent<Rigidbody>();

		if (null != rb && !rb.isKinematic) {
			if (VelocityType.RESET == velocity || (VelocityType.IGNORE_TYPE == velocity && _resetVelocityOfRigidBodies)) {
				rb.velocity = Vector3.zero;
			} else if (VelocityType.TAKE_NEW == velocity) {
				var targetRb = _teleportTransform.GetComponent<Rigidbody> ();
				if(null != targetRb){
					rb.velocity = targetRb.velocity;
				}
			}
		
			if (_resetAngularVelocityOfRigidBodies) {
				rb.angularVelocity = Vector3.zero;
			}
		}

		yield return null;
	}

}
