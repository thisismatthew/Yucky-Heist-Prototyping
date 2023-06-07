using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionKey : MonoBehaviour
{
    public bool Held = false;

    public Vector3 GetClosestPointOnCollider(Vector3 point)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return transform.position;

        return col.ClosestPoint(point);
    }
}

