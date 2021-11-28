using UnityEngine;
using System.Collections;

public class TeleportEnemyTueur : MonoBehaviour, IResetable, IActivable {

	public float speed;
	public Transform target;
	public bool gererDeplacement = true;

	public bool autoDestruct;
	public float timeBeforeAutoDestruct;

	private Vector3 positionInitial;
	private Quaternion rotationInitial;
	private bool isActif;
	private float timeToAutodestruct;
	// Use this for initialization
	void Start () {
		positionInitial = transform.position;
		rotationInitial = transform.rotation;
		isActif = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActif && gererDeplacement) {
			transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
			transform.Translate (0, 0, speed * Time.deltaTime);
		}

		if (isActif && autoDestruct)
        {
			if(UtilsTargetable.isTargetAtteignable(transform.position, target.position, 10))
            {
				timeToAutodestruct = 0;

			} 
			else
            {
				if (timeToAutodestruct > timeBeforeAutoDestruct)
				{
					GameObject.Destroy(gameObject);
				}
				else
				{
					timeToAutodestruct += Time.deltaTime;
				}
			}
        }
	}

	void OnTriggerEnter(Collider other) {
		GestionSystemVieController gestionVieController = other.GetComponent<GestionSystemVieController> ();
		if (null != gestionVieController && isActif) {//Faut il ajouter un raycast ?
			gestionVieController.tue();
		}
	}

	public void reset(){
		transform.position = positionInitial;
		transform.rotation = rotationInitial;
		isActif = false;
		Transform spotLight = transform.Find ("Spotlight");
		if (null != spotLight && null != spotLight.GetComponent<Light>()) {
			spotLight.GetComponent<Light>().color = Color.green ;
			
		}
	}

	public void activate(){
		this.isActif = true;
		Transform spotLight = transform.Find ("Spotlight");
		if (null != spotLight && null != spotLight.GetComponent<Light>()) {
			spotLight.GetComponent<Light>().color = Color.red ;

		}
	}

	public void desactivate(){
		this.isActif = false;
		Transform spotLight = transform.Find ("Spotlight");
		if (null != spotLight && null != spotLight.GetComponent<Light>()) {
			spotLight.GetComponent<Light>().color = Color.green ;	
		}
	}

	public bool getIsActif(){
		return isActif;
	}
}
