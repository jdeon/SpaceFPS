using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("PathTest"), "time", 5, "looptype", iTween.LoopType.loop));
	}

}
