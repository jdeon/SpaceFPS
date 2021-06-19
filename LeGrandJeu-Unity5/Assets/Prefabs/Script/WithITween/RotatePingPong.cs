using UnityEngine;
using System.Collections;

public class RotatePingPong : MonoBehaviour
{	
	public float time;
	public float Delay;
	public float AngleXEnFraction;
	public float AngleYEnFraction;
	public float AngleZEnFraction;
	
	void Start(){
		//FIXME
		//iTween.RotateBy(gameObject, iTween.Hash("x", AngleXEnFraction,"y",AngleYEnFraction, "z", AngleZEnFraction, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", Delay, "time", time));
	}
}

