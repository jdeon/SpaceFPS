using UnityEngine;
using System.Collections;

public class UnCheckIsKinematicForAllObjectInOnTriggerEnter : MonoBehaviour {

	[SerializeField]
	private Transform _transformParent;

	/* si on veut mettre des trait de liason
	 public void OnDrawGizmos()
	{
		if (_targetRigidbody != null)
			Debug.DrawLine(this.transform.position, _targetRigidbody.transform.position, Color.yellow);
	}*/

	public void OnTriggerEnter(Collider col)
	{
		foreach (Transform childTransform in _transformParent) {
			Rigidbody childRigidbody = childTransform.GetComponent<Rigidbody> ();
			if (null != childRigidbody) {
				childRigidbody.isKinematic = false;
				childRigidbody.WakeUp ();
			}
		}
	}
}

	/*
	 * Essayer avec object si cela ne marche pas
	 Rigidbody[] bodys = GetComponentsInChildren<Rigidbody>(); //Or GetComponentsInChildren<Rigidbody>(true) to get inactive children
 
 	for(var i=0;i<bodys.Length;i++)
 	{
  	 bodys[i].isKinematic = false;
 	}
 	*/