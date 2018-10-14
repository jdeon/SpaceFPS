using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGraviteModifier : MonoBehaviour {

	public Transform rotateTransform;
	public float tempDeRotation;
	public bool desactivateGravite;
	public bool isReinitialisate;

	private List<Rigidbody> listRigidBodyInterieurZone;
	private List<Transform> listTransformReference;


	// Use this for initialization
	void Start () {
		listRigidBodyInterieurZone = new List<Rigidbody> ();
		listTransformReference = new List<Transform> ();
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

			//Simplifier avec la création d'un parent
			//Quaternion destinationRotation = adaptationDirectionVue (rigidOther.transform);
			float angleYDestination;
			if (null != transform.parent){
				angleYDestination = other.transform.localRotation.eulerAngles.y;
			} else {//SI pas de parent on utilisera la direction de deplacement comme référence
				angleYDestination = Vector3.Angle(other.transform.forward,new Vector3(rigidOther.velocity.x,0,rigidOther.velocity.z));
			}

			other.transform.SetParent (null);
			other.transform.localScale = Vector3.one;

			if (!isReinitialisate) {
				GameObject goReference = new GameObject ("ReferenceRotation_" + this.gameObject.name);
				goReference.transform.rotation = rotateTransform.rotation;
				listTransformReference.Add (goReference.transform);
				other.transform.SetParent (goReference.transform);
			} 

			StartCoroutine(rotateToWall(rigidOther, angleYDestination));
			StartCoroutine(applyNewGravity(rigidOther));
		}
	}

	void OnTriggerExit(Collider other) {
		Rigidbody rigidOther = other.gameObject.GetComponent<Rigidbody> ();
		if (null != rigidOther && listRigidBodyInterieurZone.Contains (rigidOther)) {
			listRigidBodyInterieurZone.Remove (rigidOther);
			if (desactivateGravite) {
				rigidOther.useGravity = true;
			}
		}
	}

	private Quaternion adaptationDirectionVue(Transform transformToModified){
		Vector3 otherEuler = transformToModified.localRotation.eulerAngles;
		//Vector3 transformModifiedEuler = rotateTransform.localRotation.eulerAngles;

		GameObject go = new GameObject ("adaptationDirectionVue");
		go.transform.SetParent (rotateTransform.parent);
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.Euler(0 , otherEuler.y +90 , 0);

		Quaternion rotationTarget = go.transform.rotation;
		GameObject.Destroy (go);
		return	rotationTarget;
	}

	private IEnumerator rotateToWall(Rigidbody rigidOther, float angleDst) {
		Quaternion orgRot = rigidOther.transform.localRotation;
		Quaternion dstRot = Quaternion.Euler (0, angleDst, 0);
		for (float t = 0.0f; t < tempDeRotation; t += Time.deltaTime){
			if (null != rigidOther && listRigidBodyInterieurZone.Contains (rigidOther)) {
				rigidOther.transform.localRotation = Quaternion.Slerp(orgRot, dstRot, t/tempDeRotation);

				//UtilsObjet.setWorldScale (rigidOther.transform, Vector3.one);
				yield return null; // return here next frame
			}
		}

		rigidOther.transform.localRotation = dstRot;
	}

	private IEnumerator applyNewGravity(Rigidbody rigidOther){
		while (null != rigidOther && listRigidBodyInterieurZone.Contains (rigidOther)) {
			rigidOther.AddForce (-10f * rotateTransform.up.normalized * Time.deltaTime, ForceMode.VelocityChange);
			yield return null; // return here next frame
		}
	}
}
