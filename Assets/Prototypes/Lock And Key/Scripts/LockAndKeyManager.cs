using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAndKeyManager : MonoBehaviour
{
    //keep a list of all of the objects that can be picked up
    public List<InteractionKey> Keys = new List<InteractionKey>();
    public InteractionKey HeldObject;
    public float PickupRadius = 4;
    public Vector3 PickupOffset;
    public Color GizmoColour;
    public List<Collider2D> HandColliders;

    private void Update()
    {
        //need to account for cases where pickupables are next to each other.
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (InteractionKey key in Keys)
            {
                Debug.Log("Key:" + key.name);

                if (Vector3.Distance(transform.position, key.GetClosestPointOnCollider(transform.position)) < PickupRadius)
                {
                    AddHeldObject(key);
                    Debug.Log("Picked Up: " + key.name);
                    return;
                }
            }
            if (HeldObject != null) DropHeldObject(HeldObject);
        }
    }
    public void AddHeldObject(InteractionKey _obj)
    {
        //when we pick something up we disable all the colliders on the hand.
        //if we are holding something we will drop what we are holding.
        //we remove the object we picked up from the list of objects we are able to pick up.
        //stick it in the middle of the hand.
        //and turn on the objects rigidbody (maybe don't need to do this?)
        Collider2D objCol = _obj.GetComponent<Collider2D>();
        foreach (Collider2D collider in HandColliders)
        {
            Physics2D.IgnoreCollision(objCol, collider, true);
        }
        _obj.Held = true;
        if (HeldObject != null) DropHeldObject(HeldObject);
        Keys.Remove(_obj);
        HeldObject = _obj;
        _obj.transform.parent = transform;
        _obj.transform.localPosition = Vector3.zero + PickupOffset;
        Rigidbody2D rb = HeldObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
    }

    public void DropHeldObject(InteractionKey _obj)
    {
        Collider2D objCol = _obj.GetComponent<Collider2D>();
        foreach (Collider2D collider in HandColliders)
        {
            Physics2D.IgnoreCollision(objCol, collider, false);
        }
        Rigidbody2D rb = HeldObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        _obj.Held = false;
        Keys.Add(_obj);
        HeldObject = null;
        Debug.Log("Dropped: " + _obj.name);
        _obj.transform.parent = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColour;
        Gizmos.DrawWireSphere(this.transform.position, PickupRadius);
        //foreach (InteractionKey key in Keys)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(key.GetClosestPointOnCollider(transform.position), .3f);
        //}
    }



}
