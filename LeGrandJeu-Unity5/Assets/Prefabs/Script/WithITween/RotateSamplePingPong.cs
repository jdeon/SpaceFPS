using UnityEngine;
using System.Collections.Generic;

public class RotateSamplePingPong : MonoBehaviour
{	
	public float time;
	public float Delay;
	public float AngleEnFraction;

	public float portionRetrait;
	public float portionTempsRetrait;

	private Rigidbody rigidB;
	private float timeDuring;
	private float timeRotationPrincipal;
	private Dictionary<float, float> accelerationByTime;

	void Start()
	{
		//FIXME supprimer itween
		//iTween.RotateBy(gameObject, iTween.Hash("x", AngleEnFraction, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", Delay, "time", time));
		Debug.Log(gameObject.name + " utilise RotateSamplePingPong");

		rigidB = GetComponent<Rigidbody>();

		float accelerationRetrait = 0;
		float accelerationRotation = 0;

		//Ttotal = timeRotationPrincipal (1 + 2 * portionTempsRetrait) = time
		timeRotationPrincipal = time / (1 + 2 * portionTempsRetrait);

		//R = a*t*t/2 => 2R/(t*t)
		if (time > 0) {
			//Retrait : R = portionRetrait * angle(Radian) ; t = TempsRetrait / temps total
			accelerationRetrait = 2 * (portionRetrait* AngleEnFraction * 2 * Mathf.PI) / Mathf.Pow(timeRotationPrincipal * portionTempsRetrait / time, 2);

			//Retrait : R = portionRetrait * angle(Radian) ; t = TempsRetr
			accelerationRotation = 2 * AngleEnFraction * 2 * Mathf.PI / Mathf.Pow(timeRotationPrincipal / time, 2);
		}

		initAccelerationByTime(accelerationRetrait, accelerationRotation);

		timeDuring = 0;
	}

    private void FixedUpdate()
    {
		float accelerationActuel = getAcceleration(timeDuring);
		rigidB.AddRelativeTorque(accelerationActuel*Time.deltaTime, 0, 0, ForceMode.VelocityChange);

		timeDuring += Time.deltaTime;

		if(timeDuring > time + Delay)
        {
			timeDuring = 0;
        }
	}

	private void initAccelerationByTime(float accelerationRetrait, float accelerationRotation)
    {
		accelerationByTime = new Dictionary<float, float>();

		if (portionTempsRetrait > 0)
		{
			accelerationByTime.Add(timeRotationPrincipal * portionTempsRetrait, -accelerationRetrait);
		}

		accelerationByTime.Add(time/2, accelerationRotation);
		accelerationByTime.Add(time - timeRotationPrincipal * portionTempsRetrait, -accelerationRetrait);

		if (portionTempsRetrait > 0)
		{
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

		return acceleration;
    }
}

