using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformTransmissionForce : MonoBehaviour {

	private static readonly float COEF_FROTTEMENT = 0.90f;

	private Vector3 _oldSpeed;
	private Vector3 _deltaSpeed;
	private Rigidbody _rigidbody;
	private HashSet<Rigidbody> rigidBodyInside;

	// Use this for initialization
	void Start () {
		_oldSpeed = Vector3.zero;
		_deltaSpeed = Vector3.zero;

		_rigidbody = transform.parent.GetComponent<Rigidbody> ();

		rigidBodyInside = new HashSet<Rigidbody> ();
	}

	void FixedUpdate(){
		if (null != _rigidbody) {
			_deltaSpeed = _rigidbody.velocity - _oldSpeed;
			_oldSpeed = _rigidbody.velocity;


			foreach(Rigidbody rg in rigidBodyInside){
				//rg.AddForce (_deltaSpeed , ForceMode.VelocityChange);
				rg.MovePosition(rg.position + _rigidbody.velocity * Time.deltaTime * COEF_FROTTEMENT);
			}
		}
	}

	/*void OnCollisionStay(Collision collisionInfo)
	{
		if (null != _rigidbody) {
			_deltaSpeed = _rigidbody.velocity - _oldSpeed;
			_oldSpeed = _rigidbody.velocity;
		}

		if (null != collisionInfo.rigidbody && null != _rigidbody) {
			
		}
	}*/

	void OnTriggerEnter(Collider other) {

		Rigidbody rbOther = other.gameObject.GetComponent<Rigidbody> ();
		if(null != rbOther){
			rigidBodyInside.Add (rbOther);
		}
	}

	void OnTriggerExit(Collider other) {
		Rigidbody rbOther = other.gameObject.GetComponent<Rigidbody> ();
		if(null != rbOther){
			rigidBodyInside.Remove (rbOther);
		}
	}
}
