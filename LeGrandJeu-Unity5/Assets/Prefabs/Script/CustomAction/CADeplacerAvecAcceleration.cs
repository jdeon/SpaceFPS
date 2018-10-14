using UnityEngine;
using System.Collections;

public class CADeplacerAvecAcceleration : CustomActionScript {

	public Transform destination;
	public bool isLoop = false;

	public enum ModeAcceleration {Constant, Lineaire,Sinusoidale,Acceleration};
	public ModeAcceleration modeAcceleration;
	public float vitesseMax;
	public float dureeAuMax;

	private bool isRetour;
	private Vector3 positionInitiale;
	private Vector3 destinationFinal;
	private Vector3 direction;
	private float distanceDAcseleration;
	private float distanceTotal;
    private Rigidbody objectRigidbody;

    public override void  Start(){
		base.Start ();
		this.isRetour = false;
		this.positionInitiale = transform.position;
		this.destinationFinal = destination.position;
		this.direction = (this.destinationFinal - this.positionInitiale).normalized;
		this.distanceTotal = Vector3.Distance (this.positionInitiale, this.destinationFinal);
		this.distanceDAcseleration = (this.distanceTotal - this.vitesseMax * this.dureeAuMax) / 2f;

        this.objectRigidbody = GetComponent<Rigidbody>();


        if (this.distanceDAcseleration <= 0) {
			modeAcceleration = ModeAcceleration.Constant;
		}
	}
	
	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		bool isFinie = false;
		float vitesse;

		this.objectRigidbody.isKinematic = false;
		
		while (!isFinie){
			if (isLoop && !isRetour && Vector3.Distance(transform.position, this.destinationFinal) < .5f){
				isRetour = true;
			} else if (isLoop && isRetour && Vector3.Distance(transform.position, this.positionInitiale) < .5f){
				isRetour = false;
			} else if (Vector3.Distance(transform.position, this.destinationFinal) < .5f){
				isFinie = true;
                this.objectRigidbody.isKinematic = true;
			}

			if(!isRetour){
				float portionParcourue = Vector3.Distance(transform.position, this.positionInitiale); 
				vitesse = calculVitesseInstantanne(portionParcourue);
				this.objectRigidbody.velocity = direction * vitesse;
			} else {
				float portionParcourue = Vector3.Distance(transform.position, this.destinationFinal);
				vitesse = calculVitesseInstantanne(portionParcourue);
				this.objectRigidbody.velocity = direction * vitesse;
			}
			yield return null;
		}
		
		yield return null;
	}

	private float calculVitesseInstantanne(float portionParcourue){
		switch (this.modeAcceleration) {
		case ModeAcceleration.Constant : 
			return this.vitesseMax;

		case ModeAcceleration.Lineaire : 
			if(portionParcourue <= this.distanceDAcseleration){
				float vitesse = this.vitesseMax * portionParcourue/ this.distanceDAcseleration;
				if (vitesse < .5f) {
					vitesse = .5f;
				}
				return vitesse;
			} else if (portionParcourue >= this.distanceDAcseleration + this.vitesseMax*this.dureeAuMax){
				float portionDeceleration = portionParcourue- (this.distanceDAcseleration + this.vitesseMax*this.dureeAuMax);
				return this.vitesseMax * (1 - portionDeceleration / this.distanceDAcseleration);
			} else {
				return this.vitesseMax;
			}

		case ModeAcceleration.Sinusoidale : 
			if(portionParcourue <= this.distanceDAcseleration){
				return this.vitesseMax * Mathf.Sin(Mathf.PI*portionParcourue/ this.distanceDAcseleration);
			} else if (portionParcourue >= this.distanceDAcseleration + this.vitesseMax*this.dureeAuMax){
				float portionDeceleration = portionParcourue- (this.distanceDAcseleration + this.vitesseMax*this.dureeAuMax);
				return this.vitesseMax * Mathf.Sin(Mathf.PI + Mathf.PI*portionDeceleration/ this.distanceDAcseleration);
			} else {
				return this.vitesseMax;
			}

		//case ModeAcceleration.Acceleration :	;
		}
		return 0;
	}
}
