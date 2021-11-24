using UnityEngine;
using System.Collections;

public class suivreParcour : MonoBehaviour, IActivable {

	public Parcours parcours;
	public float vitesseRelative = 1f;
	public bool isDestroyAtEnd = false;
	public bool actif = true;
	public float delayToDestroy;
	public bool modeCinematique; //diminue le temps entre les calculs pour éviter les sacades

	private float t;
	private Vector3 positionDepart;
	private Quaternion rotationDepart;
	private int etapeEnCours;
	private float tempsParcours = 0;
	private bool isRetour = false;
	private bool isFini = false;
	private Vector3 directionDebut;
	private bool isBeforeStart;
	private float precisionCalcul;
	private float precisionCalculInit;
	private float timeLastUpdate;

	void Awake(){
		isBeforeStart = true;
	}


	// Use this for initialization
	void Start () {
		t = 0;
		enabled = false;//limit les updates non necessaires
		timeLastUpdate = Time.fixedTime;

		if (modeCinematique) {
			setPrecisionCalcul(Time.deltaTime);
		} else if (precisionCalcul <= 0f) {
			setPrecisionCalcul(.02f);
		}

		if (object.Equals(Parcours.listDesParcours, null) || ! Parcours.listDesParcours.ContainsKey (parcours.nomParcour)) {
			parcours.initialisationParcours ();
		}
		this.parcours.IsInPlay = true;
		this.etapeEnCours = 0;
		for (int i = 0; i < parcours.listTempsPourProchaineEtape.Count-1; i++) {
			tempsParcours += parcours.listTempsPourProchaineEtape [i];
		}
		if (this.actif) {
			preparerLancementCoroutine ();
		}
		isBeforeStart = false;
	}

	void Update(){
		if (this.isFini){
			if (isDestroyAtEnd) {
				Destroy (gameObject, delayToDestroy);
			} else if (null != GetComponent<Rigidbody>()) {
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				GetComponent<Rigidbody>().isKinematic = true;
				enabled = false;
			}
		}
	}

	void OnEnable(){
		if (!isBeforeStart) {
			activate ();
		}
	}

	public bool getIsActif(){
		return this.actif;
	}

	public 	void activate (){
		if (!this.actif) {
			this.actif = true;
			preparerLancementCoroutine ();
		}
	}

	public void desactivate(){
		this.actif = false;
	}

	private void preparerLancementCoroutine(){
		this.positionDepart = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		this.rotationDepart = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
		Rigidbody objectRigidbody = GetComponent<Rigidbody>();
		if (null != objectRigidbody){
			objectRigidbody.isKinematic = true;
			objectRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		}
		this.directionDebut = (this.parcours.listEtapeTransform[0].position - this.transform.position).normalized;
		this.timeLastUpdate = Time.fixedTime;

		StartCoroutine ("suivreLeParvours");
	}

	// FixedUpdate is called once per fixed deltatime
	void FixedUpdate(){
		if(!this.isFini && this.actif && isRigidbodyLogic ()) {
			precisionCalcul = Time.fixedTime - timeLastUpdate;

			if (this.etapeEnCours == 0) {
				allerVersDebutParcours ();
			} else {
				suivreParcours ();
			}
			timeLastUpdate = Time.fixedTime;
		}
	}


	private IEnumerator suivreLeParvours () {
		while(!this.isFini && this.actif) {
			if (isRigidbodyLogic ()) {
				enabled = true; //reactivation pour fixedupdates
				yield return new WaitForFixedUpdate();
				continue;
			}

			enabled = false; //desactivation des updates pour les coroutines

			precisionCalcul = Time.fixedTime - timeLastUpdate;

			if (this.etapeEnCours == 0) {
				allerVersDebutParcours ();
			} else {
				suivreParcours ();
			}

			timeLastUpdate = Time.fixedTime;
			if (modeCinematique) {
				yield return null;
			} else {
				yield return new WaitForSeconds (precisionCalculInit);
			}
		}
			
		enabled = true; //reactivation des updates car fin de la coroutine

		yield return null;
	}


	private bool isRigidbodyLogic(){
		Rigidbody rigidB = GetComponent<Rigidbody>();
		return null != rigidB;// && !rigidB.isKinematic;
	}


	void allerVersDebutParcours(){
		//
		if (parcours.listTempsPourProchaineEtape[0] > 0 ) {

			//REM depuis unity 2020 on ne peut toucher au transform après le movePosition donc rotation en priorite
			if (parcours.isRotating)
			{
				transform.rotation = Quaternion.Slerp(this.rotationDepart, parcours.listEtapeTransform[0].rotation, t / parcours.listTempsPourProchaineEtape[0]);
			}

			Vector3 targetPosition = Vector3.Lerp(positionDepart, parcours.listEtapeTransform[0].position, t / parcours.listTempsPourProchaineEtape[0]);
			Rigidbody objectRigidbody = GetComponent<Rigidbody>();
			if (null != objectRigidbody)
            {
				objectRigidbody.MovePosition(targetPosition);
			} 
			else
            {
				transform.position = targetPosition;

			}
		} else {
			transform.position = this.parcours.listEtapeTransform [0].position;

			if (parcours.isRotating)
            {
				transform.rotation = parcours.listEtapeTransform[0].rotation;
			}
		}

		t += (vitesseRelative * precisionCalcul);
		if (t > parcours.listTempsPourProchaineEtape [0]) {
			this.etapeEnCours = 1;
		}
	}


	void suivreParcours(){
		float tempsSurEtapeEnCours = t;

		for (int i = 0; i<this.etapeEnCours; i++) {
			if (i < parcours.listTempsPourProchaineEtape.Count){
				tempsSurEtapeEnCours -= parcours.listTempsPourProchaineEtape [i];
			}
		}

		//Si le temps de l'étape est dépassé, on change d'étape sinon on bouge le point
		if (this.etapeEnCours < parcours.listTempsPourProchaineEtape.Count && tempsSurEtapeEnCours >= parcours.listTempsPourProchaineEtape [this.etapeEnCours]) {
			tempsSurEtapeEnCours -= parcours.listTempsPourProchaineEtape [this.etapeEnCours];
			this.etapeEnCours++;
		}
		//Si le temps est négatif il s'agit d'un retour on diminue alors l'etape 
		else if (tempsSurEtapeEnCours < 0) {
			tempsSurEtapeEnCours += parcours.listTempsPourProchaineEtape [this.etapeEnCours];
			this.etapeEnCours--;
		} 

		switch (parcours.modeDeBouclage) {

		case "AllerSimple":
			if (this.etapeEnCours < parcours.getNbEtape()){
				parcours.parcourirEtape (transform, GetComponent<Rigidbody>(), tempsSurEtapeEnCours,this.etapeEnCours);
				t += vitesseRelative * precisionCalcul;
			} else {
				this.isFini = true;
			}
			break;

		case "AllerRetour":
			//Fin de l'aller
			if (this.etapeEnCours >= parcours.getNbEtape() && !isRetour){
				this.etapeEnCours = parcours.getNbEtape() - 1;
				isRetour=true; 
			} //Fin du retour
			else if(this.etapeEnCours == 0 && isRetour){
				this.etapeEnCours = 1;
				isRetour=false; 
			}

			parcours.parcourirEtape (transform, GetComponent<Rigidbody>(), tempsSurEtapeEnCours, this.etapeEnCours);

			if (!isRetour){
				t += vitesseRelative * precisionCalcul;
			} else {
				t -= vitesseRelative * precisionCalcul;
			}
			break;

		case "BoucleTeleport":
			if (this.etapeEnCours < parcours.getNbEtape()){
				parcours.parcourirEtape (transform, GetComponent<Rigidbody>(), tempsSurEtapeEnCours, this.etapeEnCours);
				t += vitesseRelative * precisionCalcul;
			} else {
				this.etapeEnCours = 1;
				t = parcours.listTempsPourProchaineEtape[0];
			}
			break;

		case "Boucle": 
			if (this.etapeEnCours < parcours.getNbEtape()){
				parcours.parcourirEtape (transform, GetComponent<Rigidbody>(), tempsSurEtapeEnCours, this.etapeEnCours);
				t += vitesseRelative * precisionCalcul;
			} else {
				this.etapeEnCours = 1;
				t = parcours.listTempsPourProchaineEtape[0];
			}
			break;
		}
	}

	public bool getIsFini(){
		return this.isFini;
	}

	public void setPrecisionCalcul(float precisionCalcul){
		this.precisionCalcul = precisionCalcul;
		this.precisionCalculInit = precisionCalcul;
	}
}
