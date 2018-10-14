using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CASpawnedMissile : CAPrefabSpawner {

	public ConditionEventAbstract condition;
	public GameObject missileTarget;
	public float force;
	public Material traceMissileMaterial;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		if(condition.getIsActive()){
		if (_prefab != null && null != _prefab.GetComponent<TeteChercheuseInertie> ()) {
			var newGO = (GameObject)GameObject.Instantiate (_prefab, 
				            _relativePosition && _relativeTransform != null ? 
				_position + _relativeTransform.position :
				_position, 
				            _relativeRotation && _relativeTransform != null ?
				_relativeTransform.rotation * _rotation :
				_rotation
			            );
			newGO.transform.parent = _makeRelativeTransformParent ? _relativeTransform : null;
				TeteChercheuseInertie teteChercheuse = newGO.GetComponent<TeteChercheuseInertie> ();

				if (null != teteChercheuse) {
					teteChercheuse.targetTransform = missileTarget.transform;
					teteChercheuse.force = this.force;

					ConditionEventInView conditionMissile = newGO.AddComponent<ConditionEventInView> ();
					conditionMissile.distanceInfini = true;
					conditionMissile.origin = newGO.transform;
					conditionMissile.target = missileTarget.transform;

					teteChercheuse.conditonRecher = conditionMissile;
				}

				if(null != traceMissileMaterial){
					TrailRenderer trail = newGO.AddComponent<TrailRenderer> ();
					trail.time = .5f;
					trail.startWidth = .01f;
					trail.endWidth = .1f;
					trail.startColor = Color.white;
					trail.endColor = new Color (.25f, .25f, .25f, .5f);
					trail.autodestruct = true;
					trail.sharedMaterial = traceMissileMaterial;
				}

			} else {
				base.DoActionOnEvent (sender, args);
			}
		}
		yield return null;
	}
}
