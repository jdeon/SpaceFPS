using UnityEngine;
using System.Collections;

public class RotationParametre : MonoBehaviour {

	public Transform to;
	public float speed = 0.1F;

	private Transform from;

	void Start ()
	{
		from = transform;
	}

	void Update() 
	{
		transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.deltaTime * speed);
	}
}