﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGestionTemps : MonoBehaviour {

	public float referentielTemps =1f;

	
	// Update is called once per frame
	void Update () {
		Time.timeScale = referentielTemps;
	}
}
