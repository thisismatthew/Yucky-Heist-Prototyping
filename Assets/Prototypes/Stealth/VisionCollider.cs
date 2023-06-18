using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCollider : MonoBehaviour
{
    private Gaurd AI;
    private void Start()
    {
        AI = GetComponentInParent<Gaurd>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hand")
        {
            AI.EnterState(GaurdState.chase);
        }
    }
}
