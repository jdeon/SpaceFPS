using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {
	
	[SerializeField]
	private Transform _transform;

	[SerializeField]
	private Rigidbody _rigidbody;

	[SerializeField]
	private float _accelerationForce;

	[SerializeField]
	private float _decelerationForce;

	[SerializeField]
	private float _maxVelocity;

	[SerializeField]
	private float stepOff; //  distance a sur laquelle peut monter le controller

	[SerializeField]
	private float _stepSmooth;

	[SerializeField]
	private AudioClip[] _feetSounds;

	private RaycastHit _hit;

	private AudioSource _audioSource;

	//distance entre chaque bruit de pas
	private float timeBeforeNewClip;
	private float mouvementDemander;	//Temps entre le relachment de la touche et la fin des bruit de pas
	private JumpScript _jumpScript;

	public bool isStepClimbing { get; private set; }
	private GameObject stepRayLower;
	private GameObject stepRayUpper;

	void Start () {
		if (_rigidbody == null) {
			_rigidbody = this.GetComponent<Rigidbody> ();
		}
		_audioSource = GetComponent<AudioSource> ();
		timeBeforeNewClip = 2f;
		mouvementDemander = 2.5f;

		_jumpScript = GetComponent<JumpScript>();

		CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();


		isStepClimbing = false;
		stepRayLower = new GameObject("stepRayLower");
		stepRayLower.transform.SetParent(_transform);
		stepRayLower.transform.localPosition = new Vector3(0, 0.1f, capsuleCollider.radius * 0.9f);

		stepRayUpper = new GameObject("stepRayUpper");
		stepRayUpper.transform.SetParent(_transform);
		stepRayUpper.transform.localPosition = new Vector3(0, stepOff, capsuleCollider.radius * 0.9f);
	}
	
	void FixedUpdate () {
		
		var direction = (_transform.forward * Input.GetAxis("Vertical") + _transform.right * Input.GetAxis("Horizontal")).normalized;
		direction = direction.normalized;

		//Pente max egal a 30°
		bool isTropPentu = isPenteSup30(direction);

		applyMovements(direction, isTropPentu);

		stepClimbing(direction);
   	}

	private bool isPenteSup30(Vector3 direction)
    {
		bool result = false;
		if (direction != Vector3.zero && Physics.SphereCast(_transform.position + _transform.up * 1f, .001f, direction, out _hit, Mathf.Sqrt(3f)))
		{
			if (_hit.normal.y > .1f)
			{
				result = true;
			}
		}

		return result;
	}

	//TODO gestion différente en l'air ?
	private void applyMovements(Vector3 direction, bool isTropPentu)
    {
		var rigidVelocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _rigidbody.velocity.z);
		var speed = rigidVelocity - Vector3.Scale(_transform.up, rigidVelocity);

		if (_jumpScript.isInGround() &&(Vector3.zero == direction || isTropPentu))
		{
			if (speed.magnitude < _maxVelocity / 10)
			{
				//Arret personnage
				_rigidbody.velocity = rigidVelocity - speed;
			}
			else
			{
				//Deceleration
				var speeddelta = (-speed.normalized * _decelerationForce);
				_rigidbody.AddForce(speeddelta, ForceMode.Acceleration);
			}
		}
		else if (speed.magnitude < _maxVelocity)
		{
			var speeddelta = (direction * _accelerationForce);
			_rigidbody.AddForce(speeddelta, ForceMode.Acceleration);

		}
	}

	private void generateFootSound(Vector3 direction)
    {
		if (timeBeforeNewClip < 0)
		{
			playFootStepAudio(_rigidbody.velocity.magnitude);
			timeBeforeNewClip = 2f;
		}

		if (Vector3.zero != direction)
		{
			mouvementDemander = 2.5f;
		}
		else
		{
			mouvementDemander -= Time.deltaTime;
		}

		if (mouvementDemander > 0)
		{
			timeBeforeNewClip -= _rigidbody.velocity.magnitude * Time.deltaTime;
		}
	}

	private void playFootStepAudio(float speed)
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

	/**
	 * On vérifie si en face ou a 45° de la vu il y a une marche
	 */
	private void stepClimbing(Vector3 direction)
    {
		isStepClimbing = false;
		RaycastHit hitLower;
		Vector3 direction45 = Quaternion.AngleAxis(45, Vector3.up) * direction;
		Vector3 directionMinus45 = Quaternion.AngleAxis(-45, Vector3.up) * direction;

		if (Physics.Raycast(stepRayLower.transform.position, direction, out hitLower, 0.1f))
		{
			RaycastHit hitUpper;
			if (!Physics.Raycast(stepRayUpper.transform.position, direction, out hitUpper, 0.2f))
			{
				_rigidbody.position -= new Vector3(0f, -_stepSmooth, 0f);
				isStepClimbing = true;
			}
		}
		else if (Physics.Raycast(stepRayLower.transform.position, direction45, out hitLower, 0.1f))
		{

			RaycastHit hitUpper45;
			if (!Physics.Raycast(stepRayUpper.transform.position, direction45, out hitUpper45, 0.2f))
			{
				_rigidbody.position -= new Vector3(0f, -_stepSmooth, 0f);
				isStepClimbing = true;
			}
		}
		else if (Physics.Raycast(stepRayLower.transform.position, directionMinus45, out hitLower, 0.1f))
		{

			RaycastHit hitUpperMinus45;
			if (!Physics.Raycast(stepRayUpper.transform.position, directionMinus45, out hitUpperMinus45, 0.2f))
			{
				_rigidbody.position -= new Vector3(0f, -_stepSmooth, 0f);
				isStepClimbing = true;
			}
		}
	}
}
