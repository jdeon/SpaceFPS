using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parcours : MonoBehaviour {

	public static Dictionary<string,Parcours> listDesParcours;

	public string nomParcour ="";
	public bool isRotating =false;
	public int nombreEtape = 2 ;
	public List<Transform> listEtapeTransform = new List<Transform>();
	public List<float> listTempsPourProchaineEtape = new List<float>(){0,0};
	public string modeDeParcours;	//Lineaire,BezierC1, BezierG1,bezierC2,BezierPersonaliser,Force,courbeEvitePoint
	public string modeDeBouclage;	//Aller-Fin, boucle-téléport,aller-retour,boucle
	public string modeDeVitesse;	//LineaireEtape, lineaireMoyenneTemps, ElastiqueEtape, ElasticAdaptative, elasticMoyenneTemps,bondEtape(ou elastic inverse),Bond moyenne temps
	public bool visibleEdtion = true;
	public bool visibleJeu = true;
	public Color pathColor = Color.cyan;

	//Variable utilise par l'editor
	public bool moyennerTemps = false;
	public bool isLocal = false; 

	//private List<Vector3> listPointControl;
	private List<CourbeBezier> listEtape = new List<CourbeBezier>();
	private float securiteIncrementationVitesse = 1.25f;
	public bool IsInPlay { get; set; }

	void OnDrawGizmosSelected(){
		if (visibleEdtion && this.modeDeParcours != "Force" && !this.IsInPlay) {
			float tTotal = calculTempsTotal ();
			Vector3 positionActuelle = listEtapeTransform [0].position;
			int etapeEnCours = 0;

			if (null != Parcours.listDesParcours && Parcours.listDesParcours.ContainsKey (nomParcour)) {
				Parcours.listDesParcours.Remove (nomParcour);
			} 

			listEtape = new List<CourbeBezier> ();
			initialisationParcours ();


			for (int i = 0; i< tTotal*100; i++) {
				float t = ((float)i) / 100f;

				for (int j = 0; j<etapeEnCours; j++) {
					t -= this.listTempsPourProchaineEtape [j + 1];
				}
								
				//Si le temps de l'étape est dépassé, on change d'étape sinon on bouge le point
				if (t >= this.listTempsPourProchaineEtape [etapeEnCours + 1]) {
					etapeEnCours++;
					if(etapeEnCours == this.listEtape.Count - 1 || etapeEnCours == this.listTempsPourProchaineEtape.Count -1) {
						break;
					}
				} else {
					float proportion = this.listTempsPourProchaineEtape [etapeEnCours + 1] == 0 ? 0 : t / this.listTempsPourProchaineEtape [etapeEnCours + 1];
					Vector3 positionSuivante = this.listEtape [etapeEnCours+1].getPointAtTime (proportion);
					Gizmos.color = this.pathColor;
					Gizmos.DrawLine (positionActuelle, positionSuivante);
					positionActuelle = positionSuivante;
				}
			}

			if (this.modeDeBouclage == "Boucle") {
				etapeEnCours = this.listEtape.Count - 1;
				for (int i = 0; i< this.listTempsPourProchaineEtape [0]*100; i++) {
					float t = ((float)i) / 100f;
					float proportion = this.listTempsPourProchaineEtape [0] == 0 ? 0 : t / this.listTempsPourProchaineEtape [0];
					Vector3 positionSuivante = this.listEtape [etapeEnCours].getPointAtTime (proportion);
					Gizmos.color = this.pathColor;
					Gizmos.DrawLine (positionActuelle, positionSuivante);
					positionActuelle = positionSuivante;
				}
			}
			etapeEnCours = 0;
		}
	}
		
	void OnDestroy()
	{
		Parcours.listDesParcours.Remove (this.nomParcour);
	}


	public bool isNotEmpty(){
		bool isNotEmpty = false;
		isNotEmpty = (null != nomParcour && !"".Equals (nomParcour) && null != modeDeParcours && !"".Equals (modeDeParcours)
			&& null != modeDeBouclage && !"".Equals (modeDeBouclage) && null != modeDeVitesse && !"".Equals (modeDeVitesse) 
			&& nombreEtape > 1);
		return isNotEmpty;
	}

	public void initListTransformVide(){
		for (int i =0; i<2; i++) {
			GameObject etapeObject = new GameObject ();
			etapeObject.name = "etapeNumero " + i;
			Transform transformVide = etapeObject.transform;
			transformVide.parent = gameObject.transform;
			transformVide.position = Vector3.zero;
			transformVide.rotation = Quaternion.identity;
			listEtapeTransform.Add(transformVide);
		}
	}

/**
	 * lance l'initialisation du parcours en fonction des option choisi
	 * */
	public void initialisationParcours(){
		this.listEtape.Add(new CourbeBezier (listEtapeTransform[0].position, listEtapeTransform[0].position));
		if (this.listEtapeTransform.Count > 2) {
			switch (this.modeDeParcours) {
			case "Lineaire":
				intialiserParcoursLineaire ();	//creation des courbes de bezier
				break;
			case "BezierC1":
			case "BezierG1":
				initialiserParcoursCourbeBezierOrdre1 (this.modeDeParcours == "BezierG1");		//calcul point de controle et creation des courbes de bezier
				break;
			case "BezierC2": 
				initialiserParcoursCourbeBezierC2 ();	//calcul point de controle et creation des courbes de bezier
				break;
			case "BezierPersonaliser": 
				initialiserParcoursCourbeBezierPersonaliser ();	//retirer point de controle de liste Etape pour les mettre dans liste point de controle
				break;
			case "Force": 
				//initialiserParcoursForce ();	//aucun traitement
				break;
			case "CourbeEvitePoint":
				//initialiserParcoursCourbeBezierUnique (); //retirer point de controle de liste Etape pour les mettre dans liste point de controle
				break;
			default : 
				intialiserParcoursLineaire (); 	//creation des courbes de bezier
				break;
			}
		} else {
			intialiserParcoursLineaire ();	//aucun traitement
		}

		//On rajoute la valeur pour atteindre le premier point à la fin
		if (this.modeDeBouclage == "Boucle"){
			if(this.listTempsPourProchaineEtape.Count < this.listEtape.Count){ 
				this.listTempsPourProchaineEtape.Add(this.listTempsPourProchaineEtape[0]);
			} else if (this.listTempsPourProchaineEtape[listEtape.Count - 1] != this.listTempsPourProchaineEtape[0]){
				this.listTempsPourProchaineEtape[listEtape.Count - 1] = this.listTempsPourProchaineEtape[0];
			}
		}
		//intialisationListTempsPourModeMoyen ();

		//On les ajoute dans la liste de tout les parcours
		if (null == listDesParcours) {
			listDesParcours = new Dictionary<string, Parcours> ();
		}

		if (!listDesParcours.ContainsKey (this.nomParcour)) {
			listDesParcours.Add (this.nomParcour, this);
		}
	}

	/**
	 * fait bouger la position de la transform en parametre en fonction du temps
	 * REM : attention le cas du bouclage doit etre pris en compte dans le script qui l'utilise
	 * */
	public void parcourirEtape(Transform transformTarget, Rigidbody rigidbTarget, float tempsSurEtapeEnCours, int etapeEnCours){

		if (isRotating){
			appliquerRotation (tempsSurEtapeEnCours, transformTarget, etapeEnCours);
		}
		tempsSurEtapeEnCours = calculTempsEnFonctionModeVitesse(tempsSurEtapeEnCours,0,etapeEnCours);
		if (this.modeDeParcours == "Force") {
			parcourirAvecForce(tempsSurEtapeEnCours,transformTarget,rigidbTarget, etapeEnCours);
		}else{
			parcourirBezier(this.listEtape[etapeEnCours] , tempsSurEtapeEnCours, transformTarget, etapeEnCours, rigidbTarget);
		}
	}

	/**
	 * On calcul le temps total du parcours
	 * */
	private float calculTempsTotal (){
		float tTotal = 0;
			
		//Calcul du temps et de la distance total du parcours
		for (int i=1; i<this.listTempsPourProchaineEtape.Count; i++){
			tTotal += this.listTempsPourProchaineEtape[i];
		}

		return tTotal;
	}


	/**
	 * Initialise la liste des courbe de bezier Continuité C0 (lineaiaire)
	 * */
	private void intialiserParcoursLineaire (){
		for (int numPoint = 1; numPoint < listEtapeTransform.Count; numPoint++) {
			this.listEtape.Add (new CourbeBezier (listEtapeTransform[numPoint - 1].position, listEtapeTransform[numPoint].position));
		}
		if (this.modeDeBouclage == "Boucle") {
			this.listEtape.Add (new CourbeBezier (listEtapeTransform[listEtapeTransform.Count - 1].position, listEtapeTransform[0].position));
		}
	}




	/**
	 * nitialise la liste des courbe de bezier Continuité ordre1 (quadratique)
	 * Rem : risque de problème si les point ne sont pas aussi loin les uns des autres (introduction d un facteur?)
	 * */
	private void initialiserParcoursCourbeBezierOrdre1 (bool isContinuiteG1){
		if (this.modeDeBouclage == "Boucle") {
			initialiserParcoursBoucleOrdre1(isContinuiteG1);
		} else {
			Vector3 pointControle1 = CourbeBezier.getPremierPointControle (listEtapeTransform[0].position, listEtapeTransform[1].position, listEtapeTransform[2].position);
			Vector3 pointControlePrec;

			//Coefficiant 1/2 dans fonction pour bezier quadratique
			pointControle1 = (pointControle1 + listEtapeTransform[0].position)/2;				

			this.listEtape.Add(new CourbeBezier(listEtapeTransform[0].position, pointControle1,listEtapeTransform[1].position));

			pointControlePrec = pointControle1;
			for (int numPoint = 2; numPoint < listEtapeTransform.Count; numPoint++) {

				Vector3 pointControleNouveau;
				Vector3 pointDeParcours = listEtapeTransform[numPoint - 1].position;

				if (isContinuiteG1){
					pointControleNouveau = CourbeBezier.getPointControleSuivantAvecContinuiteG1(listEtapeTransform[numPoint - 2].position, pointControlePrec, pointDeParcours, listEtapeTransform[numPoint].position);
				} else {
					pointControleNouveau = CourbeBezier.getPointControleSuivantAvecContinuiteC1(pointControlePrec, pointDeParcours);
				}

				this.listEtape.Add(new CourbeBezier(listEtapeTransform[numPoint - 1].position, pointControleNouveau, listEtapeTransform [numPoint].position));
				pointControlePrec = pointControleNouveau;
			}
		}
	}

	/**
	 * Initialise la liste des point de controle d'une bezier C1 (quadratique) si le bouclage un "boucle"
	 * 
	 * */
	private void initialiserParcoursBoucleOrdre1(bool isContinuiteG1){

		Matrice2D matriceCoef = initialisationMatriceCoeficientPourBoucleContinueteC1 ();
		Matrice2D matriceResultat = intialisationMatriceResultatBoucleC1 ();
		
		matriceCoef.inverse ();
		
		Matrice2D matricePointControle = matriceCoef.produit (matriceResultat);

		//Creation de la liste des courbe de bezier
		for (int numLig=0; numLig< matricePointControle.nbLigne; numLig++) {
			Vector3 pointControle = new Vector3 (matricePointControle.array [numLig, 0], matricePointControle.array [numLig, 1], matricePointControle.array [numLig, 2]);

			if (numLig == matricePointControle.nbLigne-1) { 
				this.listEtape.Add(new CourbeBezier(listEtapeTransform[numLig].position, pointControle, listEtapeTransform[0].position));
			} else {
				this.listEtape.Add(new CourbeBezier(listEtapeTransform[numLig].position, pointControle, listEtapeTransform[(numLig+1)].position ));
			}
		}
	}

	/**
	 * Initialise la matrice des coefficients pour une boucle de continuite C1
	 * */
	private Matrice2D initialisationMatriceCoeficientPourBoucleContinueteC1(){
		int nbLigne = this.listEtapeTransform.Count;
		//Si le nombre de ligne est paire la matrice n'est pas diagonalisable, on rajoute donc un point imaginaire moyen entre fin debut
		if (nbLigne % 2 == 0) {
			nbLigne++;
		}
		Matrice2D matriceCoef = new Matrice2D (nbLigne, nbLigne);
		for (int numLigne = 0; numLigne < nbLigne; numLigne++) {
			if (numLigne == 0) {
				matriceCoef.array [0, 0] = 1;
				matriceCoef.array [0, nbLigne - 1] = 1;
			} else {
				matriceCoef.array [numLigne, numLigne - 1] = 1;
				matriceCoef.array [numLigne, numLigne] = 1;
			}
		}
		return matriceCoef;
	}


	private Matrice2D intialisationMatriceResultatBoucleC1(){
		int nbPointParcours = this.listEtapeTransform.Count;
		int nbLigne = nbPointParcours;

		Matrice2D matriceResultat = new Matrice2D (nbLigne, 3);

		for (int numPointParcours=0; numPointParcours < nbPointParcours; numPointParcours++) {
			matriceResultat.array [numPointParcours, 0] = 2*this.listEtapeTransform[numPointParcours].position.x;
			matriceResultat.array [numPointParcours, 1] = 2*this.listEtapeTransform[numPointParcours].position.y;
			matriceResultat.array [numPointParcours, 2] = 2*this.listEtapeTransform[numPointParcours].position.z;
			}

		return matriceResultat;
		}


	/**
	 * Initialise la liste des point de controle d'une bezier C2 (Cubique)
	 * Rem : risque de problème si les point ne sont pas aussi loin les uns des autres (introduction d un facteur?)
	 * */
	private void initialiserParcoursCourbeBezierC2 (){
		int nbPointParcours = this.listEtapeTransform.Count;
		Matrice2D matriceCoef;
		Matrice2D matriceResultat;
		Vector3 pointPremierControle;
		Vector3 pointDernierControle;
		bool isBoucle = (this.modeDeBouclage == "Boucle");
		if (isBoucle) {
			matriceCoef = CourbeBezier.getMatriceCoeficientPourContinueteC2 (nbPointParcours);
			matriceResultat = intialisationMatriceResultatC2 (Vector3.zero, Vector3.zero, true);
			pointPremierControle = Vector3.zero;
			pointDernierControle = Vector3.zero;
		} else {
			pointPremierControle = CourbeBezier.getPremierPointControle (listEtapeTransform [0].position, listEtapeTransform [1].position, listEtapeTransform [2].position);
			pointDernierControle = CourbeBezier.getPremierPointControle (listEtapeTransform [nbPointParcours - 1].position, listEtapeTransform [nbPointParcours - 2].position, listEtapeTransform [nbPointParcours - 3].position);
			//Coefficiant 1/3 dans fonction pour bezier cubique
			pointPremierControle = (pointPremierControle + 2 * listEtapeTransform [0].position) / 3;

			//Coefficiant 1/3 dans fonction pour bezier cubique
			pointDernierControle = (pointDernierControle + 2 * listEtapeTransform [nbPointParcours - 1].position) / 3;

			matriceCoef = CourbeBezier.getMatriceCoeficientPourContinueteC2 (nbPointParcours);
			matriceCoef.deleteColumn(0); //Premier point de controle déjà connu
			matriceCoef.deleteColumn(matriceCoef.nbColonne - 1); //Pas boucle
			matriceCoef.deleteColumn(matriceCoef.nbColonne - 1); //Pas boucle
			matriceCoef.deleteColumn(matriceCoef.nbColonne - 1); //Dernier point de controle déjà connue

			matriceCoef.deleteRow(0); //Pas boucle
			matriceCoef.deleteRow(0); //Pas boucle
			matriceCoef.deleteRow(matriceCoef.nbLigne - 1); 
			matriceCoef.deleteRow(matriceCoef.nbLigne - 1);


			matriceResultat = intialisationMatriceResultatC2 (pointPremierControle, pointDernierControle, false);
		}

		matriceCoef.inverse ();

		Matrice2D matricePointControle = matriceCoef.produit (matriceResultat);

		//Creation de la liste des courbe de bezier
		for (int numLig=0; numLig< matricePointControle.nbLigne; numLig++) {
			Vector3 pointControle1;
			Vector3 pointControle2;
			if (numLig == 0 && !isBoucle){
				pointControle1 = pointPremierControle;
				pointControle2 = new Vector3 (matricePointControle.array [numLig, 0], matricePointControle.array [numLig, 1], matricePointControle.array [numLig, 2]);
				this.listEtape.Add(new CourbeBezier(listEtapeTransform[0].position, pointControle1, pointControle2,listEtapeTransform[1].position ));
			} else if (numLig == matricePointControle.nbLigne - 1 && !isBoucle){
				pointControle1 = new Vector3 (matricePointControle.array [numLig, 0], matricePointControle.array [numLig, 1], matricePointControle.array [numLig, 2]);
				pointControle2 = pointDernierControle;
				this.listEtape.Add(new CourbeBezier(listEtapeTransform[nbPointParcours-2].position, pointControle1, pointControle2, listEtapeTransform[nbPointParcours-1].position ));
			} else if (numLig == matricePointControle.nbLigne - 2 && isBoucle){
				//fermeture de la boucle
				pointControle1 = new Vector3 (matricePointControle.array [numLig, 0], matricePointControle.array [numLig, 1], matricePointControle.array [numLig, 2]);
				pointControle2 = new Vector3 (matricePointControle.array [numLig+1, 0], matricePointControle.array [numLig+1, 1], matricePointControle.array [numLig+1, 2]);

				this.listEtape.Add(new CourbeBezier(listEtapeTransform[listEtapeTransform.Count-1].position, pointControle1, pointControle2, listEtapeTransform[0].position ));
				numLig++;		//2 point ont ete utilise il faut donc incrementer de 1 de plus
			
			} else {
				pointControle1 = new Vector3 (matricePointControle.array [numLig, 0], matricePointControle.array [numLig, 1], matricePointControle.array [numLig, 2]);
				pointControle2 = new Vector3 (matricePointControle.array [numLig+1, 0], matricePointControle.array [numLig+1, 1], matricePointControle.array [numLig+1, 2]);

				//On cherche les point de la transform etape en theorie on tombra toujours sur le premier point de controle 
				//(si pas de boucle numLig%2 ==1 car le premier de la liste est un second point de controle)
				//Premier point de controle est relier à l'index du pointEtape debut par (numLig+1)/2 si 
				int numEtape = (modeDeBouclage == "Boucle" ? numLig/2 : (numLig+1)/2);
				this.listEtape.Add(new CourbeBezier(listEtapeTransform[numEtape].position, pointControle1, pointControle2, listEtapeTransform[numEtape+1].position ));
				numLig++;		//2 point ont ete utilise il faut donc incrementer de 1 de plus
			}
		}
	}

	/**
	 * Matrice contenant les resultats en fonction des point du parcours et de l'axe
	 * axe est egal à x, y ou z
	 * */
	private Matrice2D intialisationMatriceResultatC2 (Vector3 premierControl,Vector3 dernierControl, bool isBoucle){
		int nbPointParcours = this.listEtapeTransform.Count;

		//On a déjà 2 points de controles
		Matrice2D matriceResultat = new Matrice2D (nbPointParcours *2,3);
		int numPointParcours = 0;

		for (int numLig=0; numLig < matriceResultat.nbLigne; numLig++) {

			if (numLig%2 == 0){
				matriceResultat.array [numLig, 0] = 2*this.listEtapeTransform[numPointParcours].position.x;
				matriceResultat.array [numLig, 1] = 2*this.listEtapeTransform[numPointParcours].position.y;
				matriceResultat.array [numLig, 2] = 2*this.listEtapeTransform[numPointParcours].position.z;
				numPointParcours++;
			} else {
				matriceResultat.array [numLig, 0] = 0;
				matriceResultat.array [numLig, 1] = 0;
				matriceResultat.array [numLig, 2] = 0;
			}
		}

		if (!isBoucle) {
			matriceResultat.deleteRow (0);
			matriceResultat.deleteRow (0);
			matriceResultat.deleteRow (matriceResultat.nbLigne - 1);
			matriceResultat.deleteRow (matriceResultat.nbLigne - 1);

			matriceResultat.array [1, 0] = 2 * premierControl.x;
			matriceResultat.array [1, 1] = 2 * premierControl.y;
			matriceResultat.array [1, 2] = 2 * premierControl.z;

			matriceResultat.array [matriceResultat.nbLigne - 1, 0] = -2 * dernierControl.x;
			matriceResultat.array [matriceResultat.nbLigne - 1, 1] = -2 * dernierControl.y;
			matriceResultat.array [matriceResultat.nbLigne - 1, 2] = -2 * dernierControl.z;
		}
		return matriceResultat;
	}

	/**
	 * Calcul enleve les point de controle de la list des transformEtape pour la rajouter dans la liste des controle
	 * */
	private void initialiserParcoursCourbeBezierPersonaliser(){
		
		int nbPoint = this.listEtapeTransform.Count;
		List<Transform> listPointEtapeReelle = this.listEtapeTransform;//Clone?
		List<float> listTempsReel =new List <float>();
		bool pointControleUniquePremierEtape = false;
		bool pointControleUniqueDernierEtape = false;
		
		if(nbPoint%3 == 0){
			pointControleUniquePremierEtape =true;
		} else if (nbPoint%3 == 2){
			pointControleUniquePremierEtape = true;
			pointControleUniqueDernierEtape = true;
		}
		
		for(int numPoint = 0 ; numPoint < nbPoint ; numPoint++){
			
			//Toute les courbe de bezier seront cubique
			if (!(pointControleUniquePremierEtape && pointControleUniqueDernierEtape) && numPoint%3 !=0){
				this.listEtape.Add(new CourbeBezier (this.listEtapeTransform[numPoint-1].position, this.listEtapeTransform[numPoint].position,this.listEtapeTransform[numPoint+1].position, this.listEtapeTransform[numPoint+2].position));
				listPointEtapeReelle.RemoveAt(numPoint);
				listPointEtapeReelle.RemoveAt(numPoint+1);
				numPoint++;					//2 point ont ete utilise il faut donc incrementer de 1 de plus
			}
		
			//La première courbe sera quadratique
			else if (pointControleUniquePremierEtape && numPoint == 1){
				this.listEtape.Add(new CourbeBezier (this.listEtapeTransform[numPoint-1].position, this.listEtapeTransform[numPoint].position,this.listEtapeTransform[numPoint+1].position));
				listPointEtapeReelle.RemoveAt(numPoint);
			} //Toutes les courbes entre la première et la dernière sont cubique
			else if (pointControleUniquePremierEtape && numPoint%3 !=2 && !(pointControleUniqueDernierEtape || numPoint == nbPoint -1)){
				this.listEtape.Add(new CourbeBezier (this.listEtapeTransform[numPoint-1].position, this.listEtapeTransform[numPoint].position,this.listEtapeTransform[numPoint+1].position, this.listEtapeTransform[numPoint+2].position));
				listPointEtapeReelle.RemoveAt(numPoint);
				listPointEtapeReelle.RemoveAt(numPoint+1);
				numPoint++;					//2 point ont ete utilise il faut donc incrementer de 1 de plus
			}
			else if (pointControleUniqueDernierEtape && numPoint == nbPoint -3){
				this.listEtape.Add(new CourbeBezier (this.listEtapeTransform[numPoint-1].position, this.listEtapeTransform[numPoint].position,this.listEtapeTransform[numPoint+1].position));
				listPointEtapeReelle.RemoveAt(numPoint);
				}
			}
		
		//Construction list des temps le temps 0 est celui pour atteindre le premier point, il ne compte pas
		listTempsReel.Add(this.listTempsPourProchaineEtape[0]);

		for(int numPoint = 1 ; numPoint < nbPoint ; numPoint++){

			//Toute les courbe de bezier seront cubique
			if (pointControleUniquePremierEtape && (numPoint == 1)){
				float tempsDeParcours = this.listTempsPourProchaineEtape[numPoint] + this.listTempsPourProchaineEtape[numPoint+1];
				listTempsReel.Add(tempsDeParcours);
				numPoint++; //le numPoint++ de la boucle fera un cycle de 2	
			} else if (pointControleUniqueDernierEtape && numPoint == nbPoint -2){
				float tempsDeParcours = this.listTempsPourProchaineEtape[numPoint] + this.listTempsPourProchaineEtape[numPoint+1];
				listTempsReel.Add(tempsDeParcours);
				numPoint++; //le numPoint++ de la boucle fera un cycle de 2	
			} else {
				float tempsDeParcours = this.listTempsPourProchaineEtape[numPoint] + this.listTempsPourProchaineEtape[numPoint+1] + this.listTempsPourProchaineEtape[numPoint+2];
				listTempsReel.Add(tempsDeParcours);
				numPoint = numPoint+2; //le numPoint++ de la boucle fera un cycle de 3
			}
		}

		this.listEtapeTransform = listPointEtapeReelle;
		this.listTempsPourProchaineEtape = listTempsReel;
	}


	private void appliquerRotation (float t, Transform target, int etapeEnCours)
	{
		if (etapeEnCours == 0) {
			target.rotation = Quaternion.Slerp (target.rotation, this.listEtapeTransform [etapeEnCours].rotation, t / this.listTempsPourProchaineEtape [etapeEnCours]);
		} else if (this.modeDeBouclage == "Boucle" && etapeEnCours == this.listEtape.Count-1) {
			target.rotation = Quaternion.Slerp (this.listEtapeTransform [etapeEnCours].rotation, this.listEtapeTransform [0].rotation, t/this.listTempsPourProchaineEtape [0]);
		} else if ( etapeEnCours  < this.listEtapeTransform .Count && this.listTempsPourProchaineEtape [etapeEnCours] != 0){
			target.rotation = Quaternion.Slerp (this.listEtapeTransform [etapeEnCours - 1].rotation, this.listEtapeTransform [etapeEnCours].rotation, t/this.listTempsPourProchaineEtape [etapeEnCours]);
		}
	}




	/**
	 * Ajoute la force avec la magnitude obtenu par raport à un point à la bonne proportion du trajet
	 * */
	private void parcourirAvecForce(float t, Transform targetTransform, Rigidbody targetRigidBody, int etapeEnCours){
		  
		Vector3 positionSouhaite = Vector3.Lerp (this.listEtapeTransform[etapeEnCours - 1].position, this.listEtapeTransform[etapeEnCours].position, t / this.listTempsPourProchaineEtape[etapeEnCours]);
		Vector3 vecteurSouhaitToObjectif = this.listEtapeTransform [etapeEnCours].position - positionSouhaite;
		Vector3 vecteurSouhaitToActuelle = targetTransform.position - positionSouhaite;

		Vector3 vecteurHorsPlan = Vector3.Cross (vecteurSouhaitToObjectif, vecteurSouhaitToActuelle);
		Vector3 vecteurNormal = Vector3.Cross (vecteurHorsPlan, vecteurSouhaitToObjectif);

		Vector3 vecteurTangentUnitaire = vecteurSouhaitToObjectif.normalized;
		Vector3 vecteurNormalUnitaire = vecteurNormal.normalized;

		//vecteurSouhaitToActuelle = a*vecteurNormalUnitaire + b*vecteurTangentUnitaire
		//on cherche a= (vecteurSouhaitToActuelle.x - b*vecteurTangentUnitaire.x)/vecteurNormalUnitaire.x
		float b = (vecteurSouhaitToActuelle.x * vecteurTangentUnitaire.y - vecteurSouhaitToActuelle.y * vecteurTangentUnitaire.x) / (vecteurNormalUnitaire.x * vecteurTangentUnitaire.y - vecteurNormalUnitaire.y * vecteurTangentUnitaire.x);
		float a = (vecteurSouhaitToActuelle.y - b * vecteurNormalUnitaire.y)/vecteurTangentUnitaire.y;

		targetTransform.LookAt (this.listEtapeTransform[etapeEnCours].position);
		targetRigidBody.AddForce(targetTransform.forward*a,ForceMode.VelocityChange);
	}

	private void parcourirBezier(CourbeBezier courbeEtape, float tempsSurEtapeEnCours, Transform transformTarget, int numEtape, Rigidbody rigidTarget){
		float proportion;

		if (numEtape < this.listTempsPourProchaineEtape.Count && this.listTempsPourProchaineEtape [numEtape] != 0) {
			proportion = tempsSurEtapeEnCours / this.listTempsPourProchaineEtape [numEtape];
            Vector3 pointDestination = courbeEtape.getPointAtTime(proportion);

			//Si la cible a un rigidbody on applique la force, sinon on la télétransporte
			if (null != rigidTarget) {
				rigidTarget.MovePosition (pointDestination);
			} else {
				transformTarget.position = pointDestination;
			}
		}
	}


	private float calculTempsEnFonctionModeVitesse(float t, float vitesseIntial, int numEtape){
		//LineaireEtape, lineaireMoyenneTemps, ElastiqueEtape, ElasticAdaptative, elasticMoyenneTemps,bondEtape(ou elastic inverse),Bond moyenne temps
		float tMax = (numEtape < this.listTempsPourProchaineEtape.Count ? this.listTempsPourProchaineEtape [numEtape] : this.listTempsPourProchaineEtape [0]);

		float tRecalcule;
		switch (this.modeDeVitesse) {
		case "LineaireEtape":
			tRecalcule =t;
			return tRecalcule;
		case "ElastiqueEtape" :
			//On considère une accéleration de type sinus (A*sin(2PI*t/tmax))
			tRecalcule = (1-vitesseIntial)*(t-(2*Mathf.PI/tMax)*Mathf.Sin(2*Mathf.PI*t/tMax))+vitesseIntial*t;
			return tRecalcule;
		case "ElasticAdaptative":
			//
			break;
		case "BondEtape":
			//On considére l'accéleration de type ligne droite décroissante (-at+b)
			tRecalcule = 12*(1-vitesseIntial)*t*t/tMax*((tMax/2)-(t/3))+vitesseIntial*t;
			return tRecalcule;
		}
		return 0;
	}

	public GameObject getGameObject(){
		return gameObject;
	}

	public int getNbEtape () {
		return this.listEtape.Count;
	}
}
