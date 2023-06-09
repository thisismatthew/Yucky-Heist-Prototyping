using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyColliderChecker : MonoBehaviour
{
    public Collider2D KeyCollider;
    public GameObject ActivateObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == KeyCollider) ActivateObject.SetActive(true);
    }
}
