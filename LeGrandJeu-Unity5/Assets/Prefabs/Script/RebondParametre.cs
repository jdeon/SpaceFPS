using UnityEngine;
using System.Collections;

public class RebondParametre : MonoBehaviour {

	public float multiplicateurRebond = 1;
	public bool isUniqueDir;//Rebondi a partir du sens z uniquement

	private float delayMinEntreRebond;
	private bool isRebondOk;

	// Use this for initialization
	void Start () {
		delayMinEntreRebond = .25f;
		this.isRebondOk = true;
	}

	IEnumerator OnCollisionEnter(Collision collision){
		if (this.isRebondOk && collision.rigidbody != null && collision.contacts.Length > 0) {
			Rigidbody rigidBodyColision = collision.rigidbody;
			Vector3 velocityReflet = multiplicateurRebond * Vector3.Reflect (rigidBodyColision.velocity, collision.contacts [0].normal);
			yield return null;
			if(!isUniqueDir || Vector3.Dot(transform.up,velocityReflet) > 0){
				rigidBodyColision.velocity = velocityReflet;
			}

			this.isRebondOk = false;
			yield return new WaitForSeconds (delayMinEntreRebond);
			this.isRebondOk = true;
		}

	}
}
