using UnityEngine;
using System.Collections;

public abstract class Matrice {

	//public int[,] array;

	/**
	 * Renvoie le determinant de la matrice
	 * */
	public abstract float getDeterminant ();

	/**
	 * Inverse la matrice, si cela est possible
	 * */
	public abstract float inverse ();

	/**
	 * Additionne de matrice ayant les meme attribut
	 * */
	//public abstract void add(Matrice matrice2);

	/**
	 * Mulitplie la matrice par une constante
	 * */
	public abstract void produit(float k);

	/**
	 * Multiplie la matrice par l'autre matrice en paramètre
	 * */
	//public abstract Matrice produit(Matrice matrice2);

	/**
	 * Tranforme la matrice en sa  matrice transverse
	 * */
	public abstract void getTransverse ();

	//Rajouté une méthode static pour identité
}
