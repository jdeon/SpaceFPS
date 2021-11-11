using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnexionPseudo : ConditionEventAbstract {

	public InputField mainInputField;
	public Button boutonConnexion;
	public Button boutonNvJoueur;

	[SerializeField]
	private string adminName;

	private static int nivActuel;
	private static int idCheckpointActuel;

	void Start(){
		Cursor.visible = true;
		nivActuel = 0;
		idCheckpointActuel = 0;
	}

	// Update is called once per frame
	void Update () {
		//test saisi??
	}

	public void connexionBouton(){
		if(mainInputField.text != ""){
			if (mainInputField.text == adminName) {
				nivActuel = 6;
				idCheckpointActuel = 1;
				PlayerPrefs.SetString (adminName, GestionCheckpoint.mapActualCheckPointToText(6,1));
				PlayerPrefs.SetString (Constantes.PP_JOUEUR_COURANT, adminName);
				this.isActive = true;
			} else if (PlayerPrefs.HasKey (mainInputField.text)) {
				string etapeActuel = PlayerPrefs.GetString (mainInputField.text); //format : lvl_???_checkP_???
				GestionCheckpoint.mapSaveCheckpointDataToInt (etapeActuel, out nivActuel, out idCheckpointActuel);

				PlayerPrefs.SetString (Constantes.PP_JOUEUR_COURANT, mainInputField.text);
				this.isActive = true;
			} else {
				//Nouveau joueur
				boutonNvJoueur.gameObject.SetActive(true);
			}
		}
	}

	public void creeNouveauJoueurBouton(){
		if(mainInputField.text != ""){
			nivActuel = 1;
			idCheckpointActuel = 1;
			PlayerPrefs.SetString (mainInputField.text, GestionCheckpoint.mapActualCheckPointToText(1,1));
			PlayerPrefs.SetString (Constantes.PP_JOUEUR_COURANT, mainInputField.text);
			this.isActive = true;
		}
	}

	public static int getNivActuel(){
		return nivActuel;
	}

	public static int  getidCheckpointActuel(){
		return idCheckpointActuel;
	}

	public override void onChange (){
	}
}
