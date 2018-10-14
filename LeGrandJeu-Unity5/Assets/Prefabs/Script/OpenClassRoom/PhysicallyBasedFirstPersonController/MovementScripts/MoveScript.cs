using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {
	
	[SerializeField]
	private Transform _transform;

	[SerializeField]
	private Rigidbody _rigidbody;

	[SerializeField]
	private float _moveForce;

	[SerializeField]
	private float _geometricDrag;

	[SerializeField]
	private float _linearDrag;

	[SerializeField]
	private float _maxVelocity;

	[SerializeField]
	private AudioClip[] _feetSounds;

	private RaycastHit _hit;

	private AudioSource _audioSource;

	//distance entre chaque bruit de pas
	private float timeBeforeNewClip;
	private float mouvementDemander;	//Temps entre le relachment de la touche et la fin des bruit de pas
	private float stepOff; //  distance a sur laquelle peut monter le controller
	private Collider previousStep;

	void Start () {
		if (_rigidbody == null) {
			_rigidbody = this.GetComponent<Rigidbody> ();
		}
		_audioSource = GetComponent<AudioSource> ();
		timeBeforeNewClip = 2f;
		mouvementDemander = 2.5f;
		stepOff = .5f;
		previousStep = null;
	}
	
	void FixedUpdate () {
		if (timeBeforeNewClip < 0) {
			PlayFootStepAudio (_rigidbody.velocity.magnitude);
			timeBeforeNewClip = 2f;
		}
			
		bool isTropPentu = false;
		var direction = (_transform.forward * Input.GetAxis("Vertical") + _transform.right * Input.GetAxis("Horizontal")).normalized;
		direction = direction.normalized;

		if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
			mouvementDemander = 2.5f;
		} else {
			mouvementDemander -= Time.deltaTime;
		}
			
		//Pente max egal a 30°
		if (direction != Vector3.zero && Physics.SphereCast (_transform.position + _transform.up * 1f, .001f, direction, out _hit, Mathf.Sqrt (3f))) {
			if (_hit.normal.y > .1f) {
				isTropPentu = true;
			}
		}

		var speeddelta =  (direction *_moveForce);
		var rigidVelocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _rigidbody.velocity.z);
		var speed = rigidVelocity - Vector3.Scale(_transform.up , rigidVelocity);

		_rigidbody.AddForce((speed - speed.normalized * _linearDrag) * _geometricDrag - speed, ForceMode.VelocityChange);

		speed -= speed.normalized * _linearDrag;
		speed *= _geometricDrag;
	
		if (speed.magnitude > _maxVelocity || isTropPentu)
		{
			_rigidbody.AddForce(Vector3.ClampMagnitude((speed + speeddelta * Time.deltaTime), speed.magnitude) - speed, ForceMode.VelocityChange);
		}
		else
		{
			_rigidbody.AddForce(Vector3.ClampMagnitude((speed + speeddelta * Time.deltaTime), _maxVelocity) - speed, ForceMode.VelocityChange);
		}

		if (mouvementDemander > 0) {
			timeBeforeNewClip -= _rigidbody.velocity.magnitude * Time.deltaTime;
		}
			
			

			//On vérifie si c'est une marche
		if (Input.GetAxis ("Jump") == 0 && direction != Vector3.zero && !Physics.Raycast (_transform.position + _transform.up * stepOff, direction, 1f)) {
			RaycastHit _hit2;
			if (Physics.BoxCast (_transform.position + _transform.up * stepOff + direction * .5f, new Vector3 (.2f, .05f, .5f), _transform.up * -1, out _hit2, _transform.rotation, stepOff - .1f) && !_hit2.collider.isTrigger && previousStep != _hit2.collider) {
 				previousStep = _hit2.collider;
				float tailleDeMarche = stepOff - _hit2.distance;
				if (tailleDeMarche > 0) {
					float vitesseVertical = Mathf.Sqrt (Mathf.Abs(2 * tailleDeMarche * Physics.gravity.y));
					if (_rigidbody.velocity.y < vitesseVertical) {
						_rigidbody.AddForce (vitesseVertical * _transform.up, ForceMode.VelocityChange);
					}
				}
			} else {
				previousStep = null;
			}
		}
   	}

	private float produitScalaire(Vector3 vecteur1,Vector3 vecteur2){
		return Vector3.Scale (vecteur1, vecteur2).sqrMagnitude;
	}

	private void PlayFootStepAudio(float speed)
	{
		int lastIndex = 0;
		int n = 0;

		if (_feetSounds.Length == 0 ||!Physics.SphereCast(_transform.position + _transform.up * .4f * 1.1f, .4f, _transform.up*-1f, out _hit, 0.1f))
		{
			return;
		}
		// pick & play a differentrandom footstep sound from the array,
		while (n == lastIndex){
			n = Random.Range(0, _feetSounds.Length);
		}
			
		_audioSource.clip = _feetSounds[n];
		_audioSource.pitch = speed/2f > .75f ? speed/2f : .75f;
		_audioSource.PlayOneShot (_audioSource.clip);

		lastIndex = n;
	}

	/*void OnCollisionEnter(Collision collision)	{
		Vector3 firstContactPoint = Vector3.zero;

		foreach (ContactPoint contact in collision.contacts){
			Vector3 distanceTotalVector = contact.point - transform.position;

			Vector3 rayon = Vector3.ProjectOnPlane (distanceTotalVector, _transform.up);

			float angleForward = Vector3.Angle(transform.forward, rayon);

			//Permet de négliger les colision trop proche du centre (pied et tête)
			//Pour prendre uniquement les collision frontal
			if(rayon.magnitude > .3f && angleForward < 45f){
				firstContactPoint = contact.point;
				break;
			}
		}

		if (firstContactPoint != Vector3.zero) {
			Vector3 rayon = Vector3.ProjectOnPlane (firstContactPoint - transform.position, _transform.up);

			Vector3 directionContact = rayon.normalized;

			RaycastHit hitInfo;
			if (Physics.Raycast (transform.position + 2 * transform.up, directionContact, out hitInfo)) {
				if (null != _rigidbody) {
					_rigidbody.AddForce (-.5f * directionContact, ForceMode.VelocityChange);
				} 
			} else {
				RaycastHit hitUpInfo;
				RaycastHit hitDown;
				bool grimpe = false;

				//On regarde les deux limite
				if (Physics.Raycast (transform.position + 2 * transform.up + directionContact, transform.up, out hitUpInfo, 3f) &&
				   Physics.Raycast (transform.position + 2 * transform.up + directionContact, -transform.up, out hitDown, 3f)) {
					//On vérifie que le haut et le bas à au mois 3 metre
					float distance = Vector3.Distance (hitUpInfo.point, hitDown.point);
					if (distance > 3f) {
						grimpe = true;
					}
				} else {		//Une direction a plus de 3 metre de direction, on peut grimper
					grimpe = true;
				}

				if (grimpe && _rigidbody && false) {
					StartCoroutine (grimperAuSommet (directionContact));
				}
			}
		} else if (collision.contacts.Length > 0) {
			Vector3 versContact = collision.contacts[0].point - (transform.position +_transform.up);
			if (null != _rigidbody) {
				_rigidbody.AddForce (Vector3.Reflect (versContact, collision.contacts [0].normal), ForceMode.Force);
			}
		}
	}*/


	/**
	 * Rigidbody obligatoire
	 * 
	 * */
	private IEnumerator grimperAuSommet(Vector3 direction){
		RaycastHit hit;
		while(Physics.Raycast (transform.position , direction, out hit, 3f)) {
			_rigidbody.AddForce (0.5f * _transform.up * Time.deltaTime, ForceMode.VelocityChange);
			yield return null;
		}

		if (null != _rigidbody) {
			_rigidbody.AddForce (1f * direction, ForceMode.VelocityChange);
		} 
		yield return null;
	}
}
