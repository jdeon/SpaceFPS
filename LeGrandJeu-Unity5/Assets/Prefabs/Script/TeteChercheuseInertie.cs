using UnityEngine;
using System.Collections;

public class TeteChercheuseInertie : MonoBehaviour {

	public Transform targetTransform;
	public float force;
	public float maxVelocity;
	public float inertie;

	public ForceMode _forceMode = ForceMode.VelocityChange;

	public ConditionEventAbstract conditonRecher;
	public bool autoDestruct;
	public int delayAutoDestrucSiCondFaux;
	public GameObject animDestruct;

	private Rigidbody rigidB;
	private float delayAvantAutoDestruct;

	void Start() 
	{
		rigidB = GetComponent<Rigidbody>();
		delayAvantAutoDestruct = (float) delayAutoDestrucSiCondFaux;
		if (null != animDestruct) {
			animDestruct.SetActive (false);
		}
	}

	void Update () 
	{
		if (maxVelocity > 0 && rigidB.velocity.magnitude > maxVelocity) {
			rigidB.velocity = rigidB.velocity * maxVelocity / rigidB.velocity.magnitude;
		}

		if(null == conditonRecher || conditonRecher.getIsActive()){
			transform.LookAt (targetTransform);
			rigidB.AddForce(transform.forward*force*Time.deltaTime,_forceMode );
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
			GameObject clone = GameObject.Instantiate (animDestruct, transform.position, transform.rotation);
			clone.SetActive(true);
			GameObject.Destroy(clone, 5f);
		}
		GameObject.Destroy (gameObject);
	}
}