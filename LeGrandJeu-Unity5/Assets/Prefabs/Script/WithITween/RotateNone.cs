using UnityEngine;
using System.Collections;

public class RotateNone : RotateAbstract
{	
	public float AngleXEnFraction;
	public float AngleYEnFraction;
	public float AngleZEnFraction;
	
	void Start(){
		//FIXME supprimer itween
		//iTween.RotateBy(gameObject, iTween.Hash("x", AngleXEnFraction,"y",AngleYEnFraction, "z", AngleZEnFraction, "easeType", "easeInOutBack", "loopType", "none", "delay", Delay, "time", time));
		Debug.Log(gameObject.name + "utilise RotateNone");

		Vector3 rotation = new Vector3(AngleXEnFraction, AngleYEnFraction, AngleZEnFraction);
		AngleEnFraction = rotation.magnitude;
		axeRotation = rotation.normalized;

		base.Start();
	}

    protected override void postMovementProcess()
    {
		//Nothing
    }
}

