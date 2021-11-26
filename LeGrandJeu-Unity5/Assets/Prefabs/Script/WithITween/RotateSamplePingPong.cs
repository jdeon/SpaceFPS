using UnityEngine;
using System.Collections.Generic;

public class RotateSamplePingPong : RotateAbstract
{	
	private bool isRetour;

	void Start()
	{
		axeRotation = transform.right;
		base.Start();
		isRetour = false;
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

