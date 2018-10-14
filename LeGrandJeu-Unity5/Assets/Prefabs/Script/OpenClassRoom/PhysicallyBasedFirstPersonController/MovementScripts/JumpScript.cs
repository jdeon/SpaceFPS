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

	private AudioSource _audioSource;

	private RaycastHit _hit;

	void Start(){
		_audioSource = GetComponent<AudioSource> ();

	}


	void FixedUpdate () {
		//Evite les saut constant
		if (!_canJump && Input.GetAxis ("Jump") == 0 && inGround ()) {
			if (_laundingSound != null) {
				_audioSource.clip = _laundingSound;
				_audioSource.PlayOneShot (_audioSource.clip);
			}
			_canJump = true;
		}

		if (_canJump && Input.GetAxis("Jump") > 0 && inGround())
		{
			_rigidbody.AddForce(_transform.up * _jumpSpeed, ForceMode.VelocityChange);
			if (_jumpSound != null) {
				_audioSource.clip = _jumpSound;
				_audioSource.PlayOneShot (_audioSource.clip);
			}
			_canJump = false;
		}
    }

	private bool inGround(){
		return Physics.SphereCast (_transform.position + _transform.up * _feetRadius * 1.1f, _feetRadius, _transform.up * -1f, out _hit, 0.1f) && !_hit.collider.isTrigger;
	}
}
