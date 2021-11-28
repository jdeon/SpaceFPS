using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParcoursATraversZone : MonoBehaviour {

	public bool toTargetIfVisible;
	public Transform targetTransform;
	public List<Collider> listZoneTrajet;

	public float speed;
	public float distanceForChangeZone;

	public bool figerX = false;
	public bool figerY = false;
	public bool figerZ = false;



	private int numEtape = 0;
	private LayerMask layerRayCastDefault;
	private LayerMask layerRayCastController;
	private LayerMask layerRayCastDetectZone;
	private LayerMask layerRayCastTargetZone;


	// Use this for initialization
	void Start () {
		layerRayCastDefault = Constantes.LAYER_DEFAULT;
		layerRayCastController = Constantes.LAYER_CONTROLLER;
		layerRayCastDetectZone = Constantes.LAYER_DETECT_ZONE;
		layerRayCastTargetZone = Constantes.LAYER_TARGET_ZONE;
	}
	
	// Update is called once per frame
	void Update () {
		if (toTargetIfVisible && isTargetAtteignable(transform.position, targetTransform.position))
 {
			float targetX = figerX ? transform.position.x : targetTransform.position.x;
			float targetY = figerY ? transform.position.y : targetTransform.position.y;
			float targetZ = figerZ ? transform.position.z : targetTransform.position.z;

			transform.LookAt (new Vector3 (targetX, targetY, targetZ));
			transform.Translate (0, 0, speed * Time.deltaTime);	
		} else if (numEtape < listZoneTrajet.Count) {
			Collider collEtapeSuivante = listZoneTrajet [numEtape];
			Vector3 closestPoint = collEtapeSuivante.ClosestPointOnBounds (transform.position);
			float targetX = figerX ? transform.position.x : closestPoint.x;
			float targetY = figerY ? transform.position.y : closestPoint.y;
			float targetZ = figerZ ? transform.position.z : closestPoint.z;


			if (Vector3.Distance (closestPoint, transform.position) <= distanceForChangeZone) {
				numEtape++;
			} else {
				transform.LookAt (new Vector3 (targetX, targetY, targetZ));
				transform.Translate (0, 0, speed * Time.deltaTime);
			}
		} else if (isTargetAtteignable(transform.position, targetTransform.position)) {
			float targetX = figerX ? transform.position.x : targetTransform.position.x;
			float targetY = figerY ? transform.position.y : targetTransform.position.y;
			float targetZ = figerZ ? transform.position.z : targetTransform.position.z;
			
			transform.LookAt (new Vector3 (targetX, targetY, targetZ));
			transform.Translate (0, 0, speed * Time.deltaTime);	
		} 
	}

	public bool isTargetAtteignable(Vector3 coordOrigin, Vector3 coordTarget){
		return UtilsTargetable.isTargetAtteignable(coordOrigin, coordTarget, 10);
	}
}
