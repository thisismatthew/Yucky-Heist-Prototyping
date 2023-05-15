using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class SpringOnlyArmManager : MonoBehaviour
{
    public float handStopsFollowingDistance = 1f;
    public float StretchSpeed = 0.3f;
    public int NumberOfParticles = 10;
    public float SpringCoefficient = 0.08f;
    public float SpringDesiredSpacing = .1f;
    public float ArmWidth = 1.25f;
    public Color ArmColor;
    public float Gravity = 0.1f;
    public GameObject Hand;

    private Vector2 gravity;
    private List<ArmParticle> particles = new List<ArmParticle>();
    private List<ArmSpring> springs = new List<ArmSpring>();
    private ArmParticle wristParticle, middleParticle;
    private Vector3 handPointDirection;

    public ArmParticle WristParticle { get => wristParticle; }
    public ArmParticle MiddleParticle { get => middleParticle; }

    void Start()
    {
        gravity = new Vector2(0, -Gravity);
        for (int i = 0; i < NumberOfParticles; i++)
        {
            //spawn a particle
            ArmParticle p = new GameObject().AddComponent<ArmParticle>();
            p.gameObject.name = "particle-" + i.ToString();
            //slight offset so they don't freak out the springs initially
            p.transform.position += new Vector3(this.transform.position.x, this.transform.position.y);
            p.transform.parent = transform;
            particles.Add(p);
            //p.gameObject.hideFlags = HideFlags.HideInHierarchy;

            if (i != 0) // if we have at least 2 particles lets make a spring joining them
            {
                var a = particles[i];
                var b = particles[i - 1];
                ArmSpring s = new GameObject().AddComponent<ArmSpring>();
                s.transform.parent = transform;
                s.transform.position = this.transform.position;
                s.gameObject.name = "spring-" + i.ToString();
                s.A = a;
                s.B = b;
                s.K = SpringCoefficient;
                s.RestLength = SpringDesiredSpacing;
                s.Line = s.gameObject.AddComponent<Line>();
                springs.Add(s);
                //s.lineColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            }
        }
        //particles[0].Locked = true;
        springs[0].gameObject.name = "spring - HANDLINK";
        int midIndex = (int)Mathf.Floor((particles.Count + 1) / 2);
        middleParticle = particles[midIndex];
        wristParticle = particles[particles.Count - 1];
        wristParticle.name += " - WRIST";
        wristParticle.Locked = true;
        particles[0].Locked = true;


    }

    // Update is called once per frame
    void Update()
    {
        Hand.transform.position = wristParticle.transform.position;


        foreach (ArmSpring s in springs)
        {
            s.UpdateSpring();
            s.UpdateSpringVisuals();
            s.Line.Color = ArmColor;
            s.Line.Thickness = ArmWidth;
        }

        foreach (ArmParticle p in particles)
        {
            p.ApplyForce(gravity);
            p.UpdateParticle();
        }

        if (true)
        {
            SetHandPointDirection((springs[springs.Count - 1].Line.Start - springs[springs.Count - 1].Line.End).normalized);
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //lets get the direction
            mousePos.z = 0;
            Vector3 dir = mousePos - wristParticle.transform.position;
            dir.Normalize();
            Vector3 newPos = wristParticle.transform.position + dir * StretchSpeed;
            //lets only move the hand if its a certain distance from the mouse
            if (Vector3.Distance(wristParticle.transform.position, mousePos) > handStopsFollowingDistance)
            {
                
                wristParticle.transform.position = Vector3.Lerp(newPos, wristParticle.transform.position, .5f);
            }
            wristParticle.Velocity = Vector2.zero;
        }

    }

    public void SetHandPointDirection(Vector3 handPointDirection)
    {

        Helpers.RotateToFace(Hand, Hand.transform.position + handPointDirection, 90);
    }
}
