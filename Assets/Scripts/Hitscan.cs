using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitscan : Interactable
{
    public float pullRadius = 2;
    public float pullForce = 1;

    void Update()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius))
            {
            if (collider.CompareTag(targetTag))
            {
                // calculate direction from target to me
                Vector3 forceDirection = transform.position - collider.transform.position;

                // apply force on target towards me

                Rigidbody rBody = collider.GetComponent<Rigidbody>();
                rBody.AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(148, 0, 211);
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }

}
