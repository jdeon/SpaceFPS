using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GestionObjetSurController : MonoBehaviour {
	
	public Vector3 tailleMax = new Vector3(.5f,1f,.5f);

	private Transform controllerTransform;
	private Transform detecteurTransform;
	private Vector3 positionObjetDeBase;
	private bool isModeVisee;
	private bool transfertEnCours;

	

	// Use this for initialization
	void Start () {
		controllerTransform = transform;
		isModeVisee = false;
		transfertEnCours = false;
	}

	void Update () {
		if (Input.GetMouseButtonDown (1) && transform.Find(Constantes.STR_HEAD_CONTROLLER).Find(Constantes.STR_TRANSFORM_ARME).childCount > 0) {
			Transform transfHeadController = transform.Find(Constantes.STR_HEAD_CONTROLLER);
			Transform transfArme = transfHeadController.Find(Constantes.STR_TRANSFORM_ARME).GetChild(0);
			if(!isModeVisee){
				transfArme.rotation = transfHeadController.rotation;
				transfArme.localPosition = -transfArme.parent.localPosition + new Vector3(0f,-.05f,0.3f);
				transfHeadController.parent.GetComponent<MoveScript>().enabled = false;
                this.isModeVisee =true;

			} else {
				transfArme.position = transfArme.parent.position;
				transfArme.rotation = transfArme.parent.rotation;
				transfHeadController.parent.GetComponent<MoveScript>().enabled = true;
                this.isModeVisee = false;
			}
		}
	}

	/**Quand le controller entre dans le collider d'un objet on verifie si c'est un objet portable ou une zone de détection
	 **/
	void OnTriggerEnter(Collider other) {
		Transform destinationObjetTransform = null;
        Transform objetTransform = null;

		//Si c'est une zone de prise d'objet, on regarde s'il y a de la place dans une de mains (mains droite en priorité) 
		//puis on lance la coroutine saisirObjet
		if (!this.transfertEnCours && other.gameObject.tag == Constantes.TAG_ZONE_PRISE_OBJET_PORTABLE) {
			detecteurTransform = other.gameObject.transform;
            objetTransform = detecteurTransform.parent;
            this.positionObjetDeBase = new Vector3 (objetTransform.position.x, objetTransform.position.y, objetTransform.position.z);

			if (!(controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_DROITE) == null) && controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_DROITE).childCount == 0) {
                destinationObjetTransform = controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_DROITE);
			} else if (!(controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_GAUCHE) == null) && controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_GAUCHE).childCount == 0) {
                destinationObjetTransform = controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_GAUCHE);
			}

			StartCoroutine (saisirObjet(destinationObjetTransform,objetTransform));
		} 

		//Si c'est une zone de depot d'objet, on regarde qu'elle main tien un objet et s'il est du meme type que le détecteur (main gauche en priorité) 
		//puis si un objet est trouvé, on lance la coroutine deposerObjet avec la taille initiale de l'objet
		else if (!this.transfertEnCours && other.gameObject.tag == Constantes.TAG_ZONE_DEPOT_OBJET_PORTABLE)
		{
			string[] listTypeDetection = other.gameObject.name.Split('_');			//[0] sera toujours "detecteur" 
			bool objetTrouve = false;

			if (!objetTrouve && controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_GAUCHE) != null && controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_GAUCHE).childCount != 0 ) {
				Transform conteneurMainG = controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_GAUCHE);
				for (int numChild = 0 ; numChild < conteneurMainG.childCount ; numChild++){
					ObjetPortable objetPortable  = (ObjetPortable)conteneurMainG.GetChild(numChild).GetComponent<ObjetPortable> ();
					if (null != objetPortable && objetPortable.isDeposable() && objetPortable.isSameTypeObjectDetector(listTypeDetection)){
						objetTransform = conteneurMainG.GetChild(numChild).transform;
						objetTrouve = true;
						break;
					}
				}
			} 

			if (!objetTrouve && controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_DROITE) != null &&controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_DROITE).childCount != 0) {
				Transform conteneurMainD = controllerTransform.Find (Constantes.STR_OBJET_A_PORTER+"/"+ Constantes.STR_MAIN_DROITE);
				for (int numChild = 0 ; numChild < conteneurMainD.childCount ; numChild++){
					ObjetPortable objetPortable  = (ObjetPortable)conteneurMainD.GetChild(numChild).GetComponent<ObjetPortable> ();
					if (null != objetPortable && objetPortable.isDeposable() && objetPortable.isSameTypeObjectDetector(listTypeDetection)){
						objetTransform = conteneurMainD.GetChild(numChild).transform;
						objetTrouve = true;
						break;
					}
				}
			}

			if (objetTransform != null){
				StartCoroutine (deposerObjet(other.gameObject.transform, objetTransform, other.gameObject.transform.parent));
			}
		}
	}

	/**Si il y a de la place dans une mains et que l'objet possède le script ObjetPortable, on le déplace depuis sa position vers la mains de destination
	 * on prend en compte la position, la rotation et la taille si elle dépasse la taille maximal
	 * Ensuite on met la mains comme parent de l'objer et on désctive la zone de saisi
	 * Sinon, on affiche qu'il n'y a plus de place disponible
	 * */
	IEnumerator saisirObjet (Transform destinationObjetTransform, Transform objetTransform){
		if (null != destinationObjetTransform && null != objetTransform.GetComponent<ObjetPortable>()) {
			float tempsPourAttraper = objetTransform.GetComponent<ObjetPortable>().tempsPourAttraperDeposer;
			float step = (objetTransform.position - destinationObjetTransform.position).magnitude * tempsPourAttraper * Time.deltaTime;
			float tempsRestant = tempsPourAttraper;

			this.transfertEnCours = true;
			while (tempsRestant>0) {
				float proportion = 1 - Vector3.Distance (objetTransform.position, destinationObjetTransform.position) / Vector3.Distance (this.positionObjetDeBase, destinationObjetTransform.position);
				Vector3 tailleObjetTemp = objetTransform.localScale;
				
				objetTransform.position = Vector3.MoveTowards (objetTransform.position, destinationObjetTransform.position, step);
				objetTransform.rotation = Quaternion.Slerp (objetTransform.rotation, destinationObjetTransform.rotation, step);
				
				if (this.tailleMax.x < objetTransform.localScale.x) { 
					tailleObjetTemp.x = objetTransform.localScale.x + proportion * (this.tailleMax.x - objetTransform.localScale.x);
				}
				if (this.tailleMax.y < objetTransform.localScale.y) { 
					tailleObjetTemp.y = objetTransform.localScale.y + proportion * (this.tailleMax.y - objetTransform.localScale.y);
				}
				if (this.tailleMax.z < objetTransform.localScale.z) { 
					tailleObjetTemp.z = objetTransform.localScale.z + proportion * (this.tailleMax.z - objetTransform.localScale.z);
				}

                objetTransform.localScale = tailleObjetTemp;
				
				tempsRestant -= Time.deltaTime;
				yield return null;
			}

			objetTransform.position = destinationObjetTransform.position;
			objetTransform.rotation = destinationObjetTransform.rotation;
            objetTransform.SetParent (destinationObjetTransform);
			detecteurTransform.gameObject.SetActive(false);
			this.transfertEnCours = false;
		} else {
			//StartCoroutine(ShowMessage("Vous ne pouvez plus rien porter", 2));
		}
	}

	/**On déplace l'objet vers la zone de dépot en prennant en compte la position, la rotation et la taille initial de l'objet
	 * Ensuite on met la zone de dépot comme parent de l'objer et on désctive la zone de depot
	 * */
	IEnumerator deposerObjet (Transform zoneDetection, Transform objetTransform, Transform destinationObjetTransform){
		ObjetPortable objetPortable = objetTransform.GetComponent<ObjetPortable> ();

		Vector3 tailleInitialObjet = objetPortable.getTailleInitial();
		float tempsPourDeposer = objetPortable.tempsPourAttraperDeposer;

		float step = (objetTransform.position - destinationObjetTransform.position).magnitude * tempsPourDeposer * Time.deltaTime;
		float tempsRestant = tempsPourDeposer;

		this.transfertEnCours = true;
		desactiverEffetVisuel (destinationObjetTransform);
		objetTransform.SetParent (destinationObjetTransform);

		while (tempsRestant>0) {
			
			objetTransform.position = Vector3.MoveTowards (objetTransform.position, destinationObjetTransform.position, step);
			objetTransform.rotation = Quaternion.Slerp (objetTransform.rotation, destinationObjetTransform.rotation, step);
			objetTransform.localScale = Vector3.MoveTowards(objetTransform.localScale, tailleInitialObjet, step);
			
			tempsRestant -= Time.deltaTime;
			yield return null;
		}

		objetPortable.changerEtat();
		zoneDetection.gameObject.SetActive(false);
		this.transfertEnCours = false;
	}

	private void desactiverEffetVisuel (Transform destinationObjetTransform){
		for (int numChild = 0; numChild < destinationObjetTransform.childCount; numChild++) {
			if (destinationObjetTransform.GetChild (numChild).name == Constantes.STR_EFFET_VISUEL) {
				destinationObjetTransform.GetChild (numChild).gameObject.SetActive (false);
				break;
			}
		}
	}





	/**Affiche le message en parametre durant le temps en parametre
	 * */
	IEnumerator ShowMessage (string message, float delay) {
		Text text = GetComponent<Text>();
		text.text = message;
		text.enabled = true;
		yield return new WaitForSeconds(delay);
		text.enabled = false;
	}	
}
