using UnityEngine;
using System.Collections;

public class RotateLoop : RotateAbstract
{	
	public float AngleXEnFraction;
	public float AngleYEnFraction;
	public float AngleZEnFraction;

	void Start(){
		//FIXME supprimer itween
		//iTween.RotateBy(gameObject, iTween.Hash("x", AngleXEnFraction,"y",AngleYEnFraction, "z", AngleZEnFraction, "easeType", "easeInOutBack", "loopType", "loop", "delay", Delay, "time", time));
		Debug.Log(gameObject.name + "utilise RotateLoop");

		Vector3 rotation = new Vector3(AngleXEnFraction, AngleYEnFraction, AngleZEnFraction);
		AngleEnFraction = rotation.magnitude;
		axeRotation = rotation.normalized;

		base.Start();
	}

	protected override void postMovementProcess()
	{
		tempsActuel = 0;
		vitesseActuel = 0;
		rotateTo( rotationOriginal);
	}
}

