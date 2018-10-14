using UnityEngine;
using System.Collections;

public class CASpawnedPrefabInZone : CustomActionScript {
		
	public Transform _relativeTransform = null;
		
	public bool _makeRelativeTransformParent = false;
		
	public bool _relativePosition = false;
		
	public float borneMinX = 0f;
	public float borneMaxX = 0f;
	public float borneMinY = 0f;
	public float borneMaxY = 0f;
	public float borneMinZ = 0f;
	public float borneMaxZ = 0f;
		
	public bool _relativeRotation = false;
		
	public Quaternion _rotation = Quaternion.identity;
		
	public GameObject _prefab;

	protected GameObject instanciateObject;
		
	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)	{
		Vector3 _position = new Vector3(Random.Range(borneMinX, borneMaxX),Random.Range(borneMinY, borneMaxY),Random.Range(borneMinZ, borneMaxZ));

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
				instanciateObject = newGO;
			}
			yield return null;
		}
	}
