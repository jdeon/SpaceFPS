using UnityEngine;
using System.Collections;

public class CAChangeGravity : CustomActionScript {

	public float xGrav;
	public float yGrav;
	public float zGrav;

	private Vector3 gravity = Physics.gravity;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		Physics.gravity = gravity;
		
		if (true) 
		{
			gravity.x = xGrav;
			gravity.y = yGrav;
			gravity.z = zGrav;
		}
		yield return null;
	}
}
