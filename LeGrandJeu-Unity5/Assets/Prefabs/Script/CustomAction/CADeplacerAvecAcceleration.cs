using UnityEngine;
using System.Collections;

/*
 * FIXME problème de coheficient 2 sur le temps
 * */
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
	private float distanceDAcseleration;
	private float distanceTotal;
    private Rigidbody objectRigidbody;

    public override void  Start(){
		base.Start ();
		this.isRetour = false;
		this.positionInitiale = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		this.destinationFinal = destination.position;
		this.distanceTotal = Vector3.Distance (this.positionInitiale, this.destinationFinal);

		if (ModeAcceleration.Lineaire == modeAcceleration)
		{
			/*
			 * D = a*t1*t1/2 + Vmax*t2 + a*t1*t1/2  => 2*t1 + t2 <= Tmax et a*t1 = Vmax
			 * D = a*t1*t1 + Vmax*t2 = Vmax(t1+t2) => Vmax (t1 + Tmax - 2*t1) = Vmax(Tmax-t1)
			 * t1 = Tmax - D/Vmax => a = Vmax/t1 = Vmax / (Tmax - D/Vmax)
			 * d1 = a *t1 * t1 /2 = Vmax * t1 / 2 = (Vmax*Tmax - D) / 2
			*/
			this.distanceDAcseleration = (this.vitesseMax * this.dureeAuMax - this.distanceTotal) / 2;
		}

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

			Vector3 direction = (this.destinationFinal - transform.position).normalized;

			if (!isRetour){
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
			if (portionParcourue <= this.distanceDAcseleration)
				{
				float vitesse = this.vitesseMax * portionParcourue/ this.distanceDAcseleration;
				if (vitesse < .5f) {
					vitesse = .5f;
				}
				return vitesse;
			} else if (portionParcourue >= this.distanceTotal - this.distanceDAcseleration){
				float portionDeceleration = portionParcourue - (this.distanceTotal - this.distanceDAcseleration);
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
