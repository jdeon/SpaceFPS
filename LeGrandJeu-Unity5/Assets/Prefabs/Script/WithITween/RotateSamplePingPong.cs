using UnityEngine;
using System.Collections.Generic;

/**
 * Ojectif du script
 * faire un retrait avant la rotation de time * proportion
 * acceleraration durant time * (1-2proportion) /4
 * vitesse stable durant durant time * (1-2proportion) /2
 * deceleration durant time * (1-2proportion) /4
 * faire un retrait avant la rotation de time * proportion
 */
public class RotateSamplePingPong : MonoBehaviour
{	
	public float time;
	public float Delay;
	public float AngleEnFraction;

	public float portionRetrait;
	public float portionTempsRetrait;

	private Rigidbody rigidB;
	private float timeRotationPrincipal;
	private Dictionary<float, float> accelerationByTime;
	private Quaternion rotationOriginal;
	private bool isRetour;

	private float vitesseActuel;
	private float tempsActuel;

	void Start()
	{
		if (portionRetrait == 0)
		{
			Debug.Log(gameObject.name + " utilise RotateSamplePingPong");
		}

		rigidB = GetComponent<Rigidbody>();
		if(null != rigidB)
        {
			rigidB.isKinematic = true;
			Quaternion rotation = rigidB.rotation;
			rotationOriginal = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
		} else
        {
			Quaternion rotation = transform.rotation;
			rotationOriginal = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
		}

		float accelerationRetrait = 0;
		float accelerationRotation = 0;

		//Ttotal = timeRotationPrincipal (1 + 2 * portionTempsRetrait) = time
		timeRotationPrincipal = time / (1 + 2 * portionTempsRetrait);

		//r = a*t*t/2 => 2R/(t*t) avec R = r/2 et T = t/2
		if (time > 0) {
			//Retrait : R = portionRetrait * angle(Radian) ; t = TempsRetrait / temps total
			//R = aTT/2 => a = 2R/TT
			accelerationRetrait = 2 * (portionRetrait* AngleEnFraction * 360 / 2) / Mathf.Pow(timeRotationPrincipal * portionTempsRetrait / 2, 2);

			//1/2a*t1*t1 =R/4 et v*t2 = R/2 => a*t1*t1 = v*t2 = a*t1*t2 => t1=t2 avec 2*t1 + t2 = T => t1=T/3
			//a =R/(2*t1*t1) = R/(2*(T/3)*(T/3) = 9R/2T
			//TODO comprendre pourquoi le facteur est 3 au lieu de 2 pour la portionRetrait
			accelerationRotation = 9 * AngleEnFraction * (1+3*portionRetrait) * 360 / (2*Mathf.Pow(timeRotationPrincipal, 2));
		}

		initAccelerationByTime(accelerationRetrait, accelerationRotation);

		tempsActuel = 0;
		vitesseActuel = 0;
		isRetour = false;
	}

    private void FixedUpdate()
    {
		float accelerationActuel = getAcceleration(tempsActuel);

		float t = Time.fixedDeltaTime; 
		float rotationActuel = accelerationActuel * Mathf.Pow(t, 2)/2 + vitesseActuel * t;

		if(null != rigidB)
        {
			rigidB.MoveRotation(rigidB.rotation * Quaternion.Euler(rotationActuel, 0, 0));
        } else
        {
			transform.Rotate(rotationActuel, 0, 0, Space.Self);
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
				destination = rotationOriginal * Quaternion.Euler(AngleEnFraction * 360, 0, 0);
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

		if (tempsActuel > time + Delay)
        {
			tempsActuel = 0;
			isRetour = !isRetour;
        }
	}

	private void initAccelerationByTime(float accelerationRetrait, float accelerationRotation)
    {
		accelerationByTime = new Dictionary<float, float>();

		if (portionTempsRetrait > 0)
		{
			accelerationByTime.Add(timeRotationPrincipal * portionTempsRetrait /2, -accelerationRetrait);
			accelerationByTime.Add(timeRotationPrincipal * portionTempsRetrait, accelerationRetrait);
		}

		//Si t1=t2 alors le rotation est séparé en 3 tiers acceleration, constant ralentissement
		accelerationByTime.Add(timeRotationPrincipal * (portionTempsRetrait + 1f/3f), accelerationRotation);
		accelerationByTime.Add(timeRotationPrincipal * (portionTempsRetrait + 2f/3f), 0);
		accelerationByTime.Add(time - timeRotationPrincipal * portionTempsRetrait, -accelerationRotation);

		if (portionTempsRetrait > 0)
		{
			accelerationByTime.Add(time - timeRotationPrincipal * portionTempsRetrait / 2, -accelerationRetrait);
			accelerationByTime.Add(time, accelerationRetrait);
		}
	}

	private float getAcceleration(float time)
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

		if (isRetour)
		{
			acceleration = -1f * acceleration;
		}

		return acceleration;
    }
}

