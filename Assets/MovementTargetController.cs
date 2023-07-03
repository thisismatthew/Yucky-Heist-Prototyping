using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTargetController : MonoBehaviour
{
    //this script is going to be attatched to the movement controller 
    // and is going to make sure that it is always offset to the Hand object
    
    //could we do two things, apply a force in the direction of the mouse, but contstrain the mouse to a distance

    public float MaxDistance;
    public GameObject Hand;
    
    
    // Update is called once per frame
    void Update()
    {

        var constrainedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        constrainedPosition.z = 0;
        if (Vector2.Distance(constrainedPosition, Hand.transform.position) > MaxDistance)
        {
            var dir = constrainedPosition - Hand.transform.position;
            dir.Normalize();
            constrainedPosition = Hand.transform.position + (dir * MaxDistance);
                
        }
        constrainedPosition.z = 0;

        this.transform.position = constrainedPosition;
    }

}
