using UnityEngine;
using System.Collections;

public abstract class ArmeAFeuAbstract : MonoBehaviour {

	public float delaiRechargement;
	public float tempsVieProjectil = 5f;

	private float tempsAvantTirer;


	// Use this for initialization
	void Start () {
		tempsAvantTirer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.tempsAvantTirer >= 0) {
            this.tempsAvantTirer -= Time.deltaTime;
		} else if (Input.GetMouseButtonDown (0) && transform.parent.name == Constantes.STR_TRANSFORM_ARME) {
			tirer ();
            this.tempsAvantTirer = this.delaiRechargement;
		}
	}

	protected abstract void tirer();

	protected IEnumerator destroyProjectile(GameObject projectil){
		if (this.tempsVieProjectil > 0) {
			yield return new WaitForSeconds(this.tempsVieProjectil);
			Destroy(projectil);
		}
		yield return null;
	}
}
