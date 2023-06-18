using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GaurdState
{
    patrol,
    chase,
    followArm,
}

public class GaurdAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private PathingManager pm;

    public GaurdState State = GaurdState.patrol;

    [Header("Inputs")]
    
    public float MaximumSpeed = 1f;

    [Header("Seek Behaviour")]
    private int currentMovePointIndex = 0;
    private List<Vector2> currentPath = new List<Vector2>();
    public float PathAccuracyRadius = 1f;
    public float StopChaseRadius = 0.5f;
    private bool pathComplete = false;

    [Header("Patrol Behaviour")]
    public List<Transform> PatrolPoints;
    private int patrolIndex = 0;
    private bool patrolPointReached = true;

    [Header("Chase Behaviour")]
    public GameObject Target;
    public GameObject VisionColliderRight, VisionColliderLeft;
    public float RePathRadius = 10f;
    public float ChaseTime = 10f;
    private float chaseTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        pm = FindAnyObjectByType<PathingManager>();
        EnterState(GaurdState.patrol);
        VisionColliderLeft.SetActive(false);
    }

    

    public void EnterState(GaurdState state)
    {
        State = state;
        pathComplete = false;
        currentMovePointIndex = 0;
        switch (state)
        {
            case GaurdState.patrol:
                currentPath = pm.AStarPath(transform.position, PatrolPoints[patrolIndex].position);
                break;
            case GaurdState.chase:
                currentPath = pm.AStarPath(transform.position, Target.transform.position);
                break;
            case GaurdState.followArm:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GaurdState.patrol:
                Patrol();
                break;
            case GaurdState.chase:
                Chase();
                break;
            case GaurdState.followArm:
                //FollowArm();
                break;
        }
        UpdateSprite();
    }
    private void Patrol()
    {

        //if this is the first time we are patrolling set the path to the first point;
        if (pathComplete)
        {
            pathComplete = false;
            patrolIndex++;
            if (patrolIndex > PatrolPoints.Count - 1) patrolIndex = 0;
            currentPath = pm.AStarPath(transform.position, PatrolPoints[patrolIndex].position);
        }

        //go to the path
        FollowPath();
    }

    private void Chase()
    {
        if (chaseTimer < ChaseTime)
        {
            if (Vector2.Distance(Target.transform.position, transform.position) > RePathRadius)
            {
                chaseTimer += Time.deltaTime;
                if (pathComplete)
                {
                    currentPath = pm.AStarPath(transform.position, Target.transform.position);
                }
                else
                {
                    FollowPath();
                }
            }
            else
            {
                chaseTimer = 0;
                Seek(Target.transform.position);
            }
        }
        else
        {
            chaseTimer = 0;
            EnterState(GaurdState.patrol);
        }
    }

    private void FollowPath()
    {
        if (currentPath.Count != 0)
        {
            for (int i = currentMovePointIndex; i < currentPath.Count - 2; i++)
            {
                Debug.DrawLine(currentPath[i], currentPath[i + 1], Color.green);
            }

            if (Vector2.Distance(transform.position, currentPath[currentMovePointIndex]) < PathAccuracyRadius) 
                currentMovePointIndex++;

            if (currentMovePointIndex > currentPath.Count - 1)
            {
                currentMovePointIndex = 0;
                pathComplete = true;
                return;
            }

            Seek(currentPath[currentMovePointIndex]);
        }
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, RePathRadius);
    }
    private void UpdateSprite()
    {
        if (rb.velocity.x < 0)
        {
            VisionColliderLeft.SetActive(true);
            VisionColliderRight.SetActive(false);
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
            VisionColliderLeft.SetActive(false);
            VisionColliderRight.SetActive(true);
        }
    }
}
