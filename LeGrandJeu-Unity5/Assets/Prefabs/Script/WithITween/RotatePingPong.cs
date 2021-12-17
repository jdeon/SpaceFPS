using UnityEngine;
using System.Collections;

public class RotatePingPong : RotateAbstract
{	
	public float AngleXEnFraction;
	public float AngleYEnFraction;
	public float AngleZEnFraction;

	private bool isRetour;

	new void Start(){
		//FIXME supprimer itween
		//iTween.RotateBy(gameObject, iTween.Hash("x", AngleXEnFraction,"y",AngleYEnFraction, "z", AngleZEnFraction, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", Delay, "time", time));
		Debug.Log(gameObject.name + "utilise RotatePingPong");

		Vector3 rotation = new Vector3(AngleXEnFraction, AngleYEnFraction, AngleZEnFraction);
		AngleEnFraction = rotation.magnitude;
		axeRotation = rotation.normalized;

		base.Start();

		isRetour = false;
	}

	private void FixedUpdate()
	{
		float accelerationActuel = getAcceleration(tempsActuel);
		if (isRetour)
		{
			accelerationActuel *= -1f;
		}

		float t = Time.fixedDeltaTime;
		float rotationActuel = accelerationActuel * Mathf.Pow(t, 2) / 2 + vitesseActuel * t;

		if (null != rigidB)
		{
			rigidB.MoveRotation(rigidB.rotation * Quaternion.Euler(axeRotation * rotationActuel));
		}
		else
		{
			transform.Rotate(axeRotation * rotationActuel, Space.Self);
		}


		vitesseActuel += accelerationActuel * t;
		tempsActuel += t;

		if (tempsActuel > time)
		{
			vitesseActuel = 0;
			Quaternion destination;
			if (isRetour)
			{
				destination = rotationOriginal;
			}
			else
			{
				destination = rotationOriginal * Quaternion.Euler(axeRotation * AngleEnFraction * 360);
			}

			if (null != rigidB)
			{
				rigidB.MoveRotation(destination);
			}
			else
			{
				transform.rotation = destination;
			}
		}
	}

	protected override void movementProcess(float accelerationActuel, float deltaT)
    {
		if (isRetour)
		{
			accelerationActuel *= -1f;
		}

		base.movementProcess(accelerationActuel, deltaT);
	}

	protected override void delayProcess()
	{
		vitesseActuel = 0;
		if (isRetour)
		{
			rotateTo(rotationOriginal * Quaternion.Euler(axeRotation * AngleEnFraction * 360));
		}
		else
		{
			base.delayProcess();
		}
	}

	protected override void postMovementProcess()
	{
		tempsActuel = 0;
		isRetour = !isRetour;
	}
}

