using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionLock : MonoBehaviour
{
    public InteractionKey KeyNeeded;
    public float DropOffRadius = 2.5f;
    public GameObject ActivateOnUnlock;
    
    // Update is called once per frame
    void Update()
    {
        if (KeyNeeded.Held) return;

        if (Vector3.Distance(transform.position, KeyNeeded.transform.position) < DropOffRadius)
        {
            ActivateOnUnlock.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.transform.position, DropOffRadius);
    }
}
