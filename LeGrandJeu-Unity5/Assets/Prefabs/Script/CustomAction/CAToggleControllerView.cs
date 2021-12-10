using System.Collections;
using UnityEngine;

public class CAToggleControllerView : CustomActionScript
{
	[SerializeField]
	private bool enableView;

	public override IEnumerator DoActionOnEvent(MonoBehaviour sender, GameObject args)
	{
		ContrainteController cc = GameObject.FindObjectOfType<ContrainteController>();
		if (null == cc)
		{
			yield break;
		}

		GameObject controller = cc.gameObject;
		Transform head = controller.transform.Find("HeadController");
		
		cc.canMove = enableView;
		cc.canRotate = enableView;
		head.gameObject.SetActive(enableView);

		Rigidbody rb = controller.GetComponent<Rigidbody>();
		if (null != rb)
		{
			rb.isKinematic = enableView;
		}

		yield return null;
	}
}
