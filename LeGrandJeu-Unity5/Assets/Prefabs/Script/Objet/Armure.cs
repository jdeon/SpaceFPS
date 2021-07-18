using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armure {

    public Material[] listCouleurPV; // 3 couleurs, vert, orange et rouge
    public int protectionMax;

	private int protectionRestante;
	private bool briser;
	private Transform refAffichage;


	/** 
	 * Constructeur avec la protection de l'armure et la transform de l'affichage
	 * */
	public Armure(int protectionMax, Transform refAffichage){
		this.protectionMax = protectionMax;
		this.protectionRestante = protectionMax;
		this.briser = false;
		this.refAffichage = refAffichage;
	}

	/** 
	 * Constructeur avec la protection de l'armure et la transform de l'affichage
	 * */
	public Armure(int protectionMax, Transform refAffichage, Material[] listCouleurPV){
		this.protectionMax = protectionMax;
		this.protectionRestante = protectionMax;
		this.briser = false;
		this.refAffichage = refAffichage;
		this.listCouleurPV = listCouleurPV;
		modificationGUI (this.protectionRestante);
	}

	/**
	 * En cas de degat subis, l'armure absorbe le maximum de degat et renvoie en retour les degats non absorbe
	 * */
	public int absorberDegat(int degatSubis){
		int degatRestant = degatSubis;
		if (degatSubis > this.protectionRestante) {
			degatRestant = degatSubis - this.protectionRestante;
			this.protectionRestante = 0;
			this.briser = true;
		} else {
			this.protectionRestante -= degatSubis;
			degatRestant = 0;
		}

		modificationGUI (this.protectionRestante);

		return degatRestant;
	}

	/**
	 * Repare une armure cassé et la régénère
	 * */
	public void reparation(int montantReparation){
		this.briser = false;

		this.protectionRestante += montantReparation;
		if (this.protectionRestante > this.protectionMax) {
			this.protectionRestante = this.protectionMax;
		}

		modificationGUI (this.protectionRestante);
	}

	/**
	 * Regenere une armure cassé non brisee
	 * */
	public void regeneration (int pointArmureAjout){
		if (!this.briser) {
			this.protectionRestante += pointArmureAjout;
			if (this.protectionRestante > this.protectionMax) {
				this.protectionRestante = this.protectionMax;
			}
			modificationGUI (this.protectionRestante);
		}
	}

	public void amelioration (int pointArmureAjout){
		this.briser = false;

		this.protectionRestante += pointArmureAjout;
		if (this.protectionRestante < this.protectionMax) {
			this.protectionMax = this.protectionRestante;
		}

		modificationGUI (this.protectionRestante);
	}

	/**
	 * Gestion de laffichage des point d armure
	 * */
	public void modificationGUI(int pointDArmure)
    {
        if (null != listCouleurPV && listCouleurPV.Length > 0)
        {
			if ((float)pointDArmure / (float)this.protectionMax <= 1f / 5f || listCouleurPV.Length == 1)
            {
				affichagePV(pointDArmure, listCouleurPV[0]);
            }
			else if ((float)pointDArmure / (float)this.protectionMax <= 3f / 5f || listCouleurPV.Length == 2)
            {
				affichagePV(pointDArmure, listCouleurPV[1]);
            }
            else
            {
				affichagePV(pointDArmure, listCouleurPV[2]);
            }
        }
    }

	/**
	 * Suppression et creation de cube de point d armure
	 * */
    public void affichagePV(int nbPV, Material couleurPV)
    {
		//Destruction ancien affichage
		for (int indexChild = 0; indexChild < refAffichage.childCount; indexChild++) {
			GameObject.Destroy(refAffichage.GetChild(indexChild).gameObject);
		}

		//Creation nouvelle affichage
        for (int idxPV = 1; idxPV <= nbPV; idxPV++)
        {
            float positionX = .125f + .025f * ((float)idxPV);
            GameObject blocPV = GameObject.CreatePrimitive(PrimitiveType.Cube);
			blocPV.transform.SetParent(this.refAffichage);
            blocPV.name = "PV_" + idxPV;
            blocPV.transform.localPosition = new Vector3(positionX, 0.75F, 0.5f);
            blocPV.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            blocPV.GetComponent<Renderer>().sharedMaterial = couleurPV;
        }
    }

	public bool isBrisee(){
		return this.briser;
	}
}