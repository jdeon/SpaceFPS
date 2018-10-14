using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnigmeAbstract : MonoBehaviour {

	protected bool enigmeResolu = false;

	public bool isEnigmeResolu(){
		return this.enigmeResolu;
	}
}
