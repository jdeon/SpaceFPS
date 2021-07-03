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

        GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.rotation = transform.rotation;
		cube.transform.localScale = this.taille;
		cube.transform.parent = emptyParent.transform;
		cube.transform.localPosition = decalageAuParcour;
		cube.layer = Constantes.LAYER_DETECT_ZONE;


		if (rigidBodyAtBeggining) {
			Rigidbody rigidParent = emptyParent.AddComponent<Rigidbody> ();
			rigidParent.useGravity = false;
			rigidParent.freezeRotation = true;
			rigidParent.mass = 100f * this.taille.x * this.taille.y * this.taille.z;
		} else {
			//Si pas de rigidbody a la creation, on crée une zone qui créera un rigidbody si le controller entre
			AddRigidBodyOnTrigger addRigidodyOnTrigger = emptyParent.AddComponent<AddRigidBodyOnTrigger> ();
			addRigidodyOnTrigger.volume = this.taille;
			addRigidodyOnTrigger.offset = decalageAuParcour;
			addRigidodyOnTrigger.isKinectic = true;
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
}
