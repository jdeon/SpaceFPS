using UnityEngine;
using System.Collections;

public class CAChangementDeReference : CustomActionScript {

	public Transform from;
	public Transform to;
	public float speedRotate = 0.1F;
	public float speedMove = 5F;
	public float Tmax = 10F;
	public bool changerParent = false;


	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		float a = 0;
		transform.rotation = from.rotation;
		transform.position = from.position;

		while (a <= Tmax)
		{
			if (speedRotate > 0) {
				transform.rotation = Quaternion.Slerp (transform.rotation, to.rotation, Time.deltaTime * speedRotate);
			}
			transform.position = Vector3.MoveTowards(transform.position, to.position, Time.deltaTime * speedMove);
			a += Time.deltaTime;
			yield return new WaitForSeconds (0.01f);
		}

		if (speedRotate > 0) {
			transform.rotation = to.rotation;
		}
		transform.position = to.position;

		if (changerParent) {
			transform.SetParent(to);
		}

		yield return null;
	}
}
