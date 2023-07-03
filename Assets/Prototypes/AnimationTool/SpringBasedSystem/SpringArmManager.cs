using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;


public class SpringArmManager : MonoBehaviour
{
    public bool Paused = false;
    public bool PinWristSegment = false;
    public float StretchSpeed = 0.3f;
    public int NumberOfParticles = 10;
    public int GrowthNumberOfParticles = 3;
    public float SpringCoefficient = 0.08f;
    public float SpringDesiredSpacing = .1f;
    public float ArmWidth = 1.25f;
    public Color ArmColor;
    public float Gravity = 0.1f;
    public GameObject Hand;
    public Transform StretchTargetForCollision;
    

    private readonly float finalStretchTime = 0.2f;
    private float finalStretchTimer = 0;
    private Vector2 gravity;
    private readonly List<ArmParticle> particles = new();
    private readonly List<ArmSpring> springs = new();
    private ArmParticle wristParticle; 
    [SerializeField] private ArmTrail trailManager;
    private Vector3 handPointDirection;
    public bool UseGravity = false;

    public ArmParticle WristParticle { get => wristParticle;}


    void Start()
    {
        gravity = new Vector2(0, -Gravity);
        for (int i = 0; i< NumberOfParticles; i++)
        {
            //spawn a particle
            ArmParticle p = new GameObject().AddComponent<ArmParticle>();
            p.gameObject.name = $"particle-{i}";
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
                s.gameObject.name = $"spring-{i}";
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
        trailManager.handLink = springs[0];//just hooking up the line with the spring.
        trailManager.handLink.Line.End = Vector2.zero; //lines get spawned with a default (1,0) for thier end point, which causes issues with an always drawing ArmTrail.
        springs[0].gameObject.name = "spring - TRAIL LINK";
        wristParticle = particles[particles.Count - 1];

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Paused) return;
        Hand.transform.position = wristParticle.transform.position;

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    for(int i=0; i<GrowthNumberOfParticles; i++)
        //    {
        //        Invoke("AddParticle", i * 0.2f);
        //    }
        //}

        particles[0].Locked = PinWristSegment;

        foreach (ArmSpring s in springs)
        {
            s.UpdateSpring();
            s.UpdateSpringVisuals();
            s.Line.Color = ArmColor;
            s.Line.Thickness = ArmWidth;
            
        }

        foreach (ArmParticle p in particles)
        {
            if (UseGravity) p.ApplyForce(gravity);
            p.UpdateParticle();
        }

        //stretching & retracting
        #region
        //This is for the stretching of the particles at the end of the arm
        if (Input.GetMouseButton(0))
        {
            finalStretchTimer = finalStretchTime;
            SetHandPointDirection((springs[springs.Count - 1].Line.Start - springs[springs.Count - 1].Line.End).normalized);
            Vector3 dir = StretchTargetForCollision.position - wristParticle.transform.position;
            dir.Normalize();
            Vector3 newPos = wristParticle.transform.position + dir * StretchSpeed;
            wristParticle.transform.position = newPos;
            wristParticle.Velocity = Vector2.zero;
        }

        // Signiling a retraction
        if (Input.GetMouseButton(1) || (!Input.GetMouseButton(0) && trailManager.AutoRetract)) 
        {   

            finalStretchTimer = finalStretchTime;
            SetHandPointDirection((springs[springs.Count - 1].Line.Start - springs[springs.Count - 1].Line.End).normalized);
        }
        //Debug.Log("tick");


        // actually retracting, the signal is there so the retraction goes
        // a little longer after the mouse has let go, like coyote time but for retraction
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !trailManager.AutoRetract)
        {
            if (finalStretchTimer < 0) return;
            finalStretchTimer -= Time.deltaTime;
            SetHandPointDirection((springs[springs.Count - 1].Line.Start - springs[springs.Count - 1].Line.End).normalized);
            //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //lets get the direction
            //mousePos.z = 0;
            Vector3 dir = StretchTargetForCollision.position - wristParticle.transform.position;
            dir.Normalize();
            Vector3 newPos = wristParticle.transform.position + dir * 0.1f;
            wristParticle.transform.position = newPos;
            wristParticle.Velocity = Vector2.zero;
        }
        #endregion
    }

    public void SetHandPointDirection(Vector3 handPointDirection)
    {
        
        Helpers.RotateToFace(Hand, Hand.transform.position + handPointDirection, 90);
    }

    /*public void AddParticle()
    {
        
        ArmParticle p = new GameObject().AddComponent<ArmParticle>();
        p.gameObject.name = "particle-" + (1 + particles.Count).ToString();
        //slight offset so they don't freak out the springs initially
        p.transform.position += new Vector3(this.transform.position.x, this.transform.position.y);
        p.transform.parent = transform;
        particles.Insert(particles.Count - 2, p);
        //new spring
        ArmSpring finalSpring = new GameObject().AddComponent<ArmSpring>();
        finalSpring.transform.parent = transform;
        finalSpring.transform.position = this.transform.position;
        finalSpring.gameObject.name = "spring-" + (1 + springs.Count).ToString();
        finalSpring.K = SpringCoefficient;
        finalSpring.RestLength = SpringDesiredSpacing;
        finalSpring.Line = finalSpring.gameObject.AddComponent<Line>();

        //connect this particle to be just before the last one
        finalSpring.B = p;
        finalSpring.A = springs[springs.Count - 2].A;
        springs[springs.Count - 2].A = p;
        springs.Insert(springs.Count -2, finalSpring);
    }*/
}
