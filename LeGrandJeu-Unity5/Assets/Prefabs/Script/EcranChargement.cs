using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EcranChargement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (chargementNextLevel ());
	}
	
	public IEnumerator chargementNextLevel ()
	{
		string ecranACharger = PlayerPrefs.GetString (Constantes.PP_ECRAN_CHARGEMENT_LVL_A_CHARGER);

		if (null != ecranACharger) {
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (ecranACharger);
			//Wait until the last operation fully loads to return anything
			while (!asyncLoad.isDone) {
				yield return null;
			}
		} else {
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
			while (!asyncLoad.isDone) {
				yield return null;
			}
		}
	}
}
