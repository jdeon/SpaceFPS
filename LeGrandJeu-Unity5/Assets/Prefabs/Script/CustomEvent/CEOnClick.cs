using UnityEngine;
using System.Collections;

public class CEOnClick : CustomEventScript {


	void Update () {
	
		if ( Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.E))
			OnTriggered(this, this.gameObject);
	}
}
