using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidImpactDebugScript : MonoBehaviour {
    public bool showImpact = true;

    private RaycastHit hit;
    private Color impactColor;

    private void Start()
    {
        impactColor = GetComponent<Renderer>().material.color;
    }

    private void FixedUpdate()
    {
        Physics.Raycast(transform.position, transform.forward, out hit);
    }

    private void OnDrawGizmos()
    {
        if (showImpact)
        {
            Debug.DrawRay(transform.position, transform.forward * 50);
            Gizmos.color = impactColor;
            Gizmos.DrawSphere(hit.point, transform.localScale.x);
        }
    }
}
