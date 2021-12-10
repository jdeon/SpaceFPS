using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnigmeClavierAToucheAbstract : EnigmeAbstract {

	protected Transform[] listeToucheTrie;

	private PlayerInputAction controller;
	private bool triggerUse;


	void Awake()
	{
		controller = new PlayerInputAction();
		controller.PlayerActions.Use.performed += ctx => {
			triggerUse = true;
		};
	}
	private void LateUpdate()
	{
		if (triggerUse)
		{
			OnUse();
			triggerUse = false;
		}
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
	public virtual void Start () {
		CursorCustom.Activate = true;

		listeToucheTrie = new Transform[transform.childCount];
		List<Transform> listToucheATrier = new List<Transform>();
		Transform[] listeToucheInit = new Transform[transform.childCount]; 
		if (transform.childCount > 0) {
			for (int numChild =0; numChild < transform.childCount; numChild++) {
				listeToucheInit[numChild] = transform.GetChild(numChild);
				listToucheATrier.Add(transform.GetChild(numChild));
			}
			trierListeTouche(listToucheATrier, listeToucheInit.Length);
		}
	}


	/**
	 * Trier les transforms des touches dans un tableau par position (par x puis par z)
	 * */
	private void trierListeTouche(List<Transform> listToucheATrier, int nbDeTouche){
		for( int numTouche = 0 ; numTouche < nbDeTouche ; numTouche++){
			Transform toucheARajoute = null;
			foreach (Transform tansfTouche1 in listToucheATrier){
				toucheARajoute = tansfTouche1;
				foreach (Transform tansfTouche2 in listToucheATrier){
					if(!toucheARajoute.Equals(tansfTouche2) && tansfTouche2.localPosition.z < toucheARajoute.localPosition.z ||
					   (tansfTouche2.localPosition.z == toucheARajoute.localPosition.z && tansfTouche2.localPosition.x > toucheARajoute.localPosition.x)){
						toucheARajoute = tansfTouche2;
					}
				}
			}
			
			if ( null != toucheARajoute) {
				toucheARajoute.gameObject.AddComponent <ToucheClavierCliquable>();
				toucheARajoute.gameObject.AddComponent<BoxCollider>();
				listeToucheTrie[numTouche] = toucheARajoute;
				listToucheATrier.Remove(toucheARajoute);
				if (listToucheATrier.Count == 1){
					toucheARajoute = listToucheATrier[0];
					toucheARajoute.gameObject.AddComponent <ToucheClavierCliquable>();
					toucheARajoute.gameObject.AddComponent<BoxCollider>();
					listeToucheTrie[numTouche + 1] = toucheARajoute;
					break;
				}
			}
		}
	}

	protected abstract void OnUse();

	protected void resolutionEnigme(){
		CursorCustom.Activate = false;
		enigmeResolu = true;
	}
}
