using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentationChoc : MonoBehaviour {

	private Rigidbody rigidB;

	public float multipliBy;
	public string[] tags;

	// Use this for initialization
	void Start () {
		rigidB = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void OnCollisionEnter(Collision col) {
		if (null != rigidB && (tags.Length ==0 || containTags(tags, col.collider.gameObject.tag))) {
			if (rigidB.isKinematic) {
				rigidB.isKinematic = false;
			}

			rigidB.AddForce (multipliBy * col.impulse, ForceMode.Impulse);
		}
	}

	private bool containTags(string[] tags, string tagTest){
		bool result = false;

		foreach(string tag in tags){
			if(tag == tagTest){
				result = true;
				break;
			}
		}

		return result;
	}

}
