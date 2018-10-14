using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionPause : MonoBehaviour {

	public GameObject goEcran;

	private bool inPause;
	private bool changeEtat;

	// Use this for initialization
	void Start () {
		inPause = false;
		changeEtat = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (changeEtat) {

			/*AudioListener[] listAudio =  GameObject.FindObjectsOfType<AudioListener> () ;

			foreach(AudioListener thisAudio in listAudio){
				Debug.Log (thisAudio.gameObject.name);
			}*/


			if (inPause) {
				goEcran.SetActive (inPause);
				Cursor.visible = true;
				Time.timeScale = 0;
			} else {
				goEcran.SetActive (inPause);
				Cursor.visible = false;
				Time.timeScale = 1;
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			resumeGame ();
		}
	}

	public void resumeGame(){
		inPause = !inPause;
		changeEtat = true;
	}

	public void goToMenu(){
		SceneManager.LoadScene ("MainMenu");
	}

	public void exitGame(){
		Application.Quit();
	}

}
