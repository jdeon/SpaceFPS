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
	private float _waitToJump = 0f;
	private Collider _ground;
	private Vector3 _groundNormal;

	private AudioSource _audioSource;

	private RaycastHit _hit;

	private MoveScript _moveScript;
	private ContrainteController _contrainteController;

	void Start(){
		_audioSource = GetComponent<AudioSource> ();
		_moveScript = GetComponent<MoveScript> ();
		_contrainteController = GetComponent<ContrainteController>();
	}


	void FixedUpdate () {
		processInGround();

		if (_waitToJump > 0f) {
			_waitToJump -= Time.deltaTime;
		}

		//Evite les saut constant
		if (!_canJump && isInGround()) {
			if (_laundingSound != null) {
				_audioSource.clip = _laundingSound;
				_audioSource.PlayOneShot (_audioSource.clip);
			}
			_canJump = true;
		}
	}

	void OnJump()
    {
		if(null != _contrainteController && !_contrainteController.canMove)
        {
			return;
        }

		if(null == _rigidbody)
        {
			_rigidbody = GetComponent<Rigidbody>();
		}

		if (null != _rigidbody && _canJump)
		{
			Vector3 jumpDirection = Vector3.zero;

			if(!_groundNormal.Equals(Vector3.zero) && isInGround())
            {
				//Direction moyenne transform up et normal)
				jumpDirection = (_groundNormal + _transform.up).normalized;

			}
			else if (_moveScript.isStepClimbing)
            {
				jumpDirection = _transform.up;
			}

            if (jumpDirection.Equals(Vector3.zero))
            {
				return;
            }

			_rigidbody.AddForce(jumpDirection * _jumpSpeed, ForceMode.VelocityChange);
			if (_jumpSound != null)
			{
				_audioSource.clip = _jumpSound;
				_audioSource.PlayOneShot(_audioSource.clip);
			}
			_canJump = false;
			_waitToJump = 0.5f;
		}
	}

	private void processInGround(){
		if (Physics.SphereCast (_transform.position + _transform.up * _feetRadius * 1.05f, _feetRadius, _transform.up * -1f, out _hit, 0.1f) && !_hit.collider.isTrigger) {
			this._ground = _hit.collider;
			this._groundNormal = _hit.normal;
		} else {
			this._ground = null;
			this._groundNormal = Vector3.zero;
		}
	}

	public Collider getGround()
	{
		return this._ground;
	}

	public bool isInGround()
	{
		return _waitToJump <= 0 && null != this._ground;
	}
}