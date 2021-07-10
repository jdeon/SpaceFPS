using UnityEngine;
using System.Collections;

public class TeteChercheuseInertie : MonoBehaviour {

	public Transform targetTransform;
	public float force;

	public ForceMode _forceMode = ForceMode.VelocityChange;

	public ConditionEventAbstract conditonRecher;
	public bool autoDestruct;
	public int delayAutoDestrucSiCondFaux;
	public GameObject animDestruct;

	private Rigidbody TargetRigidbody;
	private float delayAvantAutoDestruct;

	void Start() 
	{
		TargetRigidbody = GetComponent<Rigidbody>();
		delayAvantAutoDestruct = (float) delayAutoDestrucSiCondFaux;
		if (null != animDestruct) {
			animDestruct.SetActive (false);
		}
	}

	void Update () 
	{
		if(null == conditonRecher || conditonRecher.getIsActive()){
			transform.LookAt (targetTransform);
			TargetRigidbody.AddForce(transform.forward*force*Time.deltaTime,_forceMode );
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