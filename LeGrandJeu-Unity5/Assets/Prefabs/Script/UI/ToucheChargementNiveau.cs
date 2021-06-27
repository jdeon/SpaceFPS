using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToucheChargementNiveau : MonoBehaviour {

	public List<Button> listButton;


	// Use this for initialization
	void Start () {
		gestionAffichage ();	
	}

	void onEnable(){
		gestionAffichage ();
	}

	void gestionAffichage(){
		int nivActuel = ConnexionPseudo.getNivActuel();
		for (int index = 0; index < nivActuel; index++) {
			if (index < listButton.Count) {
				listButton [index].transform.FindChild ("ImageActif").gameObject.SetActive (true);
			}
		}
	}

	public void clickBouttonNiv(int numNiv) {
		if (numNiv <= ConnexionPseudo.getNivActuel ()) {
			PlayerPrefs.SetString (PlayerPrefs.GetString(Constantes.PP_JOUEUR_COURANT), GestionCheckpoint.mapActualCheckPointToText(ConnexionPseudo.getNivActuel (),1));
			chargerNiveau(numNiv);
		}
	}

	public void continuerBouton(){
		if (ConnexionPseudo.getNivActuel () > 0) {
			chargerNiveau(ConnexionPseudo.getNivActuel ());
		}
	}

	private void chargerNiveau(int numNiv){
		switch (numNiv) {
		case 1: 
			SceneManager.LoadScene(Constantes.NOM_LVL_1);
			break;
		case 2: 
			SceneManager.LoadScene(Constantes.NOM_LVL_2);
			break;
		case 3: 
			SceneManager.LoadScene(Constantes.NOM_LVL_3);
			break;
		case 4: 
			SceneManager.LoadScene(Constantes.NOM_LVL_4);
			break;
		case 5: 
			SceneManager.LoadScene(Constantes.NOM_LVL_5);
			break;
		}
	}
}
