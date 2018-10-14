using UnityEngine;
using System.Collections;

public class Matrice2D : Matrice {

	public float[,] array;
	public int nbLigne;
	public int nbColonne;
	
	public Matrice2D (){
	}

	public Matrice2D (int nbLigne, int nbColonne){
		this.array = new float[nbLigne,nbColonne];
		this.nbLigne = nbLigne;
		this.nbColonne = nbColonne;
	}

	public Matrice2D (float[,] matrice){
		this.array = matrice;
		this.nbLigne = matrice.GetLength(0);
		this.nbColonne = matrice.GetLength(1);
	}
		
	/**
	 * renvoie le détermonant d'une matrice carré et renvoie 0 si elle n'est pas carré
	 * code inspiré de celui ci http://codes-sources.commentcamarche.net/source/view/53708/1261417#browser
	 * */
	public override float getDeterminant(){
		float determinant = 1;
		int nbC = this.nbColonne;
		int nbL = this.nbLigne;
		float[,] matrice = this.array;
		int colonneValueMaxDeLigne = -1;
		float ecart, caseValue, caseValeurMax;

		//Verification que la matrice est carre
		if (nbC != nbL){
			return 0;
		}

		for (int numL=nbL-1;numL>0;numL--) {
			caseValeurMax=0;
			for (int numC=0;numC<=nbC;numC++) {
				ecart=Mathf.Abs(matrice[numL,numC]);
				if (ecart>caseValeurMax) {
					colonneValueMaxDeLigne=numC; 
					caseValeurMax=ecart;
				}
			}

			// permutation des linges h et k
			if (colonneValueMaxDeLigne!=numL) {
				// inversion du signe;
				determinant=-determinant; 
				
				for (int numC=0;numC<=numL;numC++) {
					caseValue=matrice[numC,colonneValueMaxDeLigne];
					matrice[numC,colonneValueMaxDeLigne]=matrice[numC,numL];
					matrice[numC,numL]=caseValue;
				}
			}

			caseValue = matrice[numL,numL];
			determinant*=caseValue;

			if ((float.IsInfinity(determinant)|| !float.IsNaN(determinant))||(determinant==0)) {
				return determinant;
			}


		// pour chaque ligne (sauf la dernière)
			for (int j=0;j<=numL;j++){
				float f=matrice[numL,j]/caseValue;
				for (int i=0; i<numL; i++){
					matrice[i,j] -= f*matrice[i,numL];
				}
			}
		}
		return determinant*matrice[0,0];
	}

	/**
	 * inverse la matrice actuel et renvoie son déterminant ou 0 si elle n'est pas carré
	 * code inspiré de celui ci http://codes-sources.commentcamarche.net/source/view/53708/1261417#browser
	 * */
	public override float inverse(){
		float determinant =1;
		int nbLig = this.nbLigne;
		int nbCol = this.nbColonne;
		float caseValue, valeurMaxDeLigne;
		int h = -1; // h : numColonne de la valeur max d'une ligne
		
		float[,] matrice = this.array;	
		int[] identity = new int[nbLig];
		
		// Seulement pour matrice carrée
		if (nbLig !=nbCol ){
			return 0;
		}
		
		for (int numLig=0;numLig<nbLig; numLig++) {
			valeurMaxDeLigne=0 ;
			for (int numCol=numLig ;numCol<nbCol ;numCol++){;
				caseValue=matrice[numLig,numCol];
				if(Mathf.Abs(caseValue)>valeurMaxDeLigne){
					valeurMaxDeLigne = Mathf.Abs(caseValue);
					h=numCol;
				}
			}
			
			// meilleur pivot = 0 ==> déterminant = 0
			if (valeurMaxDeLigne == 0) {
				return 0;	
			}
			
			identity [numLig] = h;
			valeurMaxDeLigne = matrice [numLig,h];
			determinant *= (h==numLig)? valeurMaxDeLigne: -valeurMaxDeLigne;
			matrice [numLig,h] = matrice [numLig,numLig];
			matrice [numLig,numLig] = 1;
			for(int numCol=0;numCol< nbCol; numCol++){
				matrice [numLig,numCol]/= valeurMaxDeLigne;
			}
			
			for (int j=0;j<nbCol;j++) {
				if(numLig != j){
					caseValue =matrice [j,h];
					matrice [j,h]=matrice [j,numLig];
					matrice [j,numLig]=0;
					if(caseValue !=0){
						for(int i=0; i < nbCol; i++){	
							matrice [j,i] -= caseValue *matrice [numLig,i];
						}
					}
				}
			}
		}
		
		// on réarrange les colonnes
		for ( int numLig=nbLig-1; numLig>=0 ; numLig--) {
			h=identity[numLig];
			for (int i = 0; i <nbCol;i ++) {
				caseValue = matrice [h,i];
				matrice [h,i] = matrice [numLig,i];
				matrice [numLig,i] = caseValue ;	
			}
		}
		
		// retourne le déterminant
		return determinant ;
		//autre methode possible : pour chaque case on calcul le déterminant de la sous matrice;	
	}

	/**
	 * renvoie la matrice somme de deux matrice
	 * */
	public Matrice2D add(Matrice2D matrice2){

		if (this.nbColonne != matrice2.nbColonne || this.nbLigne != matrice2.nbLigne) {
			return null;
		}

		Matrice2D matriceSomme = new Matrice2D (this.nbLigne, this.nbColonne);
		for (int nCol = 0; nCol<this.nbColonne; nCol++) {
			for (int nLig = 0; nLig<this.nbLigne; nLig++) {
				matriceSomme.array[nLig,nCol] = this.array[nLig,nCol] + matrice2.array[nLig,nCol];
			}
		}
		return matriceSomme;
	}

	/**
	 * Multiplie la matrice par une constante k
	 * */
	public override void produit(float k){
		for (int nCol = 0; nCol<this.nbColonne; nCol++) {
			for (int nLig = 0; nLig<this.nbLigne; nLig++) {
				this.array [nLig, nCol] = k*this.array [nLig, nCol];
			}
		}
	}

	/**
	 * Multiplie la matrice actuel avec une autre 
	 * Renvoi null si le nbCollone de la matrice actuel est différent du nb ligne de la seconde
	 * */
	public  Matrice2D produit(Matrice2D matrice2){

		float[,] matriceActuel = this.array;
		int tC = this.nbColonne;
		int tL = this.nbLigne;
		int rC = matrice2.nbColonne;
		int rL = matrice2.nbLigne;
		Matrice2D matriceProduit = new Matrice2D (tL, rC);

		if(tC != rL){
			return null;
		}

		for (int numLig1 = 0; numLig1< tL; numLig1++) {
			for (int numCol2 = 0; numCol2< rC; numCol2++) {
				float s = 0;
				for (int i = 0; i<tC; i++) {
					s += matriceActuel [numLig1, i] * matrice2.array [i, numCol2];
				}
				matriceProduit.array [numLig1, numCol2] = s;
			}
		}
		return matriceProduit;
	}

	/**
	 * Renvoie  la matrice tranverse
	 * */
	public override void getTransverse (){
		Matrice2D matriceTranverse = new Matrice2D (this.nbColonne, this.nbLigne);
		for (int nCol = 0; nCol<this.nbColonne; nCol++) {
			for (int nLig = 0; nLig<this.nbLigne; nLig++) {
				matriceTranverse.array[nCol, nLig] = this.array[nLig,nCol];
			}
		}
		this.array = matriceTranverse.array;
		this.nbLigne = matriceTranverse.nbLigne;
		this.nbColonne = matriceTranverse.nbColonne;
	}

	/**
	 * Remplis la matrice avec 0
	 * */
	public void remplirDeZero(){
		for (int nLig = 0; nLig<this.nbLigne; nLig++) {
			for (int nCol = 0; nCol<this.nbColonne; nCol++) {
				this.array [nLig, nCol] = 0;
			}
		}
	}

	/**
	 * Renvoie la matrice inverse
	 * */
	public Matrice2D getInverse (){
		Matrice2D matriceInverse = this.copy();
		float determinant = matriceInverse.inverse();
		return determinant != 0? matriceInverse : null;
	} 

	/**
	 * Copie la matrice
	 * */
	public Matrice2D copy(){
		Matrice2D matriceCopie = new Matrice2D (this.nbLigne, this.nbColonne);
		for (int nCol = 0; nCol<this.nbColonne; nCol++) {
			for (int nLig = 0; nLig<this.nbLigne; nLig++) {
				matriceCopie.array [nLig, nCol] = this.array [nLig, nCol];
			}
		}
		return matriceCopie;
	}

	/**
	 * Ecart abs max de this par rapport à la matrice unité
	 * */
	public 	float Ecart() {
		float absolu;
		float ecart =0;
		float[,] matrice = this.array;
		int N = 1 + this.nbColonne ;

		for(int numLig = 0;numLig<this.nbLigne; numLig++){
			for(int numCol =0; numCol<this.nbColonne;numCol++){
				absolu=Mathf.Abs((((this.nbLigne*numLig+numCol)%N)==0)?1-matrice[numLig,numCol]:matrice[numLig,numCol]);
				if(absolu>ecart){
					ecart=absolu;
				}
			}
		}
		return ecart;
	}

	/**
	 * Supprime une ligne
	 * */
	public void deleteRow(int indexRow) {
		if (indexRow < this.nbLigne) {
			float[,] nouvelleMatrice = new float [this.nbLigne - 1, this.nbColonne];
			for (int numLig = 0; numLig<this.nbLigne; numLig++) {
				for (int numCol = 0; numCol<this.nbColonne; numCol++) {
					if (numLig < indexRow) {
						nouvelleMatrice [numLig, numCol] = this.array [numLig, numCol];
					} else if (numLig > indexRow) {
						nouvelleMatrice [numLig - 1, numCol] = this.array [numLig, numCol];
					} 
				}
			}

			this.nbLigne --;
			this.array = nouvelleMatrice;
		}
	}

	/**
	 * Supprime une colonne
	 * */
	public	void deleteColumn(int indexColonne) {
		if (indexColonne < this.nbColonne) {
			float[,] nouvelleMatrice = new float [this.nbLigne, this.nbColonne - 1];
			for (int numLig = 0; numLig<this.nbLigne; numLig++) {
				for (int numCol = 0; numCol<this.nbColonne; numCol++) {
					if (numCol < indexColonne) {
						nouvelleMatrice [numLig, numCol] = this.array [numLig, numCol];
					} else if (numCol > indexColonne) {
						nouvelleMatrice [numLig, numCol - 1] = this.array [numLig, numCol];
					}
				}
			}

			this.nbColonne --;
			this.array = nouvelleMatrice;
		}
	}

	/**
	 * Renvoie la matrice sous forme de String avec le séparateur entre chaque colonne et le retourLigne entre la dernière colonne d'une ligne et la première de la suivante
	 * */
	public string toString(string separateur,string retourLigne){
		
		string matriceStr ="";
		
		for(int numLigne = 0; numLigne <this.nbLigne;numLigne ++){
			for(int numCol = 0; numCol <this.nbColonne;numCol ++){
				matriceStr += this.array[numLigne ,numCol ] + (numCol ==this.nbColonne -1 ? retourLigne : separateur);	
			}		
		}
		return matriceStr;	
	}

	/**
	 * Renvoie une matrice random avec le nombre de ligne demandé et des valeurs entre -lim et +lim avec autant de démal que demandé
	 * */
	public static Matrice2D random(int nLig,int nCol, float lim,int nbDecimal){
		
		Matrice2D matriceRandom = new Matrice2D(nLig,nCol);
		
		for(int numLigne = 0; numLigne <nLig;numLigne ++){
			for(int numCol = 0; numCol <nCol;numCol ++){
				float caseAleatoire = Mathf.RoundToInt(Mathf.Pow(10,nbDecimal)*Random.Range(-lim,lim))/Mathf.Pow(10,nbDecimal);
				matriceRandom.array[numLigne ,numCol ] = caseAleatoire ;
			}
		}
		return matriceRandom;
	}

	/**
	 * revoie la matrice identité [n,n]
	 * */
	public static Matrice2D getIdentite(int n){
		Matrice2D matriceIdentite = new Matrice2D(n,n);
		for(int numLig = 0;numLig<n; numLig++){
			for(int numCol =0; numCol<n;numCol++){
				matriceIdentite.array[numLig,numCol] = (numCol == numLig ? 1 : 0);
			}
		}
		return matriceIdentite;
	}
}
