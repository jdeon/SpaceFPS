using UnityEngine;
using System.Collections;

public class CAPrintCustomMessageOnGUIForSeconds : CustomActionScript {

	public enum Ancre{Haut, Centre, Bas}

	public float _seconds;

	public string _message;

	public float _widthPourcentage;

	public float _heightPourcentage;

	public Ancre ancrageVertical;

	public int nombreCaractereParLigne; //Approximatif pour calculer la taille du texte
	private bool _printingMessage = false;

	private Rect rect;


	public void OnGUI()
	{
		if (_printingMessage && null != rect)
		{
			float tailleText = _widthPourcentage * Screen.width * ((float)nombreCaractereParLigne) / 3000f; //Aproximation 80 caractere sur 1500 pixel ont une taille de 40
			GUI.skin.textArea.fontSize = Mathf.RoundToInt(tailleText);
			GUI.TextArea(rect, _message);
		}
	}

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		
		if (ancrageVertical == Ancre.Haut) {
			rect = new Rect(Screen.width * (1f - _widthPourcentage) /2f, Screen.height/8f, Screen.width * _widthPourcentage, Screen.height * _heightPourcentage);
		} else if (ancrageVertical == Ancre.Bas) {
			rect = new Rect(Screen.width * (1f - _widthPourcentage) /2f,Screen.height * (7f/8f - _heightPourcentage) , Screen.width * _widthPourcentage, Screen.height * _heightPourcentage);
		} else {
			rect = new Rect(Screen.width * (1f - _widthPourcentage) /2f, Screen.height * (1f - _heightPourcentage) / 2f, Screen.width * _widthPourcentage, Screen.height * _heightPourcentage);
		}
		_printingMessage = true;
		yield return new WaitForSeconds(_seconds);
		_printingMessage = false;

		/*GameObject goController = GameObject.Find (Constantes.STR_CONTROLLER_OCR_AMELIORE);
		Transform transfDialoguePanel = goController.transform.Find (Constantes.STR_CONTR_PANEL_DIALOGUE);
		//Canvas canvasPanelDialogue = goDialoguePanel.GetComponent<Canvas> ();
		transfDialoguePanel.localScale = new Vector3(_width / Screen.width, _height / Screen.height,1);

		Transform transfDialogueText = goController.transform.Find (Constantes.STR_CONTR_TEXT_DIALOGUE);
		GUIText textDialogue = transfDialogueText.gameObject.GetComponent<GUIText> ();
		textDialogue.text = _message;

		transfDialoguePanel.gameObject.SetActive (true);

		_printingMessage = true;
		yield return new WaitForSeconds(_seconds);
		_printingMessage = false;
		transfDialoguePanel.gameObject.SetActive (false);*/
	}

}
