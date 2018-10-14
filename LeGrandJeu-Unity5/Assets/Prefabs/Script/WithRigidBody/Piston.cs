using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour, IActivable  {

	public Vector3 positionPause;
	public Vector3 positionMax;
	public Vector3 positionMin;
	public int speed;
	public bool actifAtStart;

	private Rigidbody objectRigidbody;
	private bool isActif;
	private bool debutArret;
	private float precision;
	private float proportionRalentissement;


	// Use this for initialization
	void Start () {
		this.objectRigidbody = gameObject.GetComponent<Rigidbody> ();
		this.isActif = false;
		this.precision = .001f;
		this.proportionRalentissement = .1f;
		if (actifAtStart) {
			activate ();
		}
	}
		
	 public bool getIsActif(){
		return this.isActif;
	}

	public 	void activate (){
		this.isActif = true;
		this.objectRigidbody.isKinematic = false;
		StartCoroutine ("lancerPiston");
	}

	public void desactivate(){
		this.debutArret = true;
	}

	private IEnumerator lancerPiston (){
		float proportion;

		float vitesse = 0;
		this.debutArret = false;
		bool isAller = true;
		Vector3 pointDestination = this.positionMax;
		Vector3 direction = (this.positionMax - this.positionMin).normalized; 

		float distanceTotal = Vector3.Distance (positionMin, positionMax);

		//Pas de rigidbody, pas de deplacement
		if (null != this.objectRigidbody) {


			while (Vector3.Distance (pointDestination, transform.localPosition) / distanceTotal > this.proportionRalentissement) {
				this.objectRigidbody.velocity = direction * this.speed;
				yield return null;
			}

		transform.localPosition = pointDestination;
		isAller = !isAller;
		pointDestination = isAller ? this.positionMax : this.positionMin;
		direction *= -1;
		this.objectRigidbody.velocity = Vector3.zero;
		yield return null;

		//Boucle de déplacement du "piston"
		//this.debutArret est modifier par desactivate()
		while (!this.debutArret) {
			proportion = Vector3.Distance(pointDestination, transform.localPosition) / distanceTotal;

			//Est on arrivé à la destination
			if (proportion < this.precision) {
				isAller = !isAller;
				pointDestination = isAller ? this.positionMax : this.positionMin;
				direction *= -1;
				this.objectRigidbody.velocity = Vector3.zero;
				yield return null;
			} //A t on depacer la destination
			else if (isHorsLimite(direction,pointDestination, transform.localPosition)){
				transform.localPosition = pointDestination;
				this.objectRigidbody.velocity = Vector3.zero;
				vitesse = 0;
			} //Est on avant l origine
			else if ((isAller && isHorsLimite(-direction,this.positionMin, transform.localPosition)) || (!isAller && isHorsLimite(-direction,this.positionMax, transform.localPosition))){
				transform.localPosition = isAller ? this.positionMin : this.positionMax;
				this.objectRigidbody.velocity = Vector3.zero;
				vitesse = 0;
			} //Dans portion ralentissement d arriver
			else if (proportion < this.proportionRalentissement) {
				vitesse = (proportion / this.proportionRalentissement) < .1f ? .1f * this.speed : (proportion / this.proportionRalentissement) * this.speed;
			} //Dans portion ralentissement de depart
			else if (proportion > (1 - this.proportionRalentissement)) {
				vitesse = ((1 - proportion) / this.proportionRalentissement) < .1f ? .1f * this.speed : ((1 - proportion) / this.proportionRalentissement) * this.speed;
			} else  {
				vitesse = this.speed;
			}
				this.objectRigidbody.velocity = direction * vitesse;

			yield return null;
		}


		//Le "piston" est désactivé, il revient à sa position de départ
		pointDestination = this.positionPause;
		direction = (pointDestination - transform.localPosition).normalized;

		//Boucle pour revenir à la position initial
		while (this.debutArret) {
			proportion = Vector3.Distance(pointDestination, transform.localPosition) / distanceTotal;
			if (proportion >= this.proportionRalentissement) {
				vitesse = this.speed / Time.deltaTime;
			} else if (proportion < this.precision) {
				this.debutArret = false;
				yield return null;
			} else	{
				vitesse = (proportion / this.proportionRalentissement) * speed / Time.deltaTime;
			}

			this.objectRigidbody.AddForce(direction * vitesse - this.objectRigidbody.velocity, ForceMode.VelocityChange);
			yield return null;
			}

		//La désactivation est complete
		this.objectRigidbody.isKinematic = true;
		this.isActif = false;

		yield return null;
	}
	}

	/**Vérifie si le point va au dela des deux limite*/
	private bool isHorsLimite(Vector3 direction, Vector3 destination, Vector3 position){
		if (direction.x < 0 && position.x < destination.x) {
			return true;
		} else if (direction.x > 0 && position.x > destination.x) {
			return true;
		} 

		if (direction.y < 0 && position.y < destination.y) {
			return true;
		} else if (direction.y > 0 && position.y > destination.y) {
			return true;
		} 

		if (direction.z < 0 && position.z < destination.z) {
			return true;
		} else if (direction.z > 0 && position.z > destination.z) {
			return true;
		} 

		return false;
	}
}