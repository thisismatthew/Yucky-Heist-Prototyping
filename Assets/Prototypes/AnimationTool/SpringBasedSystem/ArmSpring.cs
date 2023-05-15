using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSpring : MonoBehaviour
{
    public float K; //the spring coefficient
    public float RestLength;
    public ArmParticle A, B;
    public Line Line;
    public Color lineColor;
    private void Start()
    {
        //Line = gameObject.AddComponent<Line>();
        Line.Color = lineColor;
    }
    public void UpdateSpring()
    {
        var force = B.transform.localPosition - A.transform.localPosition;
        float x = force.magnitude - RestLength;
        force *= x * K; 
        //Debug.Log(gameObject.name + "-" + force.magnitude);
        A.ApplyForce(force);
        B.ApplyForce(-force);
    }

    public void UpdateSpringVisuals()
    {
        Line.Start = A.transform.localPosition;
        Line.End = B.transform.localPosition;
    }

}
