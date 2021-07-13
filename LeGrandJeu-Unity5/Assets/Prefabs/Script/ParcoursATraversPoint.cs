using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcoursATraversPoint : MonoBehaviour {

	public bool toTargetIfVisible;
	public Transform targetTransform;
	public List<Vector3> listPointTrajet;

	public float speed;
	public float distanceToClosePoint;

	public bool figerX = false;
	public bool figerY = false;
	public bool figerZ = false;


	private LayerMask layerRayCastDefault;
	private LayerMask layerRayCastController;
	private LayerMask layerRayCastDetectZone;

	private Vector3 nextPoint;
	private Rigidbody thisRb;

	private Vector3 positionIntial;


	// Use this for initialization
	void Start () {
		layerRayCastDefault = Constantes.LAYER_DEFAULT;
		layerRayCastController = Constantes.LAYER_CONTROLLER;
		layerRayCastDetectZone = Constantes.LAYER_DETECT_ZONE;
		thisRb = GetComponent<Rigidbody> ();
		positionIntial = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		if (toTargetIfVisible && isTargetAtteignable(transform.position, targetTransform.position))
		{
			goToDirection (targetTransform.position);
		} else if (getNextPoint()) {
			goToDirection (nextPoint);

		} else if (isTargetAtteignable(transform.position, targetTransform.position)) {
			goToDirection (targetTransform.position);
		} 
	}

	public bool isTargetAtteignable(Vector3 coordOrigin, Vector3 coordTarget){
		int nbObstacleMax = 10;
		RaycastHit hit = new RaycastHit();
		bool isTrigger = true;
		while (isTrigger){
			isTrigger =false;
			hit = new RaycastHit();
			if (Physics.Linecast (coordOrigin, coordTarget, out hit)) {
				if (null == hit.collider || (!hit.collider.isTrigger && hit.collider.gameObject.layer == layerRayCastDefault.value)) {
					return false;
				}
				//On touche directement le controller
				else if (hit.collider.gameObject.layer == layerRayCastController.value) {
					return true;
				}
				//On touche un collider Trigger donc on reocommence a partir du point de colision
				else if ((hit.collider.isTrigger || hit.collider.gameObject.layer == layerRayCastDetectZone.value) && nbObstacleMax >= 0) {
					isTrigger = true;
					Vector3 vectorToTarget = coordTarget - coordOrigin;
					vectorToTarget.Normalize ();
					coordOrigin = hit.point + (.001f * vectorToTarget); //Plus permet d'éviter de toujours retombé sur le meme collider
					nbObstacleMax--;
				}
			} else {
				return true;
			}
		}
		return false;
	}

	private bool getNextPoint(){
		bool trouver = false;
		float distanceMax = Vector3.Distance (transform.position, targetTransform.position);
		nextPoint = targetTransform.position;

		foreach (Vector3 poisitionPoint in listPointTrajet) {
			float distanceToPoint = Vector3.Distance (transform.position, poisitionPoint);
			//Point plus proche de cible + Supérieur distance min + Supérieur+plus proche que le dernier point select  + atteignable
			if (Vector3.Distance (poisitionPoint, targetTransform.position) < distanceMax && distanceToPoint > distanceToClosePoint
			    && distanceToPoint < Vector3.Distance (transform.position, nextPoint) && isTargetAtteignable (transform.position, poisitionPoint)) {
				nextPoint = poisitionPoint;
				trouver = true;
			}
		}
		return trouver;
	}

	private void goToDirection(Vector3 targetPoint){
		float targetX = figerX ? positionIntial.x : targetTransform.position.x;
		float targetY = figerY ? positionIntial.y : targetTransform.position.y;
		float targetZ = figerZ ? positionIntial.z : targetTransform.position.z;

		Vector3 directionPoint = new Vector3 (targetX, targetY, targetZ);
		transform.LookAt (directionPoint);

		if (null != thisRb) {
			thisRb.velocity = (directionPoint - transform.position).normalized * speed;
		} else {
			transform.Translate (0, 0, speed * Time.deltaTime);	
		}

	}
}
