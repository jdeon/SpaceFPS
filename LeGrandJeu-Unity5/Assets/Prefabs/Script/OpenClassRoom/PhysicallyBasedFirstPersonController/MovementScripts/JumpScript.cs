using UnityEngine;
using System.Collections;

public class JumpScript : MonoBehaviour {

	[SerializeField]
	private float _feetRadius;

	[SerializeField]
	private Transform _transform;

	[SerializeField]
	private float _distanceTrigger;

	[SerializeField]
	private float _jumpSpeed;

	[SerializeField]
	private Rigidbody _rigidbody;

	[SerializeField]
	private AudioClip _jumpSound;

	[SerializeField]
	private AudioClip _laundingSound;

	private bool _canJump = true;
	private Collider _ground;

	private AudioSource _audioSource;

	private RaycastHit _hit;

	private MoveScript _moveScript;

	void Start(){
		_audioSource = GetComponent<AudioSource> ();
		_moveScript = GetComponent<MoveScript> ();

	}


	void FixedUpdate () {
		processInGround();

		//Evite les saut constant
		if (!_canJump && Input.GetAxis ("Jump") == 0 && isInGround()) {
			if (_laundingSound != null) {
				_audioSource.clip = _laundingSound;
				_audioSource.PlayOneShot (_audioSource.clip);
			}
			_canJump = true;
		}

		if (_canJump && Input.GetAxis("Jump") > 0 && !_moveScript.isSlopeTooSteep && (isInGround() || _moveScript.isStepClimbing))
		{
			_rigidbody.AddForce(_transform.up * _jumpSpeed, ForceMode.VelocityChange);
			if (_jumpSound != null) {
				_audioSource.clip = _jumpSound;
				_audioSource.PlayOneShot (_audioSource.clip);
			}
			_canJump = false;
		}
	}

	private void processInGround(){
		if (Physics.SphereCast (_transform.position + _transform.up * _feetRadius * 1.1f, _feetRadius, _transform.up * -1f, out _hit, 0.1f) && !_hit.collider.isTrigger) {
			this._ground = _hit.collider;
		} else {
			this._ground = null;
		}
	}

	public Collider getGround()
	{
		return this._ground;
	}

	public bool isInGround()
	{
		return null != this._ground;
	}
}