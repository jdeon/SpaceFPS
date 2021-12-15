using UnityEngine;
using System.Collections;

public class EnigmeClavierSaisiCode : EnigmeClavierAToucheAbstract {


	public int[] listNumToucheValid;

	//0 : defaultMaterial, 1 : MaterialValid , 2 : MaterialError
	public Material[] listMaterial;

	private int numToucheActive = 0;

	protected override void OnUse () {
		if (!enigmeResolu) {
			for(int numTouche = 0; numTouche < listeToucheTrie.Length ; numTouche++){
				Transform tranfTouche = listeToucheTrie[numTouche];
				if(null != tranfTouche && null != tranfTouche.GetComponent<ToucheClavierCliquable>() && !tranfTouche.GetComponent<ToucheClavierCliquable>().getIsClickTraite()){
					if (isInToucheValide(numTouche)){
						if(listMaterial.Length >=2){
							tranfTouche.GetComponent<Renderer>().sharedMaterial = listMaterial[1];
						}
						numToucheActive++;
						if(numToucheActive == listNumToucheValid.Length){
							enigmeResolu = true;
							CursorCustom.Activate = false;
						}
					} else {
						if(listMaterial.Length >=3){
							tranfTouche.GetComponent<Renderer>().sharedMaterial = listMaterial[2];
						}
						numToucheActive = 0;
						StartCoroutine("desactivateAllTouche", numTouche);
					}
					tranfTouche.GetComponent<ToucheClavierCliquable>().setIsClickTraite(true);
					break;
				}
			}
		}
	}

	private IEnumerator desactivateAllTouche(int numMauvaiseTouche){
		yield return  new WaitForSeconds (.5f);
		for (int indexToucheValide = 0; indexToucheValide < listNumToucheValid.Length; indexToucheValide++) {
			int numToucheValide = listNumToucheValid [indexToucheValide];
			this.listeToucheTrie [numToucheValide].GetComponent<Renderer>().sharedMaterial = listMaterial [0];
		}
		this.listeToucheTrie [numMauvaiseTouche].GetComponent<Renderer>().sharedMaterial = listMaterial [0];
	}
	

	private bool isInToucheValide (int numTouche){
		for (int indexToucheValide = 0 ; indexToucheValide < listNumToucheValid.Length; indexToucheValide++){
			if ( listNumToucheValid[indexToucheValide] == numTouche){
				return true;
			}
		}
		return false;
	}
}
