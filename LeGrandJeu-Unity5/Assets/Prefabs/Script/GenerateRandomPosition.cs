using UnityEngine;
using System.Collections;

public class GenerateRandomPosition : MonoBehaviour {

	public float borneMinX = 0;
	public float borneMaxX = 0;
	public float borneMinY = 0;
	public float borneMaxY = 0;
	public float borneMinZ = 0;
	public float borneMaxZ = 0;

	public Vector3 genrateRandomVector(){
		Vector3 _position = new Vector3(Random.Range(borneMinX, borneMaxX),Random.Range(borneMinY, borneMaxY),Random.Range(borneMinZ, borneMaxZ));
		return _position;
	}

}
