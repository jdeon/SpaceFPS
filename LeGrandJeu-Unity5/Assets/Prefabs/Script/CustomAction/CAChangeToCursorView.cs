using System.Collections;
using UnityEngine;

public class CAChangeToCursorView : CustomActionScript
{
	[SerializeField]
	private Transform targetView;

	[SerializeField]
	private float time;

	[SerializeField]
	private EnigmeAbstract enigmeOptionnel;

	[SerializeField]
	private bool activatePointer = true;

	private Vector3 originPosition;
	private Quaternion originRotation;

	public override IEnumerator DoActionOnEvent(MonoBehaviour sender, GameObject args)
	{
		if(Vector3.zero.Equals(originPosition) && null != enigmeOptionnel && enigmeOptionnel.isEnigmeResolu())
        {
			yield break;
        }

        if (Vector3.zero.Equals(originPosition))
        {
			yield return enterView();
        } else
        {
			yield return exitView();
        }

		yield return null;
	}

	private IEnumerator enterView()
    {
		float timeAtStart = Time.time;

		ContrainteController cc = GameObject.FindObjectOfType<ContrainteController>();
		if (null == cc || !targetView.gameObject.activeInHierarchy)
		{
			yield break;
		}

		GameObject controller = cc.gameObject;
		controller.transform.parent = targetView;
		originPosition = new Vector3(controller.transform.localPosition.x, controller.transform.localPosition.y, controller.transform.localPosition.z);
		originRotation = new Quaternion(controller.transform.localRotation.x, controller.transform.localRotation.y, controller.transform.localRotation.z, controller.transform.localRotation.w);

		Transform head = controller.transform.Find("HeadController");
		Quaternion headOriginRotation = new Quaternion(head.transform.localRotation.x, head.transform.localRotation.y, head.transform.localRotation.z, head.transform.localRotation.w);
		
		cc.canMove = false;
		cc.canRotate = false;

		Collider collider = controller.GetComponent<Collider>();
		if(null != collider)
        {
			collider.enabled = false;
        }

		Rigidbody rb = controller.GetComponent<Rigidbody>();

		if(null != rb)
        {
			rb.isKinematic = true;
			rb.detectCollisions = false;
        }

		while (Time.time - timeAtStart < time)
		{
			float portion = (Time.time - timeAtStart) / time;
			//La Camera est dans la head
			controller.transform.localPosition = Vector3.Lerp(originPosition, -head.localPosition, portion);
			controller.transform.localRotation = Quaternion.Slerp(originRotation, Quaternion.identity, portion);
			head.transform.localRotation = Quaternion.Slerp(headOriginRotation, Quaternion.identity, portion);
			yield return null;
		}

		controller.transform.localPosition = -head.localPosition;
		controller.transform.localRotation = Quaternion.identity;
		head.transform.localRotation =  Quaternion.identity;

        if (activatePointer) { 
			CursorCustom.Activate = true;
		}

		yield return null;
	}

	private IEnumerator exitView()
	{
		float timeAtStart = Time.time;

		ContrainteController cc = GameObject.FindObjectOfType<ContrainteController>();
		if (null == cc || !targetView.gameObject.activeInHierarchy)
		{
			yield break;
		}

		GameObject controller = cc.gameObject;
		CursorCustom.Activate = false;
		Transform head = controller.transform.Find("HeadController");

		while (Time.time - timeAtStart < time)
		{
			float portion = (Time.time - timeAtStart) / time;
			controller.transform.localPosition = Vector3.Lerp(-head.localPosition, originPosition, portion);
			controller.transform.localRotation = Quaternion.Slerp(Quaternion.identity, originRotation, portion);
			yield return null;
		}

		controller.transform.localPosition = originPosition;
		controller.transform.localRotation = originRotation;

		cc.canMove = true;
		cc.canRotate = true;
		originPosition = Vector3.zero;
		controller.transform.parent = null;

		Collider collider = controller.GetComponent<Collider>();
		if (null != collider)
		{
			collider.enabled = true;
		}

		Rigidbody rb = controller.GetComponent<Rigidbody>();
		if (null != rb)
		{
			rb.isKinematic = false;
			rb.detectCollisions = true;
		}

		yield return null;
	}
}
