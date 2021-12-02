using UnityEngine;
using System.Collections;

public class TableauDeCommandeClickActivateur : MonoBehaviour {

	public Transform[] listLieuExplosionGeneral;
	public GameObject explosionIndividuelle;
	public GameObject explosionGeneral;

	private bool isControllerIn;
	private bool isReady;
	private int numEtape;

	private PlayerInputAction controller;

	void Awake()
	{
		controller = new PlayerInputAction();
		controller.PlayerActions.Use.performed += ctx => {
			OnUse();
		};
	}

	private void OnEnable()
	{
		controller.Enable();
	}

	private void OnDisable()
	{
		controller.Disable();
	}

	void Start () {
		isReady = false;
		isControllerIn = false;
		numEtape = 0;
	}

	void OnUse() {
		if (isReady && isControllerIn) {
			Transform[] tabTransformObjetAActiver = transform.parent.Find("ToucheActivable").GetComponent<TableauDeCommandeZoneActivateur>().getTabTranformObjetLie();

			Transform lieuExplosionGeneral = null;

			for(int numTransform = 0 ; numTransform < tabTransformObjetAActiver.Length; numTransform++){
				Transform transformObjet = tabTransformObjetAActiver[numTransform];
				GameObject explosionIndividuelleClone = (GameObject) Instantiate(explosionIndividuelle,transformObjet.position,transformObjet.rotation); 
				explosionIndividuelleClone.SetActive(true);
				Destroy(transformObjet.gameObject);
				Destroy(explosionIndividuelleClone, 3f);
			}
			if(numEtape < listLieuExplosionGeneral.Length){
				lieuExplosionGeneral = listLieuExplosionGeneral[numEtape];
				numEtape++;
			} else if (listLieuExplosionGeneral.Length == 1){
				lieuExplosionGeneral = listLieuExplosionGeneral[0];
			}

			if(lieuExplosionGeneral != null){
				GameObject explosionGeneralClone = (GameObject) Instantiate(explosionGeneral,lieuExplosionGeneral.position,lieuExplosionGeneral.rotation); 
				explosionGeneralClone.SetActive(true);
				Destroy(explosionGeneralClone, 3f);
			}
		}
	}


	void onTriggerEnter(Collider other){
		if (other.gameObject.name == Constantes.STR_CONTROLLER_OCR_AMELIORE) {
			isControllerIn = true;
		}
	}

	void onTriggerExit(Collider other){
		if (other.gameObject.name == Constantes.STR_CONTROLLER_OCR_AMELIORE) {
			isControllerIn = false;
		}
	}

	public void setIsReady(bool isReady){
		this.isReady = isReady;
	}
}
