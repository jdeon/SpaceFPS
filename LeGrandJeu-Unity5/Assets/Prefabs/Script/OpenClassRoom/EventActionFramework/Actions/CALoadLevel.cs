using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CALoadLevel : CustomActionScript {

	public string _levelName;
	public bool asynchrone;
	public bool withEcranChargement;
	public float minTimeAsynch;

	private static string levelChargement = "EcranChargement";

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{


		if (withEcranChargement) {
			PlayerPrefs.SetString (Constantes.PP_ECRAN_CHARGEMENT_LVL_A_CHARGER, _levelName);
			SceneManager.LoadScene (levelChargement);
			yield return null;
		} else if (asynchrone) {
			// The Application loads the Scene in the background at the same time as the current Scene.
			//This is particularly good for creating loading screens. You could also load the Scene by build //number.
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_levelName);
			float tempsRestant = minTimeAsynch;
			//Wait until the last operation fully loads to return anything
			while (!asyncLoad.isDone || tempsRestant > 0)
			{
				tempsRestant -= Time.deltaTime;
				yield return null;
			}
		} else {
			SceneManager.LoadScene (_levelName);
			yield return null;
		}

	}
}
