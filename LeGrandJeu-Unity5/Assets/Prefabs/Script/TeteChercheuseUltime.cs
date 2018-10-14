using UnityEngine;
using System.Collections;

public class TeteChercheuseUltime : MonoBehaviour {

	public Transform Target;
	public float Speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt (Target);
		transform.Translate (0, 0, Speed * Time.deltaTime);
	}
}
