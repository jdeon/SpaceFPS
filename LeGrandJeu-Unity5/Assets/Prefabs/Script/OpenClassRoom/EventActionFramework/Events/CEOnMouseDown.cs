using UnityEngine;
using System.Collections;

public class CEOnMouseDown : CustomEventScript {
	
	// Use this for initialization
	void OnMouseDown(Collision col) {
		
		OnTriggered(this, col.gameObject);
	}
}
