using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CATeleportTarget : CustomActionScript {

	public enum VelocityMode {resetVelocity, VelocityUnchange, ChangeDirectionWithTeleportTransform};

	public Transform _teleportTransform;

	public bool _applyRotation = true;

	public VelocityMode velocityMode = VelocityMode.VelocityUnchange;

	public bool _immediate = false;

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (_teleportTransform != null)
			Debug.DrawLine (this.transform.position, _teleportTransform.position, Color.blue);
	}

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		Vector3 forceSortie = Vector3.zero;

		if (!_immediate)
			yield return new WaitForFixedUpdate();
		args.transform.position = _teleportTransform.position;
		if (_applyRotation) {
			//Si c est le controlleur, seul l'axe y est modifier.
			if (args.layer == Constantes.LAYER_CONTROLLER) {
				Quaternion newRotation = args.transform.rotation;
				newRotation.eulerAngles = new Vector3(args.transform.rotation.eulerAngles.x, _teleportTransform.rotation.eulerAngles.y,args.transform.rotation.eulerAngles.z);
				args.transform.rotation = newRotation;
			} else {
				args.transform.rotation = _teleportTransform.rotation;
			}
		}
		var rb = args.GetComponent<Rigidbody>();

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

				operateurRotation.transform.forward = new Vector3 (_teleportTransform.forward.x, _teleportTransform.forward.y, _teleportTransform.forward.z);

				forceSortie = rb.velocity.magnitude * representationVecteur.transform.forward;
				rb.velocity = forceSortie;

				Destroy (representationVecteur);
				Destroy (operateurRotation);
			}
		}

		List <Collider> listColliderParentEtFrere = getListColliderParentEtFrere ();

		foreach(Collider col in listColliderParentEtFrere){
			col.enabled = false;
		}

		yield return new WaitForSeconds (.5f);

		foreach(Collider col in listColliderParentEtFrere){
			col.enabled = true;
		}
		yield return null;
	}

	private List <Collider> getListColliderParentEtFrere(){
		List <Collider> listColiderParentEtFrere = new List<Collider> ();
		Transform parentDestination = _teleportTransform.parent;

		if (null != parentDestination) {
			Collider colParent = parentDestination.GetComponent<Collider> ();
			if (null != colParent) {
				listColiderParentEtFrere.Add (colParent);
			}


			Collider[] tabColliderFrere = parentDestination.GetComponentsInChildren<Collider> ();
			for (int index = 0; index < tabColliderFrere.Length; index++) {
				listColiderParentEtFrere.Add (tabColliderFrere[index]);
			}
		}
		return listColiderParentEtFrere;
	}
}