using UnityEngine;
using System.Collections;

public class ArmePistolet : ArmeAFeuAbstract {

	public float forceTire;
	public Material traceBalleMaterial;

	protected override void tirer(){
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = transform.position;
		sphere.transform.rotation = transform.rotation;
		sphere.transform.localScale = new Vector3 (0.005f, 0.005f, 0.01f);
		sphere.AddComponent<Rigidbody>().mass = .01f;
		sphere.GetComponent<Rigidbody>().useGravity = false;
		sphere.layer = Constantes.LAYER_PROJECTILE;
		sphere.tag = Constantes.TAG_PROJECTILE_LIFE;
		sphere.GetComponent<Rigidbody>().AddForce (sphere.transform.forward * forceTire);
		TrailRenderer trail = sphere.AddComponent<TrailRenderer> ();
		trail.time = .5f;
		trail.startWidth = .01f;
		trail.endWidth = .1f;
		trail.startColor = Color.white;
		trail.endColor = new Color (.25f, .25f, .25f, .5f);
		trail.autodestruct = true;
		trail.sharedMaterial = traceBalleMaterial;

		StartCoroutine ("destroyProjectile", sphere);
	}
}
