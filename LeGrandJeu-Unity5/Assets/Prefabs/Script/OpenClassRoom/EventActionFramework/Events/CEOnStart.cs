using UnityEngine;
using System.Collections;

public class CEOnStart : CustomEventScript {

	// Use this for initialization
	void Start () {
	
		OnTriggered(this, this.gameObject);
	}
}
