using UnityEngine;
using System.Collections;

public class TranslationVers : MonoBehaviour {


	public Transform target;
	public float speed;
	public bool isLoop = false;

	private Vector3 positionInitiale;
	private bool isRetour;

	void Start(){
		positionInitiale = transform.position;
		isRetour = false;
	}

	void Update() {

		if (Vector3.Distance (transform.position, target.position) != 0 || isLoop) {
			float step = speed * Time.deltaTime;

			if (isLoop && !isRetour && Vector3.Distance (transform.position, target.position) < Vector3.Distance (positionInitiale, target.position)/100f){
				isRetour = true;
			} else if (isLoop && isRetour && Vector3.Distance (transform.position, positionInitiale) < Vector3.Distance (positionInitiale, target.position)/100f){
				isRetour = false;
			}

			if(!isRetour){
				transform.position = Vector3.MoveTowards (transform.position, target.position, step);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, positionInitiale, step);
			}
		}
	}
}
