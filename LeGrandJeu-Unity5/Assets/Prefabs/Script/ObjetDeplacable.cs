using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetDeplacable : MonoBehaviour {

	[SerializeField]
	private float vitesse;

	private bool active;

	void OnMouseDown()
	{
		if(!active){
		ObjetDeplacable[] listObjet = FindObjectsOfType<ObjetDeplacable>();
			foreach (ObjetDeplacable objectDeplacable in listObjet){
				objectDeplacable.setActive(false);
			}

			this.active = true;
		}
	}

	void OnMouseUp()
	{
		this.active = false;
	}

	void Update(){
		if (active && Input.GetMouseButton(0)) {
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

			Vector3 objectif = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (objectif.x, transform.position.y, objectif.z), vitesse * Time.deltaTime);
		}
	}


	void setActive (bool active){
		this.active = active;
	}
}
