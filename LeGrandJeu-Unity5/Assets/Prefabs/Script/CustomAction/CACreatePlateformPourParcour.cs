using UnityEngine;
using System.Collections;

public class CACreatePlateformPourParcour : CustomActionScript {
		
	public Vector3 taille;
	public Parcours parcoursASuivre;
	public float vitesseRelative = 1f;
	public Vector3 decalageAuParcour;
	public bool rigidBodyAtBeggining;
	public bool detruireObjet;
	public float tempsAvantDestroy;

	
	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args){

		if (this.tempsAvantDestroy < 0) {
            this.detruireObjet = false;
		}
		GameObject emptyParent = new GameObject();
		emptyParent.transform.position = transform.position;
		emptyParent.transform.rotation = transform.rotation;
		emptyParent.transform.parent = transform;
		emptyParent.SetActive (false);

		if (rigidBodyAtBeggining) {
			Rigidbody rigidParent = emptyParent.AddComponent<Rigidbody> ();
			rigidParent.useGravity = false;
			rigidParent.freezeRotation = true;
			rigidParent.mass = 100f * this.taille.x * this.taille.y * this.taille.z;
		} 

        GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.rotation = transform.rotation;
		cube.transform.localScale = this.taille;
		cube.transform.parent = emptyParent.transform;
		cube.transform.localPosition = decalageAuParcour;
		cube.layer = Constantes.LAYER_DETECT_ZONE;

		//Si pas de rigidbody a la creation, on crée une zonz qui créera un rigidbody si le controller entre
		if (! rigidBodyAtBeggining) {
			BoxCollider detectBox = cube.AddComponent<BoxCollider> ();
			detectBox.size = new Vector3 (2, 2, 2);
			detectBox.isTrigger = true;
			AddRigidodyOnTrigger addRigidodyOnTrigger = cube.AddComponent<AddRigidodyOnTrigger> ();
			addRigidodyOnTrigger.setScriptParent (this);
			addRigidodyOnTrigger.setEmptyParentTarget (emptyParent);
		} 
			
        suivreParcour suivreP = emptyParent.AddComponent<suivreParcour> ();
		suivreP.parcours = this.parcoursASuivre;
		suivreP.vitesseRelative = this.vitesseRelative;
		suivreP.isDestroyAtEnd = this.detruireObjet;
		suivreP.delayToDestroy = this.tempsAvantDestroy;

		yield return null;

		suivreP.setPrecisionCalcul (.25f);
		emptyParent.SetActive(true);

		yield return null;
	}

	public class AddRigidodyOnTrigger : MonoBehaviour{

		private CACreatePlateformPourParcour scriptParent;

		private GameObject emptyParentTarget;


		public void OnTriggerEnter (Collider other){
			if (null == emptyParentTarget.GetComponent<Rigidbody> ()) {
				Rigidbody rigidtarget = emptyParentTarget.AddComponent<Rigidbody> ();
				rigidtarget.useGravity = false;
				rigidtarget.freezeRotation = true;
				rigidtarget.mass = 100f * scriptParent.taille.x * scriptParent.taille.y * scriptParent.taille.z;
			}
		}

		public void setScriptParent(CACreatePlateformPourParcour scriptParent){
			this.scriptParent = scriptParent;
		}

		public void setEmptyParentTarget(GameObject emptyParentTarget){
			this.emptyParentTarget = emptyParentTarget;
		}
	}
}
