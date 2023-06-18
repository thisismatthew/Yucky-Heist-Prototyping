using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Gaurd : MonoBehaviour
{
    private SpriteRenderer spr;
    private AIPath aiPath;
    private AIDestinationSetter aiSetter;
    public GaurdState State = GaurdState.patrol;

    [Header("Patrol Behaviour")]
    public float PatrolSpeed = 5;
    public List<Transform> PatrolPoints;
    private int patrolIndex = 0;
    private bool patrolPointReached = true;

    [Header("Chase Behaviour")]
    public float ChaseSpeed = 15;
    public GameObject ChaseTarget;
    public GameObject VisionColliderRight, VisionColliderLeft;
    public float RePathRadius = 10f;
    public float ChaseTime = 10f;
    private float chaseTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        aiPath = GetComponent<AIPath>();
        aiSetter = GetComponent<AIDestinationSetter>();
        EnterState(GaurdState.patrol);
        VisionColliderLeft.SetActive(false);
        EnterState(State);
    }



    public void EnterState(GaurdState state)
    {
        State = state;
        switch (state)
        {
            case GaurdState.patrol:
                aiPath.maxSpeed = PatrolSpeed;
                patrolIndex = 0;
                aiSetter.target = PatrolPoints[patrolIndex];
                break;
            case GaurdState.chase:
                aiPath.maxSpeed = ChaseSpeed;
                aiSetter.target = ChaseTarget.transform;
                break;
            case GaurdState.followArm:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprite();
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
        
    }
    private void Patrol()
    {
        if (aiPath.reachedEndOfPath)
        {
            patrolIndex++;
            if (patrolIndex > PatrolPoints.Count - 1) patrolIndex = 0;
            aiSetter.target = PatrolPoints[patrolIndex];
        }
    }

    private void Chase()
    {
        if (chaseTimer < ChaseTime)
        {
            if (Vector2.Distance(ChaseTarget.transform.position, transform.position) > RePathRadius)
            {
                chaseTimer += Time.deltaTime;
            }
            else
            {
                chaseTimer = 0;
            }
        }
        else
        {
            chaseTimer = 0;
            EnterState(GaurdState.patrol);
        }
    }


    private void UpdateSprite()
    {

        if (aiPath.desiredVelocity.x<0)
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
