using UnityEngine;
using System.Collections;

public class UnCheckIsKinematicOnTriggerEnter : MonoBehaviour {

	public float delay = 0;

	[SerializeField]
	private Rigidbody _targetRigidbody;

	public void OnDrawGizmos()
	{
		if (_targetRigidbody != null)
			Debug.DrawLine(this.transform.position, _targetRigidbody.transform.position, Color.yellow);
	}

	public IEnumerator  OnTriggerEnter(Collider col)
	{
		yield return new WaitForSeconds (delay);

		if (_targetRigidbody != null)
		{
			_targetRigidbody.isKinematic = false;
			_targetRigidbody.WakeUp();
		}

		yield return null;
	}
}