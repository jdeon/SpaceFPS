using UnityEngine;
using System.Collections;

public class ViseeDeCanonV2 : MonoBehaviour {

	public float turnSpeed = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.left * turnSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
		
		transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
	}
}
