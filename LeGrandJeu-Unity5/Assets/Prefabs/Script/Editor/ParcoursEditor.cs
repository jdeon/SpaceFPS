using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( Parcours ) )]
public class ParcoursEditor : Editor {

	private int choixModeParcoursIndex;
	private int choixModeBouclageIndex;
	private int choixModeVitesseIndex;
	private string[] optionModeParcours = {"Lineaire","BezierC1","BezierG1", "BezierC2","BezierPersonaliser","Force"/*,"CourbeEvitePoint"*/};
	private string[] optionModeBouclage = {"AllerSimple", "AllerRetour", "BoucleTeleport", "Boucle"};
	private string[] optionModeVitesse = {"LineaireEtape", "ElastiqueEtape", /*"ElasticAdaptative",*/"BondEtape"};

	private bool moyennerTemps;
	private bool isLocal;
	private float tempsTotal;

	public void OnEnable(){
		Parcours parcoursTraget = (Parcours)target;
		choixModeParcoursIndex = ArrayUtility.IndexOf (optionModeParcours, parcoursTraget.modeDeParcours);
		choixModeBouclageIndex = ArrayUtility.IndexOf (optionModeBouclage, parcoursTraget.modeDeBouclage);
		choixModeVitesseIndex = ArrayUtility.IndexOf (optionModeVitesse, parcoursTraget.modeDeVitesse);
		moyennerTemps = parcoursTraget.moyennerTemps;
		isLocal = parcoursTraget.isLocal;
		tempsTotal = sommeTempsEtape ();

	}


	public override void OnInspectorGUI(){	

		Parcours parcoursTraget = (Parcours)target;
		GameObject scriptObjet = parcoursTraget.getGameObject();
		//int ancienneValeur;

		if(parcoursTraget.listEtapeTransform.Count == 0){
			parcoursTraget.initListTransformVide(); 
		}

		parcoursTraget.nomParcour = EditorGUILayout.TextField ("Nom du parcours", parcoursTraget.nomParcour);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Parcours visible en jeu");
		parcoursTraget.visibleJeu = EditorGUILayout.Toggle (parcoursTraget.visibleJeu);
		EditorGUILayout.Space ();
		EditorGUILayout.PrefixLabel("Parcours visible en edition");
		parcoursTraget.visibleEdtion = EditorGUILayout.Toggle (parcoursTraget.visibleEdtion);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Couleur du parcours");
		parcoursTraget.pathColor = EditorGUILayout.ColorField (parcoursTraget.pathColor);
		EditorGUILayout.EndHorizontal();

		choixModeParcoursIndex = EditorGUILayout.Popup("Mode de parcours", choixModeParcoursIndex, optionModeParcours);
		//ancienneValeur = choixModeParcoursIndex;
		parcoursTraget.modeDeParcours = optionModeParcours [choixModeParcoursIndex];
		verifNecessiteAjoutPoint ();

		choixModeBouclageIndex = EditorGUILayout.Popup("Mode de bouclage", choixModeBouclageIndex, optionModeBouclage);
		//ancienneValeur = choixModeBouclageIndex;
		parcoursTraget.modeDeBouclage = optionModeBouclage [choixModeBouclageIndex];
		verifNecessiteAjoutPoint ();

		choixModeVitesseIndex = EditorGUILayout.Popup("Mode de vitesse", choixModeVitesseIndex, optionModeVitesse);
		parcoursTraget.modeDeVitesse = optionModeVitesse [choixModeVitesseIndex];

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Apliquer rotation");
		parcoursTraget.isRotating = EditorGUILayout.Toggle (parcoursTraget.isRotating);
		EditorGUILayout.PrefixLabel("Rendre local");
		isLocal = EditorGUILayout.Toggle (isLocal);
		parcoursTraget.isLocal = isLocal;
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel("Moyenner les temps");
		moyennerTemps = EditorGUILayout.Toggle (moyennerTemps);
		parcoursTraget.moyennerTemps = moyennerTemps;
		if (moyennerTemps) {
			EditorGUILayout.PrefixLabel ("Temps total");
			tempsTotal = EditorGUILayout.FloatField (tempsTotal);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal ();
		parcoursTraget.nombreEtape = Mathf.Max (2, EditorGUILayout.IntField ("Nb etape", parcoursTraget.nombreEtape));
		EditorGUILayout.EndHorizontal();

		//Ajouter des points vides
		if(parcoursTraget.nombreEtape > parcoursTraget.listEtapeTransform.Count){
			verifNecessiteAjoutPoint();
			for (int i = parcoursTraget.listEtapeTransform.Count; i < parcoursTraget.nombreEtape; i++) {
				GameObject etapeObject = new GameObject();
				etapeObject.name = "etapeNumero " + i;
				Transform transformVide = etapeObject.transform;
				transformVide.parent = scriptObjet.transform;
				if (isLocal){
					transformVide.position = scriptObjet.transform.position;
				} else {
					transformVide.position = Vector3.zero;
				}
				transformVide.rotation = Quaternion.identity;
				parcoursTraget.listEtapeTransform.Add(transformVide);
				parcoursTraget.listTempsPourProchaineEtape.Add(0);
			}
		}
		
		//Supprimer des points
		if(parcoursTraget.nombreEtape < parcoursTraget.listEtapeTransform.Count){
			verifNecessiteAjoutPoint();
			if(EditorUtility.DisplayDialog("Supprimer des point?","Etes vous sure de vouloir supprimer ces points", "OK", "Annuler")){
				int nbEtapeTransform = parcoursTraget.listEtapeTransform.Count;
				int removeCount = nbEtapeTransform - parcoursTraget.nombreEtape;

				for (int i = parcoursTraget.nombreEtape; i < nbEtapeTransform ; i++) {
					Transform etapeADetruire = parcoursTraget.transform.Find("etapeNumero " + i);
					if(null != etapeADetruire){
						GameObject.DestroyImmediate(etapeADetruire.gameObject);
					}
				}

				parcoursTraget.listEtapeTransform.RemoveRange(parcoursTraget.nombreEtape,removeCount);
				parcoursTraget.listTempsPourProchaineEtape.RemoveRange(parcoursTraget.nombreEtape,removeCount);
			}else{
				parcoursTraget.nombreEtape = parcoursTraget.listEtapeTransform.Count;	
			}
		}
		
		//Valeurs des points:
		EditorGUI.indentLevel = 2;
		for (int i = 0; i < parcoursTraget.listEtapeTransform.Count; i++) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Point N°" + (i+1) + ", temps pour l'atteindre :");
				if (moyennerTemps && i!=0){
					EditorGUILayout.LabelField(calculTempsEtape(i).ToString());
					parcoursTraget.listTempsPourProchaineEtape[i] = calculTempsEtape(i);
				} else {
					parcoursTraget.listTempsPourProchaineEtape[i] = EditorGUILayout.FloatField(parcoursTraget.listTempsPourProchaineEtape[i]);
				}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
            if (isLocal)
            {
                Vector3 transformPosition = parcoursTraget.listEtapeTransform[i].localPosition;
                transformPosition = EditorGUILayout.Vector3Field("", transformPosition);
                parcoursTraget.listEtapeTransform[i].localPosition = transformPosition;
            } else
            {
                Vector3 transformPosition = parcoursTraget.listEtapeTransform[i].position;
                transformPosition = EditorGUILayout.Vector3Field("", transformPosition);
                parcoursTraget.listEtapeTransform[i].position = transformPosition;
            }
			
			EditorGUILayout.EndHorizontal();
		}

		/*EditorGUILayout.BeginHorizontal();
		GUILayout.BeginArea (new Rect(0, 500, 500, 500));
		for (int i = 0; i < parcoursTraget.listEtapeTransform.Count; i++) {
			//
			
			Vector3 transformPosition = parcoursTraget.listEtapeTransform[i].position;
			if (isLocal){
				transformPosition -= scriptObjet.transform.position;
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Etape " + (i+1));
			transformPosition.x = EditorGUILayout.FloatField(transformPosition.x);
			transformPosition.y = EditorGUILayout.FloatField(transformPosition.y);
			transformPosition.z = EditorGUILayout.FloatField(transformPosition.z);
			if (moyennerTemps && i!=0){
				EditorGUILayout.LabelField(calculTempsEtape(i).ToString());
				parcoursTraget.listTempsPourProchaineEtape[i] = calculTempsEtape(i);
			} else {
				parcoursTraget.listTempsPourProchaineEtape[i] = EditorGUILayout.FloatField(parcoursTraget.listTempsPourProchaineEtape[i]);
			}

			if (isLocal){
				transformPosition += scriptObjet.transform.position;
			}
			parcoursTraget.listEtapeTransform[i].position = transformPosition;
			EditorGUILayout.EndHorizontal();
		}
		GUILayout.EndArea ();
		EditorGUILayout.EndHorizontal();*/
		
		//update and redraw:
		if(GUI.changed){
			EditorUtility.SetDirty(parcoursTraget);			
		}
	}
	
	void OnSceneGUI(){
		Parcours parcoursTraget = (Parcours)target;
		if(parcoursTraget.visibleEdtion){			
			if(parcoursTraget.listEtapeTransform.Count > 0){				
				//path begin and end labels:
				Handles.Label(parcoursTraget.listEtapeTransform[0].position, "'" + parcoursTraget.nomParcour + "' Begin");
				Handles.Label(parcoursTraget.listEtapeTransform[parcoursTraget.listEtapeTransform.Count-1].position, "'" + parcoursTraget.nomParcour + "' End");
				
				//node handle display:
				for (int i = 0; i < parcoursTraget.listEtapeTransform.Count; i++) {
					parcoursTraget.listEtapeTransform[i].position = Handles.PositionHandle(parcoursTraget.listEtapeTransform[i].position, parcoursTraget.listEtapeTransform[i].rotation);
				}	
			}	
		}
	}

	private float calculTempsEtape(int numEtape){
		Parcours parcoursTraget = (Parcours)target;
		float distanceTotal = 0;

		//Calcul de la distance total du parcours
		if(numEtape > 0 && numEtape < parcoursTraget.listEtapeTransform.Count){
			for (int i=1; i<parcoursTraget.listEtapeTransform.Count; i++){
				Vector3 etape = parcoursTraget.listEtapeTransform[i].position-parcoursTraget.listEtapeTransform[i-1].position;
				distanceTotal += etape.magnitude; 
			}
			if (distanceTotal != 0){
				Vector3 distanceEtape = parcoursTraget.listEtapeTransform[numEtape].position-parcoursTraget.listEtapeTransform[numEtape-1].position;
				return tempsTotal*distanceEtape.magnitude/distanceTotal;
			}
		}
		return 0;
	}

	/**
	 * rajoute les coordonné de la transform du script à tous les point du parcours
	 * */
	public Vector3 majPositionParcours(Vector3 positionObject, Vector3 positionLocal, bool goToLocal){
		if (goToLocal) {
			positionObject = positionObject + positionLocal;
		} else {
			positionObject = positionObject - positionLocal;
		}
		return positionObject;
	}

	private void verifNecessiteAjoutPoint(){
		Parcours parcoursTraget = (Parcours)target;
		if (parcoursTraget.listEtapeTransform.Count % 2 == 0 && parcoursTraget.modeDeBouclage == "Boucle" && parcoursTraget.modeDeParcours == "BezierC1") {
			if (EditorUtility.DisplayDialog ("Rajouter un point?", "Le calcul est impossible avec c'est condition, un poit sera rajouter pour résoudre le problème", "OK", "Annuler")) {
				GameObject etapeObject = new GameObject ();
				etapeObject.name = "etapeNumero " + parcoursTraget.listEtapeTransform.Count; 
				Transform transformVide = etapeObject.transform;
				transformVide.parent = parcoursTraget.getGameObject ().transform;

				transformVide.position = (parcoursTraget.listEtapeTransform [0].position + parcoursTraget.listEtapeTransform [parcoursTraget.listEtapeTransform.Count - 1].position) / 2;
				transformVide.rotation = Quaternion.identity;
				parcoursTraget.listEtapeTransform.Add (transformVide);
				parcoursTraget.listTempsPourProchaineEtape.Add (0);
				parcoursTraget.nombreEtape++;
			} else {
				choixModeBouclageIndex = 0;
				parcoursTraget.modeDeBouclage = optionModeBouclage [choixModeBouclageIndex];
			}
		}
	}

	private float sommeTempsEtape(){
		Parcours parcoursTraget = (Parcours)target;
		float tempsTotal = 0;

		for (var numEtape = 1; numEtape<parcoursTraget.listEtapeTransform.Count; numEtape++){
			tempsTotal += parcoursTraget.listTempsPourProchaineEtape[numEtape];
		}

		return tempsTotal;
	}
}