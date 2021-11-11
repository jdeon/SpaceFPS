using UnityEngine;
using System.Collections;

public class TeteChercheuseInertie : MonoBehaviour {

	public Transform targetTransform;
	public float force;
	public float maxVelocity;

	public ForceMode _forceMode = ForceMode.VelocityChange;

	public ConditionEventAbstract conditonRecher;
	public bool autoDestruct;
	public int delayAutoDestrucSiCondFaux;
	public GameObject animDestruct;

	private Rigidbody targetRigidbody;
	private float delayAvantAutoDestruct;

	void Start() 
	{
		targetRigidbody = GetComponent<Rigidbody>();
		delayAvantAutoDestruct = (float) delayAutoDestrucSiCondFaux;
		if (null != animDestruct) {
			animDestruct.SetActive (false);
		}
	}

	void Update () 
	{
		if (maxVelocity > 0 && targetRigidbody.velocity.magnitude > maxVelocity) {
			targetRigidbody.velocity = targetRigidbody.velocity * maxVelocity / targetRigidbody.velocity.magnitude;
		}

		if(null == conditonRecher || conditonRecher.getIsActive()){
			transform.LookAt (targetTransform);
			targetRigidbody.AddForce(transform.forward*force*Time.deltaTime,_forceMode );
			delayAvantAutoDestruct = (float) delayAutoDestrucSiCondFaux;
		} else if (autoDestruct){
			delayAvantAutoDestruct -= Time.deltaTime;
		}

		if (delayAvantAutoDestruct < 0) {
			destructionProjectile ();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		destructionProjectile ();
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == Constantes.LAYER_CONTROLLER)
		{
			destructionProjectile ();
		} 
	}


	private void destructionProjectile(){
		if (null != animDestruct) {
			GameObject.Instantiate (animDestruct, transform.position, transform.rotation);
		}
		GameObject.Destroy (gameObject);
	}
}