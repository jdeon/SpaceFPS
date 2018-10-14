using UnityEngine;
using System.Collections;

public class CARotationParametre : CustomActionScript {

	public Transform from;
	public Transform to;
	public float speed = 0.1F;
	public float Tmax = 10F;


	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		float a = 0;
		while (a <= Tmax)
		{
			transform.rotation = Quaternion.Slerp (from.rotation, to.rotation, Time.deltaTime * speed);
			a += Time.deltaTime;
			yield return new WaitForSeconds (0.01f);
		}
		yield return null;
	}
}
