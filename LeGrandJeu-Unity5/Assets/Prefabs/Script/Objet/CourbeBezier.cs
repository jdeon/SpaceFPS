using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CourbeBezier : System.Object {

	private List<Vector3> listPoint;

	/**
	 * Constructeur a partir de 2 points
	 **/
	public CourbeBezier(Vector3 point1, Vector3 point2){
		listPoint = new List<Vector3>();
		listPoint.Add(point1);
		listPoint.Add(point2);
	}

	/**
	 * Constructeur a partir de 3 points
	 **/
	public CourbeBezier(Vector3 point1, Vector3 point2, Vector3 point3){
		listPoint = new List<Vector3>();
		listPoint.Add(point1);
		listPoint.Add(point2);
		listPoint.Add(point3);
	}

	/**
	 * Constructeur a partir de 4 points
	 **/
	public CourbeBezier(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4){
		listPoint = new List<Vector3>();
		listPoint.Add(point1);
		listPoint.Add(point2);
		listPoint.Add(point3);
		listPoint.Add(point4);
	}

	/**
	 * Constructeur a partir d'une list de points
	 **/
	public CourbeBezier(List<Vector3> listPoint){
		this.listPoint = listPoint;
	}

    public Vector3 getStartPoint()
    {
        if(null != listPoint && listPoint.Count > 0)
        {
            return listPoint[0];
        }
        return Vector3.zero;
    }

    public Vector3 getEndPoint()
    {
        if (null != listPoint && listPoint.Count > 0)
        {
            return listPoint[listPoint.Count-1];
        }
        return Vector3.zero;
    }

    /**
	 * Permet d'obtenir le premier ou le dernier point de controle d'une Bezier Cubique
	 * Rajoute coefficient 1/2 ou 1/3 pour quadratique ou cubique 
	 * */
    public static Vector3 getPremierPointControle(Vector3 point0, Vector3 point1, Vector3 point2){
		Vector3 pointDeControle;
		Vector3 vecteur01 = new Vector3 ((point1.x - point0.x), (point1.y - point0.y), (point1.z - point0.z));
		Vector3 vecteur21 = new Vector3 ((point1.x - point2.x), (point1.y - point2.y), (point1.z - point2.z));
		Vector3 vecteurHorsPlan = Vector3.Cross (vecteur01, vecteur21);
		Vector3 vecteurNormal = Vector3.Cross (vecteurHorsPlan, vecteur01);
		Vector3 vecteurTangentUnitaire = vecteur01.normalized;
		Vector3 vecteurNormalUnitaire = vecteurNormal.normalized;

		//O2 = a*Normal+b*Tangent
		//b = (O2.y-a*normal.y)/ Tangent.y
		//O2.x = a*normal.x + b * Tangent.x
		float a = ((point2.x - point0.x) * vecteurTangentUnitaire.y - (point2.y - point0.y) * vecteurTangentUnitaire.x) / (vecteurNormalUnitaire.x * vecteurTangentUnitaire.y - vecteurNormalUnitaire.y * vecteurTangentUnitaire.x);
		float b = ((point2.y - point0.y)- a * vecteurNormalUnitaire.y)/vecteurTangentUnitaire.y;
		Vector2 vecteurPourNormalise = new Vector2 (a, b);
		//Angle entree la normal et le point de controle
		float alpha = (Mathf.PI / 8f) - vecteurPourNormalise.normalized.x * (Mathf.PI / 8f);

		//Calcul du point de controle
		pointDeControle = point0 + (Mathf.Cos (alpha) * vecteurNormalUnitaire + Mathf.Sin (alpha) * vecteurTangentUnitaire) * vecteur01.magnitude;
		return pointDeControle;
	}

	/**
	 * Renvoie le prochain point de controle pour que la continuété de la courbe soit C1
	 * */
	public static Vector3 getPointControleSuivantAvecContinuiteC1(Vector3 PointControlePrecedent, Vector3 PointLiason){
		Vector3 pointControleSuivant = PointLiason - (PointControlePrecedent - PointLiason);
		return pointControleSuivant;
	}

	/**
	 * Renvoie le prochain point de controle pour que la continuété de la courbe soit G1
	 * on utilise la formule suivante (PC1-PL)/(PP-PL)=(PC2-PL)/(PS-PL)
	 * avec PC1 = PointControlePrecedent; PL = PointLiason ; PP = PointCourbePrecedent; PC2 = pointControleSuivant; PS = PointCourbeSuivant
	 * C2 = PC1* facteurPointDeControle + PL * facteurPointDeLiason
	 * */
	public static Vector3 getPointControleSuivantAvecContinuiteG1(Vector3 pointCourbePrecedent, Vector3 pointControlePrecedent, Vector3 pointLiason, Vector3 pointCourbeSuivant){

		//Initalisation en continuité C1
		Vector3 pointControleSuivant = pointLiason - (pointCourbePrecedent - pointLiason);

		float distanceP1PL = (pointCourbePrecedent - pointLiason).magnitude;

		if (distanceP1PL != 0) {
			float distanceP2PL = (pointCourbeSuivant - pointLiason).magnitude;

			//Attention les distance sont absolue mais le sens est different, d'ou le "-" qui apparait dans facteurPointDeControle et la differrence qui ce transforme en somme dans facteurPointDeLiason
			float facteurPointDeControle = - distanceP2PL / distanceP1PL;
			float facteurPointDeLiason = (distanceP1PL + distanceP2PL) / distanceP1PL;

			pointControleSuivant = pointControlePrecedent * facteurPointDeControle + pointLiason * facteurPointDeLiason;
		}
		return pointControleSuivant;
	}

	/**
	 * Renvoie la matrice de coefficient pour calculer les points de controle pour une boucle
	 * Rem : pour une non-boucle, supprimer les 2 première ligne et la première et dernière colonnecolonne
	 * Le produit de cette matrice avec celle comportant  : ligne0 = 2*PointCOntole1, ligne paire 2*pointDeParcourEnsuite, lignemax-1 -2*DernierPC pour renvoyer la matrice de tous les PC hormis le premier et le dernier 
	 * */
	public static Matrice2D getMatriceCoeficientPourContinueteC2 (int nbPointEtape){

		Matrice2D matriceCoef = new Matrice2D (nbPointEtape *2, nbPointEtape *2);
		matriceCoef.remplirDeZero ();
			
		for (int numLig=0; numLig < nbPointEtape *2; numLig++) {
			if (numLig == 0) {
				matriceCoef.array [numLig, nbPointEtape *2 -1] = 1;
				matriceCoef.array [numLig, 0] = 1;
				numLig ++;
				matriceCoef.array [numLig, nbPointEtape *2 -2] = -2;
				matriceCoef.array [numLig, nbPointEtape *2 -1] = 4;
				matriceCoef.array [numLig, 0] = -4;
				matriceCoef.array [numLig, 1] = 2;
			} else if (numLig % 2 == 0) {
				matriceCoef.array [numLig, numLig - 1] = 1;
				matriceCoef.array [numLig, numLig] = 1;
			} else {
				matriceCoef.array [numLig, numLig - 3] = -2;
				matriceCoef.array [numLig, numLig - 2] = 4;
				matriceCoef.array [numLig, numLig - 1] = -4;
				matriceCoef.array [numLig, numLig] = 2;
			}
		}
		return matriceCoef;
	}


	/**
	 * Permet d'obtenir le point à la proportion t de la courbe
	 *  0.0 >= t <= 1.0
	 * */
	public Vector3 getPointAtTime(float t)  {
		Vector3 point;

		switch (this.listPoint.Count){
		case 2 :
			point = getPointOfBezierLineaireAtTime(this.listPoint, t);
			break;
		case 3 :
			point = getPointOfBezierQuadratiqueAtTime(this.listPoint, t);
			break;
		case 4:
			point = getPointOfBezierCubiqueAtTime(this.listPoint, t);
			break;
		default :
			point.x = 0;
			point.y = 0;
			point.z = 0;
			break;
		}
		return point;
	}

	/**
	 * Equation d'une Bezier lineaire
	 * */
	private Vector3	getPointOfBezierLineaireAtTime(List<Vector3> listPoint,float t){
		Vector3 pointIntitial = listPoint[0];
		Vector3 pointFinal = listPoint[1];
		return pointIntitial * (1 - t) + pointFinal * t;
	}

	/**
	 * Equation d'une Bezier quadratique
	 * */
	private Vector3	getPointOfBezierQuadratiqueAtTime(List<Vector3> listPoint,float t){
		Vector3 pointIntitial = listPoint[0];
		Vector3 pointControl1 = listPoint[1];
		Vector3 pointFinal = listPoint[2];

		return pointIntitial * (1 - t) * (1 - t) + 2 * pointControl1 * t * (1 - t) + pointFinal * t * t;
	}

	/**
	 * Equation d'une Bezier cubique
	 * */
	private Vector3	getPointOfBezierCubiqueAtTime(List<Vector3> listPoint,float t){
		Vector3 pointIntitial = listPoint[0];
		Vector3 pointControl1 = listPoint[1];
		Vector3 pointControl2 = listPoint[2];
		Vector3 pointFinal = listPoint[3];

		return pointIntitial * (1 - t) * (1 - t) * (1 - t) + 3 * pointControl1 * t * (1 - t) * (1 - t) + 3 * pointControl2 * t * t * (1 - t) + pointFinal * t * t * t;
	}
}