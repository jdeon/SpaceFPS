using UnityEngine;
using System.Collections;

public abstract class CustomEventScript : MonoBehaviour {

	public delegate void CustomEventDelegate(MonoBehaviour sender, GameObject args);

	[SerializeField]
	public CustomEventDelegate _triggered;

	public void OnTriggered(MonoBehaviour sender, GameObject args)
	{
		if (_triggered != null)
			_triggered(sender, args);
	}

}
