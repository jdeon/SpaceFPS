using UnityEngine;
using System.Collections;

public class ConditionEventEnigme : ConditionEventAbstract {

	private EnigmeAbstract enigmeAbstract;
	private bool conditionDesactive;

	// Use this for initialization
	void Start () {
		enigmeAbstract = transform.GetComponent<EnigmeAbstract>();
		conditionDesactive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (null != enigmeAbstract && !conditionDesactive) {
			if(enigmeAbstract.isEnigmeResolu()){
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
