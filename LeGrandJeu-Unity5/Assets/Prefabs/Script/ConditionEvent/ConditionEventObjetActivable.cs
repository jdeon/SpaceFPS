using UnityEngine;
using System.Collections;

public class ConditionEventObjetActif : ConditionEventAbstract {

	public int niveauActivation = 0;
	
	private ObjetActivable objetActivable;
	
	// Use this for initialization
	void Start () {
		objetActivable = transform.GetComponent<ObjetActivable>();
	}
	
	// Update is called once per frame
	void Update () {
		if (null != objetActivable) {
			if(objetActivable.getNivActivation() == niveauActivation){
				activeEvent();
			}else if (isActive){
				desactiveEvent();
			}
		}
	}

	public override void onChange(){
	}
}
