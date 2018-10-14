using UnityEngine;
using System.Collections;

public class CAModifierTaille : CustomActionScript {

	public Vector3 tailleFinal;
	public float tempsAller;
	public bool isLoop = false;
	
	private Vector3 tailleInitiale;
	private bool isRetour;
	private float vitesse;
	private float taillePourDisable = .005f;
	
	public override void  Start(){
		base.Start ();
		tailleInitiale = transform.localScale;
		isRetour = false;
		
		vitesse = (tailleFinal - tailleInitiale).magnitude / tempsAller;
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
				transform.localScale = Vector3.MoveTowards (transform.localScale, tailleFinal, vitesse * Time.deltaTime);
			} else {
				transform.localScale = Vector3.MoveTowards (transform.localScale, tailleFinal, vitesse * Time.deltaTime);
			}

			if(transform.localScale.x < taillePourDisable || transform.localScale.y < taillePourDisable || transform.localScale.z < taillePourDisable){
				transform.gameObject.SetActive(false);
				isFinie = true;
			}

			tempsRestant -= Time.deltaTime;
			yield return null;
		}
		
		yield return null;
	}
}
