using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestionCheckpoint : MonoBehaviour {

	public GameObject objGroupCheckpoint;
	public int numLevelActuel;

	private CheckPoint checkPointActuel;
	private string affichage;
	private int numNiveau;

	// Use this for initialization
	void Start () {
		affichage = "";
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
			respawnCheckPoint ();
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
		if(null == this.checkPointActuel)
        {
			this.checkPointActuel = objGroupCheckpoint.GetComponentInChildren<CheckPoint>();
		}


		Transform transScripDeRespawn = this.checkPointActuel.transform.Find ("checkPointWhenRespawn");
		StartCoroutine (lancerScriptCheckpoint(transScripDeRespawn, 0f));
		teleportController (this.checkPointActuel);
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

	public void setCheckPointActuel(CheckPoint checkPointActuel){
		this.checkPointActuel = checkPointActuel;
	}
}
