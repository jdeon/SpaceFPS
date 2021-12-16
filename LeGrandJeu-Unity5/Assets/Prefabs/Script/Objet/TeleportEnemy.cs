using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemy : MonoBehaviour
{

    public float distanceMax;
    public float angleCone;

    //Cette ennemie n'affecte que le controller
    private List<GestionCheckpoint> controllerInZone;
    private SphereCollider collider;

    private void Start()
    {
        controllerInZone = new List<GestionCheckpoint>();
        collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = distanceMax;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        GestionCheckpoint gestionCheckpoint = other.gameObject.GetComponent<GestionCheckpoint>();
        if (null != gestionCheckpoint)
        {
            controllerInZone.Add(gestionCheckpoint);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GestionCheckpoint gestionCheckpoint = other.gameObject.GetComponent<GestionCheckpoint>();
        if (null != gestionCheckpoint)
        {
            controllerInZone.Add(gestionCheckpoint);
        }
    }

    // Use this for initialization
    private void Update()
    {
        if (controllerInZone.Count > 0)
        {
            foreach (GestionCheckpoint target in controllerInZone)
            {
                RaycastHit hit = new RaycastHit();
                float angle = Vector3.Angle(-target.transform.up, target.transform.position - transform.position);
                if (Mathf.Abs(angle) < angleCone && Physics.Linecast(transform.position, target.transform.position, out hit)
                    && null != hit.collider && target.gameObject.Equals(hit.collider.gameObject))
                {
                    target.respawnCheckPoint();       
                }
            }
        }
    }
}
