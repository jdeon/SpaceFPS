using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEOnSecondAfterStart : CustomEventScript {

	public float secondToWait;

	// Use this for initialization
	void Start () {
		StartCoroutine (startEvent ());
	}

	public IEnumerator startEvent (){
		if(secondToWait > 0){
			yield return new WaitForSeconds(secondToWait);
		}
		OnTriggered (this, this.gameObject);
	}
}