using UnityEngine;
using System.Collections;

public class RebondParametre : MonoBehaviour {

	public float multiplicateurRebond = 1;

	private float delayMinEntreRebond;
	private bool isRebond;

	// Use this for initialization
	void Start () {
		delayMinEntreRebond = .25f;
		isRebond = true;
	}

	IEnumerator OnCollisionEnter(Collision collision){
		if (isRebond && collision.rigidbody != null && collision.contacts.Length > 0) {
			Rigidbody rigidBodyColision = collision.rigidbody;
			Vector3 velocityReflet = multiplicateurRebond * Vector3.Reflect (rigidBodyColision.velocity, collision.contacts [0].normal);
			yield return null;
			rigidBodyColision.velocity = velocityReflet;
			isRebond = false;
			yield return new WaitForSeconds (delayMinEntreRebond);
			isRebond = true;
		}

	}
}
