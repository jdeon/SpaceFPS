using UnityEngine;
using System.Collections;

public class RotationContinueAutour : MonoBehaviour {

	public float AngleXParSec = 10f;
	public float AngleYParSec = 10f;
	public float AngleZParSec = 10f;

	//L'objet tourne t il autour de lui meme
	public bool isObjectRotating =false;

	public Transform centre;
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = transform.position - centre.position;

		direction = Quaternion.AngleAxis(AngleXParSec * Time.deltaTime, Vector3.right) * direction;
		direction = Quaternion.AngleAxis(AngleYParSec * Time.deltaTime, Vector3.up) * direction;
		direction = Quaternion.AngleAxis(AngleZParSec * Time.deltaTime, Vector3.forward) * direction;

		transform.position = centre.position + direction;

		if (isObjectRotating) {
			transform.Rotate (AngleXParSec * Time.deltaTime, AngleYParSec * Time.deltaTime, AngleZParSec * Time.deltaTime);
		}
	}
}
