using UnityEngine;
using System.Collections;

public class FollowPathNoneV2 : MonoBehaviour {

	public string pathToFollow ="";
	public float time;
	public float delay;

	// Use this for initialization
	void Start () {
		iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath (pathToFollow), "time", time, "delay", delay, "looptype", iTween.LoopType.none, "easetype", iTween.EaseType.easeInOutSine));
	}

}
