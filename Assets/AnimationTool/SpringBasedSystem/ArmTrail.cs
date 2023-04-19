using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// I was fixing the issue with the arm allignment when retracted all the way back to first position but it appears to be fixed!
// maybe i did it yesterday and I forgot, or maybe it's just because at low stretch/retract speeds it doesn't occur?

public class ArmTrail : MonoBehaviour
{
    private Line maskingLine;
    [SerializeField] public Line CurrentFrontSegment;
    [SerializeField] public float newSegmentDistance = 0.1f;
    [HideInInspector] public ArmSpring handLink;
    public float ArmWidth = 1.25f;
    public Color ArmColor;
    private List<Line> armSegments = new List<Line>();
    public float RetractSpeed = .5f;
    //public bool AtFirstPosition = true;
    //private bool finalRetractionPointReached = false;
    [Header("Wave Stuff")]
    [SerializeField] public float Amplitude = .2f;
    [SerializeField] public float Frequency = .3f;
    //this next field should probably be assigned directly to each line segment via the armSpring object when I refactor
    private List<Vector3> LineRelativeDirections = new List<Vector3>();

    private void Start()
    {
        maskingLine = new GameObject().AddComponent<Line>(); 
        maskingLine.transform.parent = transform;
        maskingLine.transform.position = transform.position;
        maskingLine.name = "MaskingLine";
        maskingLine.Color = ArmColor;
        maskingLine.Thickness = ArmWidth;

        armSegments.Add(CurrentFrontSegment);

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector2.Distance(CurrentFrontSegment.Start, handLink.Line.End));

        if (handLink == null) return;
        
        //if (armSegments.Count == 1) AtFirstPosition = true; else AtFirstPosition = false;
        if (Input.GetMouseButton(1)) Retract();
        else CreateTrail();
        UpdateMaskLine(armSegments.Count == 1);
        SimulateWave();


    }

    private void UpdateMaskLine(bool atFirstLink)
    {
        //the mask line connect the gap that can occur between the springy wrist and the trail end (current front segment end)
        maskingLine.Start = handLink.Line.End;
        if (atFirstLink) maskingLine.End = CurrentFrontSegment.Start;
        else maskingLine.End = CurrentFrontSegment.Start - transform.position;
    }
    private void CreateTrail()
    {
        
        // if the hand link end gets too far from the current segment's end point spawn a new segment.
        if (Vector2.Distance(CurrentFrontSegment.Start, handLink.Line.End + transform.position) > newSegmentDistance)
        {
            //Debug.Log(CurrentFrontSegment.Start);
            //Debug.Log(handLink.Line.End + transform.position);
            //why does it create an arm particle at distance 0?
            var newSegment = new GameObject().AddComponent<Line>();
            newSegment.name = "Trail Segment - " + (armSegments.Count - 1);
            newSegment.gameObject.transform.parent = transform;
            newSegment.End = CurrentFrontSegment.Start;
            if (armSegments.Count == 1) { newSegment.End += transform.position; }
            newSegment.Start = handLink.Line.End + transform.position;
            newSegment.Color = ArmColor;
            newSegment.Thickness = ArmWidth;
            armSegments.Add(newSegment);
            LineRelativeDirections.Add(Vector2.Perpendicular(newSegment.Start - newSegment.End));
            CurrentFrontSegment = newSegment;

        }
    }
    private void Retract()
    {
        Vector2 targetPos;
        if (armSegments.Count > 1) targetPos = Vector2.MoveTowards(handLink.B.transform.localPosition, CurrentFrontSegment.Start - transform.position, RetractSpeed);
        else targetPos = Vector2.MoveTowards(handLink.B.transform.localPosition, CurrentFrontSegment.Start, RetractSpeed);
        handLink.B.transform.localPosition = targetPos;
        if (Vector2.Distance(handLink.Line.End, CurrentFrontSegment.Start - transform.position) < newSegmentDistance / 2)
        {
            if (armSegments.Count == 1)
            {
                //if the arm is done stretching back lets rotate it along the direction of the starting line
                Vector2 dir = (CurrentFrontSegment.End - CurrentFrontSegment.Start).normalized;
                //Todo: these two scripts should probably be one script and that's why i'm making this gross call.
                FindAnyObjectByType<SpringArmManager>().SetHandPointDirection(dir);
                return;
            }

            armSegments.Remove(CurrentFrontSegment);
            Destroy(CurrentFrontSegment.gameObject);
            CurrentFrontSegment = armSegments[armSegments.Count - 1];

        }
    }

    private void SimulateWave()
    {
        //so we're looping through the arm segments if we have at least 2
        if (armSegments.Count > 2)
        {
            for (int i = 2; i <= armSegments.Count-1; i++)
            {
                armSegments[i].End = armSegments[i - 1].Start;
                if (i > LineRelativeDirections.Count -1) return;
                var pos = armSegments[i].Start + (LineRelativeDirections[i] * Amplitude) * Mathf.Sin(Time.time + i * Frequency);
                armSegments[i].Start = pos;
            }
            //armSegments[armSegments.Count-1].End = armSegments[armSegments.Count - 2].Start;
        }
    }

    private void OnDrawGizmos()
    {
        //for (int i = 1; i < armSegments.Count - 2; i++)
        //{
        //    Vector3 RelativeDirection = Vector2.Perpendicular(armSegments[i].Start - armSegments[i].End);
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(armSegments[i].Start + (RelativeDirection * Amplitude) * Mathf.Sin(Time.time + i * Frequency), .5f);
        //}

        foreach (Line l in armSegments)
        {
            Gizmos.color = Color.green;
            if (l == armSegments[0]) continue;
            Gizmos.DrawSphere(l.Start, .5f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(l.End, .3f);


        }
    }


}
