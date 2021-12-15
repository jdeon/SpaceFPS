using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClickScript : MonoBehaviour {

	public string _nextScene = "";

    InputAction action = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>");

    public void Update()
	{
		if (action.triggered)
		{
            SceneManager.LoadScene(_nextScene);
		}
	}
}
