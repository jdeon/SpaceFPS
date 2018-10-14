using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CANextLevel : CustomActionScript {

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
	}
}
