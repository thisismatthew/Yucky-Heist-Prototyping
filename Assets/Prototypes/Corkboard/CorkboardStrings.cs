using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class CorkboardStrings : MonoBehaviour
{
    public bool HideSpringsInInspector;
    public int NumberOfParticles = 10;
    public float SpringCoefficient = 0.08f;
    public float SpringDesiredSpacing = .1f;
    public float StringWidth = 1.25f;
    public Color StringColor;
    public float Gravity = 0.1f;
    public GameObject Pin_1;
    public GameObject Pin_2;

    public bool GenerateArmParticlesOnStart = true;

    private Vector2 gravity;
    private List<ArmParticle> particles = new List<ArmParticle>();
    private List<ArmSpring> springs = new List<ArmSpring>();
    private ArmParticle wristParticle;

    public ArmParticle WristParticle { get => wristParticle; }


    void Start()
    {
        gravity = new Vector2(0, -Gravity);

        for (int i = 0; i < NumberOfParticles; i++)
        {
            if (GenerateArmParticlesOnStart == false) break;
            //spawn a particle
            ArmParticle p = new GameObject().AddComponent<ArmParticle>();
            p.gameObject.name = "particle-" + i.ToString();
            //slight offset so they don't freak out the springs initially
            p.transform.position += new Vector3(this.transform.position.x, this.transform.position.y);
            p.transform.parent = transform;
            //p.gameObject.AddComponent<CircleCollider2D>();
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
                s.Line.SortingOrder = 2;
                
                springs.Add(s);
                //s.lineColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            }
        }
        //particles[0].Locked = true;
        //particles[0].gameObject.transform.parent = Pin_2.transform;
        wristParticle = particles[particles.Count - 1];
        wristParticle.name += " - WRIST";
        wristParticle.Locked = true;
        particles[0].Locked = true;

    }

    // Update is called once per frame
    void Update()
    {

        wristParticle.transform.position = Pin_1.transform.position;
        particles[0].transform.position = Pin_2.transform.position;

        foreach (ArmSpring s in springs)
        {
            s.UpdateSpring();
            s.UpdateSpringVisuals();
            s.Line.Color = StringColor;
            s.Line.Thickness = StringWidth;
        }

        foreach (ArmParticle p in particles)
        {
            p.ApplyForce(gravity);
            p.UpdateParticle();
        }
    }

 
}
