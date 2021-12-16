using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEOnEnterConeCast : CustomEventScript
{

    public float distanceMax;
    public float angleCone;

    private List<GameObject> goInZone;
    private SphereCollider collider;

    private void Start()
    {
        goInZone = new List<GameObject>();
        collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = distanceMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        goInZone.Add(collider.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        goInZone.Remove(collider.gameObject);
    }

    // Use this for initialization
    private void Update()
    {
        if (goInZone.Count > 0)
        {
            foreach (GameObject target in goInZone)
            {
                RaycastHit hit = new RaycastHit();
                float angle = Vector3.Angle(-target.transform.up, target.transform.position - transform.position);
                if (Mathf.Abs(angle) < angleCone && Physics.Linecast(transform.position, target.transform.position, out hit)
                    && null != hit.collider && target.Equals(hit.collider.gameObject))
                {
                    OnTriggered(this, target);
                }
            }
        }
    }
}
