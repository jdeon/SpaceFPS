using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script réalisé pour remplacer les appel à iTween.RotateBy
 * 
 * Ojectif du script
 * faire un retrait avant la rotation de time * proportion
 * acceleraration durant time * (1-2proportion) /4
 * vitesse stable durant durant time * (1-2proportion) /2
 * deceleration durant time * (1-2proportion) /4
 * faire un retrait avant la rotation de time * proportion
 * */
public abstract class  RotateAbstract : MonoBehaviour
{
	public float time;
	public float Delay;
	public float AngleEnFraction;

	public float portionRetrait;
	public float portionTempsRetrait;

	protected Rigidbody rigidB;
	protected float timeRotationPrincipal;
	protected Dictionary<float, float> accelerationByTime;
	protected Quaternion rotationOriginal;

	protected Vector3 axeRotation;
	protected float vitesseActuel;
	protected float tempsActuel;

	protected void Start()
	{
		if (portionRetrait == 0)
		{
			Debug.Log(gameObject.name + " utilise ancien script ITween " + GetType());
		}

		rigidB = GetComponent<Rigidbody>();
		if (null != rigidB)
		{
			rigidB.isKinematic = true;
			Quaternion rotation = rigidB.rotation;
			rotationOriginal = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
		}
		else
		{
			Quaternion rotation = transform.rotation;
			rotationOriginal = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
		}

		float accelerationRetrait = 0;
		float accelerationRotation = 0;

		//Ttotal = timeRotationPrincipal (1 + 2 * portionTempsRetrait) = time
		timeRotationPrincipal = time / (1 + 2 * portionTempsRetrait);

		//r = a*t*t/2 => 2R/(t*t) avec R = r/2 et T = t/2
		if (time > 0)
		{
			//Retrait : R = portionRetrait * angle(Radian) ; t = TempsRetrait / temps total
			//R = aTT/2 => a = 2R/TT
			accelerationRetrait = 2 * (portionRetrait * AngleEnFraction * 360 / 2) / Mathf.Pow(timeRotationPrincipal * portionTempsRetrait / 2, 2);

			//1/2a*t1*t1 =R/4 et v*t2 = R/2 => a*t1*t1 = v*t2 = a*t1*t2 => t1=t2 avec 2*t1 + t2 = T => t1=T/3
			//a =R/(2*t1*t1) = R/(2*(T/3)*(T/3) = 9R/2T
			//TODO comprendre pourquoi le facteur est 3 au lieu de 2 pour la portionRetrait
			accelerationRotation = 9 * AngleEnFraction * (1 + 3 * portionRetrait) * 360 / (2 * Mathf.Pow(timeRotationPrincipal, 2));
		}

		initAccelerationByTime(accelerationRetrait, accelerationRotation);

		tempsActuel = 0;
		vitesseActuel = 0;
	}

	private void FixedUpdate()
	{
		float t = Time.fixedDeltaTime;

		if(tempsActuel < Delay)
        {
			delayProcess();
		}
		else if (tempsActuel < Delay + time)
		{
			float accelerationActuel = getAcceleration(tempsActuel - Delay);
			movementProcess(accelerationActuel, t);
		} 
		else
        {
			postMovementProcess();
		}


		tempsActuel += t;
	}

	protected virtual void movementProcess(float accelerationActuel, float deltaT)
    {
		float rotationActuel = accelerationActuel * Mathf.Pow(deltaT, 2) / 2 + vitesseActuel * deltaT;
		rotateTo(axeRotation * rotationActuel);
		vitesseActuel += accelerationActuel * deltaT;
	}

	protected virtual void delayProcess()
	{
		vitesseActuel = 0;
		//Force la fin du cycle
		rotateTo(rotationOriginal);
	}

	protected abstract void postMovementProcess();

	private void initAccelerationByTime(float accelerationRetrait, float accelerationRotation)
	{
		accelerationByTime = new Dictionary<float, float>();

		if (portionTempsRetrait > 0)
		{
			accelerationByTime.Add(timeRotationPrincipal * portionTempsRetrait / 2, -accelerationRetrait);
			accelerationByTime.Add(timeRotationPrincipal * portionTempsRetrait, accelerationRetrait);
		}

		//Si t1=t2 alors le rotation est séparé en 3 tiers acceleration, constant ralentissement
		accelerationByTime.Add(timeRotationPrincipal * (portionTempsRetrait + 1f / 3f), accelerationRotation);
		accelerationByTime.Add(timeRotationPrincipal * (portionTempsRetrait + 2f / 3f), 0);
		accelerationByTime.Add(time - timeRotationPrincipal * portionTempsRetrait, -accelerationRotation);

		if (portionTempsRetrait > 0)
		{
			accelerationByTime.Add(time - timeRotationPrincipal * portionTempsRetrait / 2, -accelerationRetrait);
			accelerationByTime.Add(time, accelerationRetrait);
		}
	}

	protected float getAcceleration(float time)
	{
		float acceleration = 0;

		foreach (KeyValuePair<float, float> item in accelerationByTime)
		{
			if (time > item.Key)
			{
				continue;
			}

			acceleration = item.Value;
			break;
		}

		return acceleration;
	}

	protected void rotateTo(Vector3 rotationTarget)
    {
		if (null != rigidB)
		{
			rigidB.MoveRotation(rigidB.rotation * Quaternion.Euler(rotationTarget));
		}
		else
		{
			transform.Rotate(rotationTarget, Space.Self);
		}
	}

	protected void rotateTo(Quaternion rotationTarget)
	{
		if (null != rigidB)
		{
			rigidB.MoveRotation(rotationTarget);
		}
		else
		{
			transform.rotation = rotationTarget;
		}
	}
}
