using UnityEngine;
using System.Collections;

public class CADebugLogMessage : CustomActionScript {

	public string _message;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		Debug.Log(_message);
		yield return null;
	}
}
