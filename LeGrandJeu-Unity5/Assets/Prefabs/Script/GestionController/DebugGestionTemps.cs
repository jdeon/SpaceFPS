using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGestionTemps : MonoBehaviour {

	public float referentielTemps =1f;

	void Start () {
		Object[] objects = Resources.FindObjectsOfTypeAll <AudioListener> ();
		foreach (Object obj in objects) {
			Debug.Log ("AudioLisner sur " + obj.name);
		}
	}

	// Update is called once per frame
	void Update () {
		Time.timeScale = referentielTemps;
	}
}
