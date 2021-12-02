using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionPause : MonoBehaviour {

	public GameObject goEcran;

	private bool inPause;
	private bool changeEtat;

	private PlayerInputAction controller;
	void Awake()
	{
		controller = new PlayerInputAction();
		controller.PlayerActions.Cancel.performed += ctx => {
			OnCancel();
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

			changeEtat = false;
		}
	}

    void OnCancel()
    {
		resumeGame();
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
