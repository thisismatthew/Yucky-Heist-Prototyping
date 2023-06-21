using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    public Collider2D IgnoreCollisionsWith;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(IgnoreCollisionsWith, GetComponent<Collider2D>());
    }


}
