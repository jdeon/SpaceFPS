using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporteurDoubleSens : MonoBehaviour {

	public enum VelocityMode {resetVelocity, VelocityUnchange, ChangeDirectionWithTeleportTransform};

	public TeleporteurDoubleSens telePortTarget;
	public bool drawLineToTarget;
	public bool _applyRotation = true;

	public VelocityMode velocityMode = VelocityMode.VelocityUnchange;

	private bool isZoneActive;


	// Use this for initialization
	void Start () {
		isZoneActive = true;
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
		if (isZoneActive) {
			telePortTarget.desactivate ();
			other.transform.position = telePortTarget.transform.position;

			if (_applyRotation) {
				//Si c est le controlleur, seul l'axe y est modifier.
				if (other.gameObject.layer == Constantes.LAYER_CONTROLLER) {
					Quaternion newRotation = other.transform.rotation;
					newRotation.eulerAngles = new Vector3(other.transform.rotation.eulerAngles.x, telePortTarget.transform.rotation.eulerAngles.y,other.transform.rotation.eulerAngles.z);
					other.transform.rotation = newRotation;
				} else {
					other.transform.rotation = telePortTarget.transform.rotation;
				}
			}

			Rigidbody rb = other.gameObject.GetComponent<Rigidbody> ();

			if (rb != null && !rb.isKinematic) {
				if (velocityMode.Equals (VelocityMode.resetVelocity)) {
					rb.velocity = Vector3.zero;
				} else if (velocityMode.Equals (VelocityMode.ChangeDirectionWithTeleportTransform)) {
					//Possible perte de performance pour juste tourner un vecteur
					GameObject operateurRotation = new GameObject("OperateurRotation");
					operateurRotation.transform.forward = new Vector3 (transform.forward.x, transform.forward.y, transform.forward.z);

					GameObject representationVecteur = new GameObject("RepresentationVecteur");
					representationVecteur.transform.SetParent (operateurRotation.transform);
					representationVecteur.transform.forward = new Vector3 (rb.velocity.x, rb.velocity.y, rb.velocity.z);

					operateurRotation.transform.forward = new Vector3 (telePortTarget.transform.forward.x, telePortTarget.transform.forward.y, telePortTarget.transform.forward.z);

					Vector3 forceSortie = rb.velocity.magnitude * representationVecteur.transform.forward;
					rb.velocity = forceSortie;

					Destroy (representationVecteur);
					Destroy (operateurRotation);
				}
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (!isZoneActive) {
			isZoneActive = true;
		}
	}

	void desactivate(){
		isZoneActive = false;
	}

	void OnDrawGizmosSelected() {
		if (telePortTarget != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, telePortTarget.transform.position);
		}
	}

	public void OnDrawGizmos(){
		if (telePortTarget != null && drawLineToTarget) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, telePortTarget.transform.position);
		}
	}
}
