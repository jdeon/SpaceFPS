using UnityEngine;
using System.Collections;

public class CADeplacerTargetAvecVecteur : CustomActionScript {
	
	public Vector3 destination;
	public float tempsAller;
	public bool isLoop = false;
	public bool isDestinationLocal= true;
	
	private Vector3 positionInitiale;
	private bool isRetour;
	private float vitesse;
	private Vector3 destinationFinal;

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

			if(!isRetour){
				transform.position = Vector3.MoveTowards (transform.position, destinationFinal, vitesse * Time.deltaTime);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, positionInitiale, vitesse * Time.deltaTime);
			}

			tempsRestant -= Time.deltaTime;
			yield return null;
		}
		
		yield return null;
	}
}