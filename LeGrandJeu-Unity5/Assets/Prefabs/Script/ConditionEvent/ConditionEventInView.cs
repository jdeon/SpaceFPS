using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionEventInView : ConditionEventAbstract {

	public bool distanceInfini;
	public int distanceMax;

	public Transform origin;
	public Transform target;
	
	// Update is called once per frame
	void Update () {
		Vector3 coordOrigin = origin.position;
		Vector3 coordTarget = target.position;
		if (distanceInfini || Vector3.Distance (coordOrigin, coordTarget) <= distanceMax) {
			int nbObstacleMax = 10;
			RaycastHit hit = new RaycastHit ();
			bool isTrigger = true;
			while (isTrigger) {
				isTrigger = false;
				hit = new RaycastHit ();
				if (Physics.Linecast (coordOrigin, coordTarget, out hit)) {
					if (null == hit.collider) {
						desactiveEvent ();
						return;
					}
				//On touche directement le controller
				else if (hit.collider.gameObject == target.gameObject) {
						activeEvent ();
						return;
					}
				//On touche un collider Trigger donc on reocommence a partir du point de colision
					else if ((hit.collider.isTrigger || hit.collider.tag == "GameController") && nbObstacleMax >= 0) {
						isTrigger = true;
						Vector3 vectorToTarget = coordTarget - coordOrigin;
						vectorToTarget.Normalize ();
						coordOrigin = hit.point + (.001f * vectorToTarget); //Plus permet d'éviter de toujours retombé sur le meme collider
						nbObstacleMax--;
					}
				}
			}
		}
		desactiveEvent();
	}

	public override void onChange (){
	}
}
