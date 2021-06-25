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

		yield return new WaitForSeconds (1);

		AsyncOperation asyncLoad;
		if (null != ecranACharger && !"".Equals(ecranACharger)) {
			asyncLoad = SceneManager.LoadSceneAsync (ecranACharger);
			PlayerPrefs.SetString (Constantes.PP_ECRAN_CHARGEMENT_LVL_A_CHARGER, null);
		} else {
			asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
		}

		yield return chargementNextLevel (asyncLoad);
	}

	private IEnumerator chargementNextLevel (AsyncOperation asyncLoad)
	{
		
		//Wait until the last operation fully loads to return anything
		while (!asyncLoad.isDone) {
			Debug.Log ("Chargement à " + (asyncLoad.progress * 100) + "%");
			yield return null;
		}

	}
}
