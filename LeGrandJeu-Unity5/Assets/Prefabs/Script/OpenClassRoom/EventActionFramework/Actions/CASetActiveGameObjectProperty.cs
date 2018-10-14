using UnityEngine;
using System.Collections;

public class CASetActiveGameObjectProperty : CustomActionScript {

	public bool _setActive = true;

	public GameObject _gameObjectToActivate;

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		if (_gameObjectToActivate != null)
			Debug.DrawLine(this.transform.position, _gameObjectToActivate.transform.position, Color.blue);

	}

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
		yield return new WaitForFixedUpdate();
		if (_gameObjectToActivate != null)
			_gameObjectToActivate.SetActive(_setActive); 
		yield return null;
	}

}
