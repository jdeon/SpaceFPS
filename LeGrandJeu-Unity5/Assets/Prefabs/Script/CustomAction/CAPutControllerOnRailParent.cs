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
		if(null == cc || !railParent.gameObject.activeInHierarchy)
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

		controller.transform.parent = null;
		//TODO mieux faire ?
		controller.transform.up = Vector3.up;
		controller.GetComponent<Collider>().isTrigger = false;

		cc.canMove = true;
		cc.canRotate = true;

		yield return null;
	}
}
