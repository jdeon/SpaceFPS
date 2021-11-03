using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marteau : ObjetPortable {

	private static string VERTICAL = "V";
	private static string HORIZONTAL = "H";

	public string[] attackOrder;
	private int indexAttack;

	private bool attacking;


	// Use this for initialization
	void Start () {
		attacking = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && !attacking){
			if (transform.parent.name == "MainDroite" || transform.parent.name == "MainGauche") {
				string typeAttack;

				if (indexAttack < attackOrder.Length) {
					typeAttack = attackOrder [indexAttack];
					indexAttack++;
				} else {
					indexAttack = 0;
					typeAttack = attackOrder [indexAttack];
				}
					
				StartCoroutine (frappe (typeAttack));
			}
		}
	}

	IEnumerator frappe(string typeAttack){
		attacking = true;
		float timeInAttack = 0;
		//Mains;Objet portable; controller
		Transform controller = transform.parent.parent.parent;

		if (typeAttack.ToUpper () == HORIZONTAL) {
			////Horizontal
			Vector3 dirPlane = Vector3.ProjectOnPlane(transform.right, controller.up);
			float angle = Vector3.Angle (transform.up, dirPlane);

			//Rotation parallele au sol 0.5s
			while (timeInAttack < .5f) {
				transform.Rotate (angle * Time.deltaTime / 0.5f, 0, 0, Space.Self);
				timeInAttack += Time.deltaTime;
				yield return null;
			}

			//Frappe 0.5s -> force
			Rigidbody rigidB = GetComponent<Rigidbody> ();
			while (timeInAttack < 1f) {
				int sens = Vector3.Angle (transform.up, controller.forward) > 0 ? 1 : -1;
				rigidB.AddTorque (0f,1f * sens,0f,ForceMode.Acceleration);
				timeInAttack += Time.deltaTime;
				yield return null;
			}

			//Retour à la mains 1s
			Vector3 positionInit = transform.position;
			Quaternion rotationInit = transform.rotation;
			while (timeInAttack < 2f) {
				transform.position = Vector3.Slerp (positionInit, transform.parent.position, timeInAttack - 1f);
				transform.rotation = Quaternion.Slerp(rotationInit, transform.parent.rotation, timeInAttack - 1f);
				timeInAttack += Time.deltaTime;
				yield return null;
			} 
		} else if (typeAttack.ToUpper () == VERTICAL) {
			////Vertial
			//Rotation 90
			yield return null;
		}

		attacking = false;

		yield return null;
	}

}
