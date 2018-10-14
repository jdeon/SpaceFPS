using UnityEngine;
using System.Collections;

public class CAPrefabSpawner : CustomActionScript {

	public Transform _relativeTransform = null;

	public bool _makeRelativeTransformParent = false;

	public bool _relativePosition = false;

	public Vector3 _position = Vector3.zero;

	public bool _relativeRotation = false;

	public Quaternion _rotation = Quaternion.identity;

	public GameObject _prefab;

	public bool setActifIfNot;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		yield return new WaitForFixedUpdate();
		if (_prefab != null)
		{
			var newGO = (GameObject) GameObject.Instantiate(_prefab, 
			                                                _relativePosition && _relativeTransform != null ? 
			                                                _position + _relativeTransform.position :
			                                                _position, 
			                                                _relativeRotation && _relativeTransform != null ?
			                                                _relativeTransform.rotation * _rotation :
			                                                _rotation
			                                                );
			newGO.transform.parent = _makeRelativeTransformParent ? _relativeTransform : null;

			if (setActifIfNot) {
				newGO.SetActive (true);
			}
		}
		yield return null;
	}
}
