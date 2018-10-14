using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPrefabParElargissementOnEnter : MonoBehaviour {

	public bool _makeRelativeTransformParent = false;

	public bool _relativeRotation = false;

	public Vector3 angles;

	public GameObject _prefab;

	public float tempsDeChargement;

	public Vector3 _positionRelative;


	protected GameObject instanciateObject;

	protected float proportionDeBase = .001f;



	// Use this for initialization
	void OnTriggerEnter(Collider col) {
		if (_prefab != null) {
			StartCoroutine (apparitionPrefab (col));
		}
	}

	private IEnumerator apparitionPrefab (Collider col) {
		Vector3 tailleFinal;
		Quaternion _rotation = Quaternion.identity;
		Vector3 positionAppariton = col.transform.position + this._positionRelative;
		_rotation.eulerAngles = angles;

		var newGO = (GameObject) GameObject.Instantiate(_prefab,positionAppariton, 
			this._relativeRotation ?	col.transform.rotation * _rotation : _rotation);

		newGO.transform.parent = this._makeRelativeTransformParent ? this.transform : null;
		tailleFinal = new Vector3 (newGO.transform.localScale.x, newGO.transform.localScale.y, newGO.transform.localScale.z);
		newGO.transform.localScale = newGO.transform.localScale * this.proportionDeBase;
		this.instanciateObject = newGO;
		yield return null;

		float time = 0f;
		while (time < tempsDeChargement) {
			newGO.transform.localScale = tailleFinal * (time / this.tempsDeChargement);
			time += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}
}
