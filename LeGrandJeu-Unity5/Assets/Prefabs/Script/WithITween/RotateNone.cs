using UnityEngine;
using System.Collections;

public class RotateNone : MonoBehaviour
{	
	public float time;
	public float Delay;
	public float AngleXEnFraction;
	public float AngleYEnFraction;
	public float AngleZEnFraction;
	
	void Start(){
		iTween.RotateBy(gameObject, iTween.Hash("x", AngleXEnFraction,"y",AngleYEnFraction, "z", AngleZEnFraction, "easeType", "easeInOutBack", "loopType", "none", "delay", Delay, "time", time));
	}
}

