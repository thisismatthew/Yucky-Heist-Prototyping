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

    private void Update()
    {
        //need to account for cases where pickupables are next to each other.
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (InteractionKey key in Keys)
            {
                if (Vector3.Distance(transform.position+PickupOffset, key.transform.position) < PickupRadius)
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
        _obj.Held = true;
        if (HeldObject != null) DropHeldObject(HeldObject);
        Keys.Remove(_obj);
        HeldObject = _obj;
        _obj.transform.parent = transform;
        _obj.transform.localPosition = Vector3.zero + PickupOffset;
    }

    public void DropHeldObject(InteractionKey _obj)
    {
        _obj.Held = false;
        Keys.Add(_obj);
        HeldObject = null;
        Debug.Log("Dropped: " + _obj.name);
        _obj.transform.parent = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColour;
        Gizmos.DrawWireSphere(this.transform.position + PickupOffset, PickupRadius);
    }
}
