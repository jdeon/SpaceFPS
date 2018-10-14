using UnityEngine;
using System.Collections;

public abstract class ConditionEventAbstract : MonoBehaviour {

	protected bool isActive;

	// Use this for initialization
	void Start () {
		isActive = false;
	}

	public void activeEvent(){
		if (!this.isActive) {
			onChange ();
			this.isActive = true;
		}
	}

	public void desactiveEvent(){
		if (this.isActive) {
			onChange ();
			StartCoroutine (desactivate ());
		}
	}


	private IEnumerator desactivate(){
		yield return new WaitForEndOfFrame ();
		this.isActive = false;
		yield return null;
	}

	public abstract void onChange ();

	public bool getIsActive(){
		return this.isActive;
	}
}
