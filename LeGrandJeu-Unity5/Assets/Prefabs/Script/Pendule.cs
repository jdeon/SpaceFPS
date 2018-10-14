using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Necessite un rigidBody et un hingePoint
 * */
public class Pendule : MonoBehaviour, IActivable {

	public enum ModePendule {Symetrique, startPositionAsRef} //startPositionAsRef non implementer

	public bool actifAtStart;
	public float tempsDePeriode;
	public ModePendule mode;

	private bool actif;
	private Rigidbody rigidComposent;
	private HingeJoint hingeJointComponent;
	private float coefMouvementFluide;

	// Use this for initialization
	void Start () {
		actif = false;
		rigidComposent = GetComponent<Rigidbody> ();
		hingeJointComponent = GetComponent<HingeJoint> ();
		coefMouvementFluide = 2.5f; //evite d atteindre la limit
		if (actifAtStart) {
			activate ();
		} 
	}

	public void activate(){
		if (!actif && isComposantNecessaire ()) {
			actif = true;
			rigidComposent.isKinematic = false;
			hingeJointComponent.useMotor = true;
			if (mode == ModePendule.Symetrique) {
				calculSymetriqueMotor ();
				StartCoroutine (symetriqueBalancier ());
			}
		}
	}

	public void desactivate(){
		actif = false;
		if (isComposantNecessaire ()) {
			rigidComposent.isKinematic = true;
			hingeJointComponent.useMotor = false;
		}
	}

	public bool getIsActif(){
		return actif;
	}

	private IEnumerator symetriqueBalancier (){
		float quartParcours = (hingeJointComponent.limits.max - hingeJointComponent.limits.min) / 4;
		bool retour = false; //Pour ne pas changer de direction des le début

		while (actif && isComposantNecessaire ()) {

			Vector3 rayon = transform.position - hingeJointComponent.connectedAnchor;
			float temps = Time.fixedDeltaTime;
			Vector3 directionObjet = Vector3.Cross (hingeJointComponent.axis, rayon);

			if (mode == ModePendule.Symetrique){
				if (retour && hingeJointComponent.angle < hingeJointComponent.limits.min + quartParcours) {
					//Debug.Log (rigidComposent.angularVelocity + " ; " + rigidComposent.velocity);
					JointMotor newMotor = new JointMotor ();
					newMotor.force = hingeJointComponent.motor.force;
					newMotor.targetVelocity = hingeJointComponent.motor.targetVelocity * -1;
					hingeJointComponent.motor = newMotor;
					retour = false;
				}else if (!retour && hingeJointComponent.angle > hingeJointComponent.limits.max - quartParcours){
					//Debug.Log (rigidComposent.angularVelocity + " ; " + rigidComposent.velocity);
					JointMotor newMotor = new JointMotor ();
					newMotor.force = hingeJointComponent.motor.force;
					newMotor.targetVelocity = hingeJointComponent.motor.targetVelocity * -1;
					hingeJointComponent.motor = newMotor;
					retour = true;
				}
			} 
			yield return new WaitForSeconds(.05f);
		}

		actif = false;
		yield return null;
	}

	private bool isComposantNecessaire(){
		if (null != rigidComposent && null != hingeJointComponent && tempsDePeriode > 0) {
			return true;
		} else {
			if (null == rigidComposent && null != GetComponent<Rigidbody> ()) {
				rigidComposent = GetComponent<Rigidbody> ();
			} else {
				return false;
			}

			if (null == rigidComposent) {
				if (null != GetComponent<Rigidbody> ()) {
					rigidComposent = GetComponent<Rigidbody> ();
				} else {
					return false;
				}
			}

			if (null == hingeJointComponent) {
				if (null != GetComponent<HingeJoint> ()) {
					hingeJointComponent = GetComponent<HingeJoint> ();
				} else {
					return false;
				}
			}

			if (tempsDePeriode > 0 && mode == ModePendule.Symetrique) {
				calculSymetriqueMotor ();
				return true;
			}
		}
		return false;
	}

	/**
	 * Calcul la foce necessaire
	 * 
	 * */
	private void calculSymetriqueMotor (){
		//Calcul de velocité et force fait avec les données suivante
		//2 v*(t1) = delta(angle); 4* 1/2 * a * (t2)^2 = delta(angle) ; a*(t2) = v
		// f = m * rayon * a

		float rayonAncre = Vector3.ProjectOnPlane(transform.position - hingeJointComponent.connectedAnchor, hingeJointComponent.axis).magnitude;
		float force = tempsDePeriode > 0 ? 18 * rigidComposent.mass * rayonAncre * Mathf.Deg2Rad * (hingeJointComponent.limits.max - hingeJointComponent.limits.min) / Mathf.Pow (tempsDePeriode, 2) : 0;
		float vitesseAngulaire = 3 * rayonAncre * (hingeJointComponent.limits.max - hingeJointComponent.limits.min) / tempsDePeriode;

		JointMotor newMotor = new JointMotor ();
		newMotor.force = force * coefMouvementFluide;
		newMotor.targetVelocity = vitesseAngulaire;

		hingeJointComponent.motor = newMotor;
	}
}