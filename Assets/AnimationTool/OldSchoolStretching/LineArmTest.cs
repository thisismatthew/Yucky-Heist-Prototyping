using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class LineArmTest : MonoBehaviour
{
    private Vector2 mousePos;
    [SerializeField] private Line currentSegment;
    [SerializeField] private float maximumSegmentLength = 0.1f;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject hand;
    [SerializeField] private float stopRadiusFromMouse = .5f;
    private List<Line> armSegments = new List<Line>();
    public float StretchAmountPerFrame = 0.5f;
    public float RetractSlownessMultiplier = .5f;

    private void Start()
    {
        armSegments.Add(currentSegment);
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && Vector2.Distance(mousePos, hand.transform.position) > stopRadiusFromMouse)
        {
            Debug.Log("Stretching");
            //this.transform.position = mousePos;
            hand.transform.position = Vector2.MoveTowards(hand.transform.position, mousePos, StretchAmountPerFrame);
            currentSegment.Start = hand.transform.position;
            Helpers.RotateToFace(hand, mousePos, 90);


            if(Vector2.Distance(currentSegment.Start, currentSegment.End)> maximumSegmentLength)
            {
                Debug.Log("New Segment Added");
                var newSegment = Instantiate(linePrefab, currentSegment.transform.parent).GetComponent<Line>();
                newSegment.End = currentSegment.Start;
                newSegment.Start = hand.transform.position;
                armSegments.Add(newSegment);
                newSegment.Color = currentSegment.Color;
                currentSegment = newSegment;
            }
        }
        else if (armSegments.Count > 1 && !Input.GetMouseButton(0))
        {
            Debug.Log("Retracting");
            //var middleOfCurrentSegment = Vector2.Lerp(currentSegment.Start, currentSegment.End, 0.5f);
            hand.transform.position = Vector2.MoveTowards(hand.transform.position, currentSegment.Start, StretchAmountPerFrame * RetractSlownessMultiplier);
            Helpers.RotateToFace(hand, currentSegment.Start, 270);

            if (Vector2.Distance(hand.transform.position, currentSegment.Start) < maximumSegmentLength)
            {
                Debug.Log("Segment Deleted");
                armSegments.Remove(currentSegment);
                Destroy(currentSegment.gameObject);
                currentSegment = armSegments[armSegments.Count - 1];
            }
        }
        else if (armSegments.Count == 1 && !Input.GetMouseButton(0))
        {
            if (Vector2.Distance(hand.transform.position, currentSegment.Start) < 0.01)
            {
                hand.transform.position = Vector2.MoveTowards(hand.transform.position, currentSegment.Start, StretchAmountPerFrame * RetractSlownessMultiplier);
                Helpers.RotateToFace(hand, currentSegment.Start, 90);
                Debug.Log("At Base");
            }
        }
    }


    //TODO put these into a helper script rather than hangin here
    //object to object

}
