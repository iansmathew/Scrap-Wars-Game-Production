using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineImpactDebugScript : MonoBehaviour {
    public bool showImpact = true;
    private Color impactColor;
    private void Start()
    {
        impactColor = GetComponent<Renderer>().material.color;
       // Debug.Log(impactColor);
    }
    private void OnDrawGizmos()
    {
        if (showImpact)
        {
            impactColor.a = 0.3f;
            Gizmos.color = impactColor;
            Gizmos.DrawSphere(transform.position, transform.localScale.x * 2);
        }
    }
}
