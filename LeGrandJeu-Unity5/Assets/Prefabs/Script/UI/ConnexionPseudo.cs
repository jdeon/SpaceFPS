using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConnexionPseudo : ConditionEventAbstract {

	public InputField mainInputField;
	public Button boutonConnexion;
	public Button boutonNvJoueur;

	[SerializeField]
	private string adminName;

	private static int nivActuel;
	private static int idCheckpointActuel;
	private static string alphabeticString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	private PlayerInputAction controller;
	void Awake()
	{
		controller = new PlayerInputAction();
		controller.UI.Movement.performed += ctx => {
			Vector2 inputDirection = ctx.ReadValue<Vector2>();
			OnNavigate(inputDirection);
		};
		controller.UI.Submit.performed += ctx => {
			OnSubmit();
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

	void Start(){
		CursorCustom.Activate = true;
		nivActuel = 0;
		idCheckpointActuel = 0;
		EventSystem.current.SetSelectedGameObject(null);
		mainInputField.Select();
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

	private void OnNavigate(Vector2 inputDirection)
	{
		if (mainInputField.gameObject.Equals(EventSystem.current.currentSelectedGameObject)) {
			if(mainInputField.text.Length == 0)
            {
				mainInputField.text += "A";
				return;
            }

			if (inputDirection.y > 0)
			{
				changeLastLetterText(-1);
			}
			else if (inputDirection.y < 0)
			{
				changeLastLetterText(1);
			}
			else if (inputDirection.x > 0)
			{
				mainInputField.text += "A";
			}
			else if (inputDirection.x < 0)
			{
				mainInputField.text = mainInputField.text.Remove(mainInputField.text.Length - 1);
			}
		}
	}

	private void changeLastLetterText(int indexUpdate)
    {
		char lastChar = mainInputField.text[mainInputField.text.Length - 1];
		int alphaIndex = alphabeticString.IndexOf(lastChar);
		alphaIndex += indexUpdate;

		if(alphaIndex >= alphabeticString.Length)
        {
			alphaIndex -= alphabeticString.Length;
		} 
		else if (alphaIndex < 0)
        {
			alphaIndex += alphabeticString.Length;
		}

		string newText = mainInputField.text.Remove(mainInputField.text.Length - 1);
		newText += alphabeticString[alphaIndex];

		mainInputField.text = newText;
	}

	private void OnSubmit()
	{
		if (EventSystem.current.currentSelectedGameObject.Equals(mainInputField.gameObject))
		{
			boutonConnexion.Select();
		}
	}

	public override void onChange (){
	}
}
