using UnityEngine;
using System.Collections;

public class CEOnConditionValide : CustomEventScript {

	public ConditionEventAbstract[] listCondition;
	public int numConditionMin =0;
	public bool isEventUnique = true;

	private bool isFirstTime;
		// Use this for initialization
	void Start () {
		isFirstTime = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFirstTime) {
			int numConditionActives = 0;
			for (int i = 0; i < listCondition.Length; i++) {
				if (listCondition [i].getIsActive ()) {
					numConditionActives++;
				}
			}

			if (numConditionActives >= numConditionMin) {
				OnTriggered (this, this.gameObject);
				desactiverToutesLesConditions ();
				if (isEventUnique){
					isFirstTime = false;
				}
			}
		}
	}

	void desactiverToutesLesConditions(){
		for (int i = 0 ; i < listCondition.Length; i++){
			listCondition[i].desactiveEvent();
		}
	}
}
