using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour {

	public float referentielTemps =1f;

	public int debbugCheckPoint;

	void Start () {
		Object[] objects = Resources.FindObjectsOfTypeAll <AudioListener> ();
		foreach (Object obj in objects) {
			Debug.Log ("AudioLisner sur " + obj.name);
		}
	}

	// Update is called once per frame
	void Update () {
		if (referentielTemps != 1f) {
			Time.timeScale = referentielTemps;
		}
	}
}
