using UnityEngine;
using System.Collections;

public class ConditionEventObjetPortable : ConditionEventAbstract {

	public int niveauActivation = 0;
	
	private ObjetPortable objetPortable;
	
	// Use this for initialization
	void Start () {
		objetPortable = transform.GetComponent<ObjetPortable>();
	}
	
	// Update is called once per frame
	void Update () {
		if (null != objetPortable) {
			if(objetPortable.getNumEtat() == niveauActivation){
				activeEvent();
			}else if (isActive){
				desactiveEvent();
			}
		}
	}

	public override void onChange(){
	}
}
