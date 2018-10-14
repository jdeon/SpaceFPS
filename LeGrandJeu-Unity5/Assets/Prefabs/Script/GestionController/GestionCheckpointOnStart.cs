using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//A utiliser uniquement si le controller est désactivé au début de la scène
public class GestionCheckpointOnStart : MonoBehaviour {

	public GameObject objGroupCheckpoint;
	public int numLevel;
	public GameObject controller;

	private CheckPoint checkPointActuel;
	private int numNiveau;

	// Use this for initialization
	void Start () {
		loadAtCheckPoint ();

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

	/**
	 * Cette méthode est appelée au chargement du niveau
	 * */
	public void loadAtCheckPoint(){
		List<Transform> listCheckpointALancer = new List<Transform>();
		int actualCheckPoint = -1;
		string etapeActuel = PlayerPrefs.GetString (PlayerPrefs.GetString(Constantes.PP_JOUEUR_COURANT)); //format : lvl_???_checkP_???
		string[] tabInfoEtape = etapeActuel.Split ('_');
		numNiveau = 0;
		int.TryParse(tabInfoEtape[1], out numNiveau);

		if (numLevel == numNiveau) {
			int.TryParse (tabInfoEtape [3], out actualCheckPoint);

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
			foreach (Transform tranfCheckpoint in listCheckpointALancer) {
				if (null != tranfCheckpoint) {
					StartCoroutine (lancerScriptCheckpoint (tranfCheckpoint.Find ("checkPointToLoaded")));
				}
			}
			if (listCheckpointALancer.Count > 0) {
				teleportController (listCheckpointALancer [listCheckpointALancer.Count - 1].gameObject.GetComponent<CheckPoint> ());
			}
		}
	}

	private IEnumerator lancerScriptCheckpoint(Transform transformCheckpointALancer){
		if (null != transformCheckpointALancer) {
			/*transformCheckpointALancer.gameObject.SetActive (true);
			yield return new WaitForSeconds (2f);
			transformCheckpointALancer.gameObject.SetActive (false);*/

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
		if (null != checkPoint && null != controller) {
			controller.transform.position = checkPoint.transformRespawn.position;
			controller.transform.rotation = checkPoint.transformRespawn.rotation;
			controller.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}
}
