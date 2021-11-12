using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGraviteModifier : MonoBehaviour {

	private static readonly float TIME_PER_PROCESSING = 0.05f;

	public Transform rotateTransform;
	public float tempDeRotation;
	public bool desactivateGravite;
	public bool isReinitialisate;

	private List<Rigidbody> listRigidBodyInterieurZone;
	private List<Transform> listTransformReference;

	private bool rotationEnCours;


	// Use this for initialization
	void Start () {
		listRigidBodyInterieurZone = new List<Rigidbody> ();
		listTransformReference = new List<Transform> ();
		rotationEnCours = false;
	}

	void Update(){
		//on supprime le transform de référence vide
		List<Transform> listTransformReferenceASupprimer = new List<Transform> ();

		foreach (Transform transformReference in listTransformReference) {
			if (transformReference.childCount == 0) {
				listTransformReferenceASupprimer.Add (transformReference);
			}
		}

		foreach (Transform TransformReferenceASupprimer in listTransformReferenceASupprimer) {
			listTransformReference.Remove (TransformReferenceASupprimer);
			GameObject.Destroy (TransformReferenceASupprimer.gameObject);
		}
	}


	void OnTriggerEnter(Collider other) {
		Rigidbody rigidOther = other.gameObject.GetComponent<Rigidbody> ();
		if (null != rigidOther) {
			listRigidBodyInterieurZone.Add (rigidOther);
			if (desactivateGravite) {
				rigidOther.useGravity = false;
			}

			//Le but est de coincider les direction de l'objet et de l'other
			float deltaAngle;
			Vector3 pivotRotation;
			float timeToRotate = this.tempDeRotation;
			bool isOppose = Vector3.Dot (other.transform.up, transform.up) < -0.25f;


			if (isOppose) {
				//opposingWait = true;
				//Si les deux vecteur haut sont opposé, on fait la rotation avec l'opposé et on tourne de 180 dans le sens de la direction
				pivotRotation = Vector3.Cross(other.transform.up, -1f * transform.up);
				deltaAngle = Vector3.Angle(-1f * transform.up, other.transform.up);

				if (!Vector3.zero.Equals (pivotRotation)) {
					//Deux rotation a prévoir;
					timeToRotate = this.tempDeRotation / 2;
				}

				StartCoroutine(rotateToWall(rigidOther,other.transform.forward, 180f, timeToRotate, Space.World));

			} else {
				pivotRotation = Vector3.Cross(other.transform.up, transform.up);
				deltaAngle = Vector3.Angle(transform.up, other.transform.up);
			}


			StartCoroutine(rotateToWall(rigidOther, pivotRotation, deltaAngle, timeToRotate, Space.World));
			if (!isReinitialisate) {
				StartCoroutine (applyNewGravity (rigidOther));
			}
		}
	}

	void OnTriggerExit(Collider other) {
		Rigidbody rigidOther = other.gameObject.GetComponent<Rigidbody> ();
		if (null != rigidOther && listRigidBodyInterieurZone.Contains (rigidOther)) {
			listRigidBodyInterieurZone.Remove (rigidOther);
			rigidOther.useGravity = true;
		}
	}

	private IEnumerator rotateToWall(Rigidbody rigidOther, Vector3 pivot, float deltaAngle, float timeToRotate, Space spaceRef) {
		float angleRestant = deltaAngle;

		while (this.rotationEnCours) {
			yield return null;
		}

		this.rotationEnCours = true;

		do{
			float deltaFrameAngle = deltaAngle*Time.deltaTime/timeToRotate;
			deltaFrameAngle = Mathf.Min(angleRestant,deltaFrameAngle); //Au cas où le deltaTime est trop grand
			rigidOther.transform.Rotate (pivot, deltaFrameAngle, spaceRef);
			angleRestant -= deltaFrameAngle;
			yield return null; // return here next frame
		} while (angleRestant > 0 && null != rigidOther && listRigidBodyInterieurZone.Contains (rigidOther));

		this.rotationEnCours = false;
	}

	private IEnumerator applyNewGravity(Rigidbody rigidOther){
		while (null != rigidOther && listRigidBodyInterieurZone.Contains (rigidOther)) {
			if (desactivateGravite) {
				rigidOther.useGravity = false;
			}
			rigidOther.AddForce (-10f * rotateTransform.up.normalized * Time.deltaTime, ForceMode.VelocityChange);
			yield return null; // return here next frame
		}
	}
}
