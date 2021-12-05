using UnityEngine;
using System.Collections;

public class EnigmeClavierToucheBinaire : EnigmeClavierAToucheAbstract {

	public int nbColonne;
	public int nbLigne;

	//0 : defaultMaterial, 1 : MaterialValid
	public Material[] listMaterial;
	
	private Transform[,] tableauDeTouche;

	private PlayerInputAction controller;


	void Awake()
	{
		controller = new PlayerInputAction();
		controller.PlayerActions.Use.performed += ctx => {
			OnUse();
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

	// Use this for initialization
	public override void Start () {
		base.Start();
		this.tableauDeTouche = new Transform[this.nbLigne,this.nbColonne];

		int index = 0;
		int numLigne = 0;
		int numColonne = 0;
		foreach (Transform touche in listeToucheTrie){
			//Nouvelle ligne
			if( index != 0 && listeToucheTrie[index-1].localPosition.z < touche.localPosition.z && numColonne >= this.nbColonne){
				numLigne ++;
				numColonne = 0;
			} 

			if(numLigne <= this.nbLigne){
				this.tableauDeTouche[numLigne, numColonne] = touche;
				numColonne++;
				index++;
			}
		}

		initBoutonActif();
	}
	
	// Update is called once per frame
	void OnUse() {
		if (!enigmeResolu) {
			for (int numLigne = 0; numLigne < this.nbLigne; numLigne++) {
				for (int numColonne = 0; numColonne < this.nbColonne; numColonne++) {
					Transform tranfTouche = this.tableauDeTouche [numLigne,numColonne];
					ToucheClavierCliquable scriptTouche = tranfTouche.GetComponent<ToucheClavierCliquable> ();
					if (null != tranfTouche && null != scriptTouche && !scriptTouche.getIsClickTraite ()) {
						CursorCustom.Activate = true;

						Transform[] tabCase = new Transform[5];
						tabCase[0] = tranfTouche;
						tabCase[1] = numLigne > 0 ? this.tableauDeTouche [numLigne-1,numColonne] : null;
						tabCase[2] = numColonne > 0 ? this.tableauDeTouche [numLigne,numColonne-1] : null;
						tabCase[3] = numColonne < (this.nbColonne - 1) ? this.tableauDeTouche [numLigne,numColonne+1] : null;
						tabCase[4] = numLigne < (this.nbLigne - 1) ? this.tableauDeTouche [numLigne+1, numColonne] : null;
						
						for (var numCase = 0 ; numCase < tabCase.Length ; numCase++){
							if(tabCase[numCase] != null && this.listMaterial.Length >= 2){
								tabCase[numCase].GetComponent<Renderer>().sharedMaterial = tabCase[numCase].GetComponent<Renderer>().sharedMaterial == this.listMaterial [1]? this.listMaterial [0] : this.listMaterial [1];
							}
						}

                        scriptTouche.setIsClickTraite(true);

						verificationVictoire();
						break;
					}
				}
			}
		}
	}
	
	private void initBoutonActif(){
		int[] coordCol;
		int[] coordLig;
	
		if(this.nbColonne % 2 == 0){
			coordCol = new int[]{this.nbColonne/2, (this.nbColonne-2)/2};
		} else {
			coordCol = new int[]{(this.nbColonne-1)/2};
		}

		if(this.nbLigne % 2 == 0){
			coordLig = new int[]{this.nbLigne/2, (this.nbLigne-2)/2};
		} else {
			coordLig = new int[]{(this.nbLigne-1)/2};
		}

		for(int idxCoordLigne = 0; idxCoordLigne < coordLig.Length ; idxCoordLigne++){
			int numLigne = coordLig[idxCoordLigne];
			for(int idxCoordCol = 0; idxCoordCol < coordCol.Length ; idxCoordCol++){
				int numColonne = coordCol[idxCoordCol];
				if(listMaterial.Length >=2){
					this.tableauDeTouche[numLigne,numColonne].GetComponent<Renderer>().sharedMaterial = this.listMaterial[1];
				}
			}
		}
	}

	
	private void verificationVictoire(){
		var victoire = true;
		for (int numLigne = 0; numLigne < this.nbLigne; numLigne++) {
			for (int numColonne = 0; numColonne < this.nbColonne; numColonne++) {
				if(this.tableauDeTouche [numLigne,numColonne].GetComponent<Renderer>().sharedMaterial == this.listMaterial[0]){
					victoire = false;
					break;
				}
			}
			
			if(!victoire){
				break;
			}
		}
		
		if(victoire){
			resolutionEnigme();
		}
	}

}
