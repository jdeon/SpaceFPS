using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CASpawnedPrefabParElargissement : CustomActionScript {

	public Transform _relativeTransform = null;

	public bool _makeRelativeTransformParent = false;

	public bool _relativePosition = false;

	public bool _relativeRotation = false;

	public Vector3 _position;

	public Quaternion _rotation = Quaternion.identity;

	public GameObject _prefab;

	public float tempsDeChargement;


	protected GameObject instanciateObject;

	protected float proportionDeBase = .001f;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)	{
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

			newGO.transform.localScale = newGO.transform.localScale * proportionDeBase;
			newGO.transform.parent = _makeRelativeTransformParent ? _relativeTransform : null;
			instanciateObject = newGO;
			yield return null;

			float time = 0f;
			while (time > tempsDeChargement) {
				newGO.transform.localScale = newGO.transform.localScale * (time / this.tempsDeChargement);
				time += Time.deltaTime;
				yield return null;
			}
		}
		yield return null;
	}
}
