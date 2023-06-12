using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class TrailOnlyArmManager : MonoBehaviour
{

    public float StretchSpeed = 0.3f;
    public GameObject Hand;

    private Line maskingLine;
    [SerializeField] public Line CurrentFrontSegment;
    [SerializeField] public float newSegmentDistance = 0.1f;

    public float ArmWidth = 1.25f;
    public Color ArmColor;
    private List<Line> armSegments = new List<Line>();
    public float RetractSpeed = .5f;
    
    [Header("Wave Stuff")]
    [SerializeField] public float Amplitude = .0008f;
    [SerializeField] public float Frequency = .5f;
    //this next field should probably be assigned directly to each line segment via the armSpring object when I refactor
    private List<Vector3> LineRelativeDirections = new List<Vector3>();

    private void Start()
    {
        maskingLine = new GameObject().AddComponent<Line>();
        maskingLine.transform.parent = transform;
        //maskingLine.transform.position = Hand.transform.position - transform.position;
        maskingLine.name = "MaskingLine";
        maskingLine.Color = ArmColor;
        maskingLine.Thickness = ArmWidth;

        armSegments.Add(CurrentFrontSegment);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) CreateTrail();
        else Retract();
        UpdateMaskLine(armSegments.Count == 1);
        SimulateWave();
    }

    private void UpdateMaskLine(bool atFirstLink)
    {
        //the mask line connect the gap that can occur between the springy wrist and the trail end (current front segment end)
        maskingLine.Start = Hand.transform.position;
        if (atFirstLink) maskingLine.End = CurrentFrontSegment.Start;
        else maskingLine.End = CurrentFrontSegment.Start;
    }
    private void CreateTrail()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Hand.transform.position = Vector2.MoveTowards(Hand.transform.position, mousePos, StretchSpeed);
        //CurrentFrontSegment.Start = Hand.transform.position;
        Helpers.RotateToFace(Hand, mousePos, 90);

        // if the hand link end gets too far from the current segment's end point spawn a new segment.
        if (Vector2.Distance(CurrentFrontSegment.Start, Hand.transform.position) > newSegmentDistance)
        {
            //Debug.Log(CurrentFrontSegment.Start);
            //Debug.Log(handLink.Line.End + transform.position);
            //why does it create an arm particle at distance 0?
            var newSegment = new GameObject().AddComponent<Line>();
            newSegment.name = "Trail Segment - " + (armSegments.Count - 1);
            newSegment.gameObject.transform.parent = transform;
            newSegment.End = CurrentFrontSegment.Start - transform.position;
            if (armSegments.Count == 1) { newSegment.End += transform.position; }
            newSegment.Start = Hand.transform.position;
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
        SetHandPointDirection((CurrentFrontSegment.Start - CurrentFrontSegment.End).normalized);
        if (armSegments.Count > 1) targetPos = Vector2.MoveTowards(Hand.transform.position, CurrentFrontSegment.Start , RetractSpeed);
        else targetPos = Vector2.MoveTowards(Hand.transform.position, CurrentFrontSegment.Start, RetractSpeed);
        Hand.transform.position = targetPos;
        if (Vector2.Distance(Hand.transform.position, CurrentFrontSegment.Start ) < newSegmentDistance / 2)
        {
            if (armSegments.Count == 1)
            {
                //if the arm is done stretching back lets rotate it along the direction of the starting line
                Vector2 dir = (CurrentFrontSegment.End - CurrentFrontSegment.Start).normalized;
                //Todo: these two scripts should probably be one script and that's why i'm making this gross call.
                SetHandPointDirection(dir);
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
            for (int i = 2; i <= armSegments.Count - 1; i++)
            {
                armSegments[i].End = armSegments[i - 1].Start;
                if (i > LineRelativeDirections.Count - 1) return;
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

    public void SetHandPointDirection(Vector3 handPointDirection)
    {

        Helpers.RotateToFace(Hand, Hand.transform.position + handPointDirection, 90);
    }
}
