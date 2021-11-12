using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CASpawnedEnnemieTeleportInZone : CASpawnedPrefabInZone {

	public float speed;
	public Transform target;
	public bool modeAuto;

	//défini si l'on peut passer dans tout le box collider ou uniquement le centre
	public bool useColliderForPassage;
	public List<GameObject> listPassage;
	
	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{	
		yield return StartCoroutine(base.DoActionOnEvent (sender, args));

		if (instanciateObject != null) {
			TeleportEnemyTueur teleportScript = instanciateObject.GetComponent<TeleportEnemyTueur> ();
			teleportScript.speed = speed;
			teleportScript.target = target;
			teleportScript.gererDeplacement = modeAuto;
			teleportScript.activate();
			if(!modeAuto && null != listPassage && listPassage.Count >0){
				if (useColliderForPassage) {
					List<Collider> listZonePassage = new List<Collider> ();

					foreach (GameObject passage in listPassage) {
						Collider zonePassage = passage.GetComponent<Collider> ();
						if (null != zonePassage) {
							listZonePassage.Add (zonePassage);
						}
					}
					ParcoursATraversZone parcourTraverZoneScript = instanciateObject.AddComponent<ParcoursATraversZone> ();
					parcourTraverZoneScript.toTargetIfVisible = true;
					parcourTraverZoneScript.targetTransform = target;
					parcourTraverZoneScript.listZoneTrajet = listZonePassage;
					parcourTraverZoneScript.speed = speed;
					parcourTraverZoneScript.distanceForChangeZone = .5f;
					parcourTraverZoneScript.figerY = true;
				} else {
					List<Vector3> listPointDePassage = new List<Vector3> ();

					foreach (GameObject passage in listPassage) {
						listPointDePassage.Add (passage.transform.position);
					}

					ParcoursATraversPoint parcourTraversPointScript = instanciateObject.AddComponent<ParcoursATraversPoint> ();
					parcourTraversPointScript.toTargetIfVisible = true;
					parcourTraversPointScript.targetTransform = target;
					parcourTraversPointScript.listPointTrajet = listPointDePassage;
					parcourTraversPointScript.speed = speed;
					parcourTraversPointScript.distanceToClosePoint = .5f;
					parcourTraversPointScript.figerY = true;
				}
			}
		}
		yield return null;
	}
}

