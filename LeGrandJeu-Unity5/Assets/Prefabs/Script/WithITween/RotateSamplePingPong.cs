using UnityEngine;
using System.Collections;

public class RotateSamplePingPong : MonoBehaviour
{	
	public float time;
	public float Delay;
	public float AngleEnFraction;

	void Start(){
		iTween.RotateBy(gameObject, iTween.Hash("x", AngleEnFraction, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", Delay, "time", time));
	}
}

