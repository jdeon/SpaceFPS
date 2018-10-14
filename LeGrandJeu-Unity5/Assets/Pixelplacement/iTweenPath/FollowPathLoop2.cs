using UnityEngine;
using System.Collections;

public class FollowPathLoop2 : MonoBehaviour {

	public string pathToFollow ="";
	public float time;

	// Use this for initialization
	void Start () {
		iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath (pathToFollow), "time", time, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.easeInSine));
	}

}
