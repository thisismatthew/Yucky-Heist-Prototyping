using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaurdAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private PathingManager pm;


    [Header("Inputs")]
    public GameObject Target;
    public float MaximumSpeed = 1f;

    [Header("Seek Behaviour")]
    private int currentMovePointIndex = 0;
    private List<Vector2> currentPath = new List<Vector2>();
    public float PathAccuracyRadius = 1f;
    public float StopChaseRadius = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        pm = FindAnyObjectByType<PathingManager>();
        currentPath = pm.AStarPath(transform.position, Target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath.Count != 0)
        {
            for (int i = currentMovePointIndex; i < currentPath.Count - 2; i++)
            {
                Debug.DrawLine(currentPath[i], currentPath[i + 1], Color.green);
            }
            if (Vector2.Distance(transform.position, currentPath[currentMovePointIndex]) < PathAccuracyRadius) currentMovePointIndex++;

            Seek(currentPath[currentMovePointIndex]);
        }

        UpdateSprite();
    }

    private void Seek(Vector2 position)
    {
        Vector2 desiredVel = position - (Vector2)transform.position;
        desiredVel = desiredVel.normalized * MaximumSpeed;
        Vector2 steeringVel = desiredVel - rb.velocity;
        //rb.velocity = Vector2.zero;
        rb.AddForce(steeringVel);
    }

    private void OnDrawGizmos()
    {
        if (currentPath.Count != 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentPath[currentMovePointIndex], .5f);
            Gizmos.DrawWireSphere(transform.position, PathAccuracyRadius);
        }
    }
    private void UpdateSprite()
    {
        if (rb.velocity.x < 0) spr.flipX = true;
        else spr.flipX = false;
    }
}
