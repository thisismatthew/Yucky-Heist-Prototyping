using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingBlocker : MonoBehaviour
{

    private void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        // there is a 100% a more efficient algorythm for checking the bounds of the collider against the grid
        // but atm i'm just going to check every cell in the grid and see if its inside the collider
        PathingManager pm = FindObjectOfType<PathingManager>();

    }
}
