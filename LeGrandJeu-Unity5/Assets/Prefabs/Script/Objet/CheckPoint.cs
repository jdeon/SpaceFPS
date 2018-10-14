using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	private static int nbCheckPoint;


	public string libelle;

	public Transform transformRespawn;

	public bool checkPointDeSauvegarde;

	public bool actif;

	public bool checkPointDoubleSens;

	//Ordre des checkpoint
	private int idCheckPoint;

	void Start (){
		idCheckPoint = transform.GetSiblingIndex();
		nbCheckPoint++;
	}

	public static int getNbCheckPoint(){
		return nbCheckPoint;
	}

	public int getIdCheckPoint(){
		return idCheckPoint;
	}
}
