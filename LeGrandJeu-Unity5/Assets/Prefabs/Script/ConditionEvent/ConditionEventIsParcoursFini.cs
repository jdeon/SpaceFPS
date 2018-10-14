using UnityEngine;
using System.Collections;

public class ConditionEventIsParcoursFini : ConditionEventAbstract {

	private suivreParcour suivreParcours;
	private bool conditionDesactive;
	
	// Use this for initialization
	void Start () {
		suivreParcours = transform.GetComponent<suivreParcour>();
		conditionDesactive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (null != suivreParcours && !conditionDesactive) {
			if(suivreParcours.getIsFini()){
				activeEvent();
				conditionDesactive = true;
			}else if (isActive){
				desactiveEvent();
			}
		}
	}
	
	public override void onChange(){
	}
}
