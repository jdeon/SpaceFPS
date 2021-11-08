using UnityEngine;
using System.Collections;

public class CADeplacerTargetAvecVecteur : CustomActionScript {
	
	public Vector3 destination;
	public float tempsAller;
	public bool isLoop = false;
	public bool isDestinationLocal= true;
	public float delaiUpdate;
	
	private Vector3 positionInitiale;
	private bool isRetour;
	private float vitesse;
	private Vector3 destinationFinal;
	private float timeLastUpdate;

	public override void  Start(){
		base.Start ();
		positionInitiale = transform.position;
		isRetour = false;
		if (isDestinationLocal) {
			destinationFinal = destination + positionInitiale;
		} else {
			destinationFinal = destination;
		}
		vitesse = (destinationFinal - positionInitiale).magnitude / tempsAller;
	}
	
	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		float tempsRestant = tempsAller;
		bool isFinie = false;

		timeLastUpdate = Time.fixedTime;

		while (!isFinie){
			if (isLoop && !isRetour && tempsRestant > 0){
				isRetour = true;
				tempsRestant = tempsAller;
			} else if (isLoop && isRetour && tempsRestant > 0){
				isRetour = false;
				tempsRestant = tempsAller;
			} else if (tempsRestant < 0){
				isFinie = true;
			}

			float t = Time.fixedTime - timeLastUpdate;

			if(!isRetour){
				transform.position = Vector3.MoveTowards (transform.position, destinationFinal, vitesse * t);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, positionInitiale, vitesse * t);
			}

			tempsRestant -= t;
			timeLastUpdate = Time.fixedTime;

			if (delaiUpdate > 0) {
				yield return new WaitForSeconds(delaiUpdate);
			} else {
				yield return null;
			}
		}
		
		yield return null;
	}
}