using UnityEngine;
using System.Collections;

public class CARotationSurCible : CustomActionScript {

	public Transform to;
	public float speedRotate = 0.1F;
	public float Tmax = 10F;
	public GameObject Target;


	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{

		float a = 0;
		while (a <= Tmax)
		{
			Target.transform.rotation = Quaternion.Slerp (Target.transform.rotation, to.rotation, Time.deltaTime * speedRotate);
			a += Time.deltaTime;
			yield return new WaitForSeconds (0.01f);
		}

		Target.transform.rotation = to.rotation;
		yield return null;
	}
}
