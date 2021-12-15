using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAPutControllerOnRailParent : CustomActionScript
{
	[SerializeField]
	private Transform railParent;

	[SerializeField]
	private bool constraintRotation;

	[SerializeField]
	private float time;

	private RigidBodyData rbData;

	private class RigidBodyData
    {
		public float mass;
		public RigidbodyConstraints contraint;
		public RigidbodyInterpolation interpolationMode;
		public CollisionDetectionMode collisionDetectionMode;

		public RigidBodyData(Rigidbody rigidbody)
        {
			this.mass = rigidbody.mass;
			this.contraint = rigidbody.constraints;
			this.interpolationMode = rigidbody.interpolation;
			this.collisionDetectionMode = rigidbody.collisionDetectionMode;
        }
    }

	public override IEnumerator DoActionOnEvent(MonoBehaviour sender, GameObject args)
	{
		float timeAtStart = Time.time;
		ContrainteController cc = GameObject.FindObjectOfType<ContrainteController>();
		if(null == cc || null == railParent || !railParent.gameObject.activeInHierarchy)
        {
			yield break;
        }

		GameObject controller = cc.gameObject;
		controller.transform.parent = railParent;
		controller.transform.localPosition = Vector3.zero;
		controller.GetComponent<Collider>().isTrigger = true;

		cc.canMove = false;

		if (constraintRotation)
		{
			cc.canRotate = false;
			controller.transform.localRotation = Quaternion.identity;
		}

		Rigidbody rb = controller.GetComponent<Rigidbody>();
		if(null != rb)
        {
			this.rbData = new RigidBodyData(rb);
			Destroy(rb);
			//rb.useGravity = false;
			//rb.constraints = RigidbodyConstraints.FreezeAll;
		}

		while (Time.time - timeAtStart < time)
        {
			controller.transform.localPosition = Vector3.zero;
			yield return new WaitForSeconds(Time.fixedDeltaTime);
		}


		if (null != rbData)
		{
			Rigidbody newRb = controller.AddComponent<Rigidbody>();
			newRb.mass = rbData.mass;
			newRb.constraints = rbData.contraint;
			newRb.interpolation = rbData.interpolationMode;
			newRb.collisionDetectionMode = rbData.collisionDetectionMode;

			Rigidbody rbParent = controller.transform.parent.gameObject.GetComponentInParent<Rigidbody>();

			if (null != rbParent)
            {
				newRb.velocity = rbParent.velocity;
			}
		}

		Transform head = controller.transform.Find("HeadController");
		Vector3 headLookVector = new Vector3(head.forward.x, head.forward.y, head.forward.z);

		controller.transform.parent = null;
		controller.transform.up = Vector3.up;
		controller.transform.forward = Vector3.ProjectOnPlane(headLookVector, controller.transform.up).normalized;

		head.transform.localRotation = Quaternion.identity;
		head.transform.forward = Vector3.ProjectOnPlane(headLookVector, head.transform.right).normalized;


		controller.GetComponent<Collider>().isTrigger = false;

		cc.canMove = true;
		cc.canRotate = true;

		yield return null;
	}
}
