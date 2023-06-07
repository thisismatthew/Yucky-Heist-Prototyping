using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCollisionManager : MonoBehaviour
{
    //currently this is just handling collision for the hand. 

    
    private SpringOnlyArmManager sam;
    private void Start()
    {
        sam = GetComponentInParent<SpringOnlyArmManager>();
    }
    //this doesn't work, the hand just bonks into the collider and stops forever. No forces are pushing back
    //We could just apply a very mild bounce back force to both objects?
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        sam.FollowMouse = false;
        sam.WristParticle.Locked = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        sam.FollowMouse = true;
        sam.WristParticle.Locked = true;
    }*/
}
