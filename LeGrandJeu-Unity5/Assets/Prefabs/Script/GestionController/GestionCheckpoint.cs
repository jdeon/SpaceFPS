using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestionCheckpoint : MonoBehaviour {

	public GameObject objGroupCheckpoint;
	public int numLevelActuel;
	public int debbugCheckPoint;//FIXME a supprimer

	private CheckPoint checkPointActuel;
	private string affichage;
	private int numNiveau;

	// Use this for initialization
	void Start () {
		loadAtCheckPoint ();
		affichage = "";

		if (null == this.checkPointActuel) {
			//On prend toute les checkpoint inferieur pour remplir la list
			for (int numChild = 0; numChild < objGroupCheckpoint.transform.childCount; numChild++) {
				Transform actualChild = objGroupCheckpoint.transform.GetChild (numChild);
				CheckPoint checkPoint = actualChild.GetComponent<CheckPoint> ();
				if (null != checkPoint && checkPoint.actif) {
					checkPointActuel = checkPoint;
				}
			}
		}
	}

	public void OnGUI()
	{
		if (null != affichage && affichage != "")
		{
			Rect rect = new Rect (Screen.width * (1f - .1f) / 2f, Screen.height * (1f - .05f) / 2f, Screen.width * .1f, Screen.height * .05f);
			GUI.skin.textArea.fontSize = Mathf.RoundToInt(Screen.width / 100f);
			GUI.TextArea(rect, affichage);
		}
	}

	public void OnTriggerEnter(Collider other) {
		GameObject objOther = (other.gameObject);
		if (objOther.tag == Constantes.TAG_RESPAWN) {
			if (null == this.checkPointActuel) {
				loadAtCheckPoint ();
			} else {
				respawnCheckPoint ();
			}
		} else if (objOther.GetComponent<CheckPoint> () != null) {
			CheckPoint futurCheckpoint = objOther.GetComponent<CheckPoint> ();
			if (null == this.checkPointActuel || (futurCheckpoint.checkPointDoubleSens && this.checkPointActuel.checkPointDoubleSens  && this.checkPointActuel.getIdCheckPoint() != futurCheckpoint.getIdCheckPoint()) || (futurCheckpoint.getIdCheckPoint() > this.checkPointActuel.getIdCheckPoint())) {
				this.checkPointActuel = futurCheckpoint;
				StartCoroutine (affichageCheckPoint());
				if (this.checkPointActuel.checkPointDeSauvegarde) {
					string nivEtape = mapActualCheckPointToText(this.numLevelActuel, this.checkPointActuel.getIdCheckPoint());
					PlayerPrefs.SetString (PlayerPrefs.GetString(Constantes.PP_JOUEUR_COURANT), nivEtape);
				}
			}
		}
	}

	/**
	 * map value to save format : lvl_???_checkP_???
	 * */
	public static string mapActualCheckPointToText(int numLevel, int numCheckPoint){
		return Constantes.PP_LEVEL + "_" + numLevel + "_" + Constantes.PP_CHECKPOINT + "_" + numCheckPoint;
	}

	/**
	 * map format : lvl_???_checkP_??? to real value
	 * */
	public static void mapSaveCheckpointDataToInt(string saveText, out int numLevel, out int numCheckPoint){
		numLevel = 0;
		numCheckPoint = -1;

		if (null != saveText) {
			string[] tabInfoEtape = saveText.Split ('_');

			if (tabInfoEtape.Length == 4) {
				int.TryParse (tabInfoEtape [1], out numLevel);
				int.TryParse (tabInfoEtape [3], out numCheckPoint);
			}
		}
	}

	/**
	 * Cette méthode est appelé quand le controler tombe à travers une zone de respawn
	 * */
	public void respawnCheckPoint(){
		Transform transScripDeRespawn = this.checkPointActuel.transform.Find ("checkPointWhenRespawn");
		StartCoroutine (lancerScriptCheckpoint(transScripDeRespawn, 0f));
		teleportController (this.checkPointActuel);
	}

	/**
	 * Cette méthode est appelée au chargement du niveau
	 * */
	public void loadAtCheckPoint(){
		List<Transform> listCheckpointALancer = new List<Transform>();
		int actualCheckPoint = -1;
		string etapeActuel = PlayerPrefs.GetString (PlayerPrefs.GetString(Constantes.PP_JOUEUR_COURANT)); 


		if (debbugCheckPoint >= 0) {
			//FIXME to delete
			numNiveau = numLevelActuel;
			actualCheckPoint = debbugCheckPoint;
		} else {
			mapSaveCheckpointDataToInt (etapeActuel, out numNiveau, out actualCheckPoint);
		}

		if (numLevelActuel == numNiveau) {
			//On prend toute les checkpoint inferieur pour remplir la list
			for (int numChild = 0; numChild < objGroupCheckpoint.transform.childCount; numChild++) {
				Transform actualChild = objGroupCheckpoint.transform.GetChild (numChild);
				//format checkpointNum_??? actualChild.name.Split ('_')
				int numCheckpoint = actualChild.transform.GetSiblingIndex ();
				if (actualChild.name.Split ('_').Length > 1 && numCheckpoint <= actualCheckPoint) {
					while (listCheckpointALancer.Count < numCheckpoint) {
						listCheckpointALancer.Add (null);
					}
					listCheckpointALancer.Insert (numCheckpoint, actualChild);
				}
			}

			//Jouer le script de chargement de tous les checkpoints inferieurs
			float delayBeforeLoad = 0f;
			foreach (Transform tranfCheckpoint in listCheckpointALancer) {
				if (null != tranfCheckpoint) {
					StartCoroutine (lancerScriptCheckpoint (tranfCheckpoint.Find ("checkPointToLoaded"), delayBeforeLoad));
					delayBeforeLoad += 0.01f;
				}
			}
			if (listCheckpointALancer.Count > 0) {
				teleportController (listCheckpointALancer [listCheckpointALancer.Count - 1].gameObject.GetComponent<CheckPoint> ());
			}
		}
	}
		
	private IEnumerator lancerScriptCheckpoint(Transform transformCheckpointALancer, float delayBeforeLoad){
		if (null != transformCheckpointALancer) {
			if (delayBeforeLoad > 0) {
				yield return new WaitForSeconds (delayBeforeLoad);
			}

			ConditionEventAbstract conditionAActiver = transformCheckpointALancer.GetComponent<ConditionEventAbstract> ();
			if (null != conditionAActiver) {
				conditionAActiver.activeEvent ();
			}
		}
		yield return null;
	}

	/**
	 * Telport au checkpoint
	 * */
	private void teleportController(CheckPoint checkPoint){
		if (null != checkPoint) {
			this.transform.position = checkPoint.transformRespawn.position;
			this.transform.rotation = checkPoint.transformRespawn.rotation;
			Rigidbody rigidController = this.gameObject.GetComponent<Rigidbody> ();
			if (null != rigidController) {
				rigidController.velocity = Vector3.zero;
			}
		}
	}

	private IEnumerator affichageCheckPoint(){
		affichage = this.checkPointActuel.checkPointDeSauvegarde ? "SavePoint" : "CheckPoint";
		yield return new WaitForSeconds(2f);
		affichage = "";
	}
}
