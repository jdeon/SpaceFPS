using UnityEngine;
using System.Collections;

public class FollowPathNone : MonoBehaviour {

	public string pathToFollow ="";
	public float time;

	// Use this for initialization
	void Start () {
		iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath (pathToFollow), "time", time, "looptype", iTween.LoopType.none, "easetype", iTween.EaseType.easeInOutSine));
	}

}
