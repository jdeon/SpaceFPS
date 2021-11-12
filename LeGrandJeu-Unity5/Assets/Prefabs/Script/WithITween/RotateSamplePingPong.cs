using UnityEngine;
using System.Collections;

public class RotateSamplePingPong : MonoBehaviour
{	
	public float time;
	public float Delay;
	public float AngleEnFraction;

	void Start(){
		//FIXME supprimer itween
		//iTween.RotateBy(gameObject, iTween.Hash("x", AngleEnFraction, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", Delay, "time", time));
		Debug.Log(gameObject.name + "utilise RotateSamplePingPong");
	}
}

