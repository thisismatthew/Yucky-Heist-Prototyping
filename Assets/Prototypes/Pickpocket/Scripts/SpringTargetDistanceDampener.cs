using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpringTargetDistanceDampener : MonoBehaviour
{

    public List<SpringJoint2D> Springs;
    private Rigidbody2D connectedRigidBody;
    public float FrequencyMininum, FrequencyMaximum;

    public float HardFollowRadius = 2f;
    public float SoftFollowRadius = 20f;

    /*    
     *  [Header("Depreciated")]
     *  public AnimationCurve FrequencyCurve;
     *  
    */



    private void Start()
    {
        connectedRigidBody = Springs[0].connectedBody;
    }

    private void FixedUpdate()
    {
        float distCheck = Vector2.Distance(connectedRigidBody.position, transform.position);
        foreach (SpringJoint2D s in Springs)
        {
            if (distCheck > HardFollowRadius)
            {
                if (distCheck > SoftFollowRadius)
                {
                    s.frequency = 0;
                    Debug.Log("zero frequency");
                }
                else
                {
                    s.frequency = FrequencyMininum;
                }
            }
            else
            {
                s.frequency = FrequencyMaximum;
                
            }
        }
        //UpdateSpringFrequency();
    }

    /// <summary>
    /// The below does not work, i couldn't find a good distance, or curve or anything to tune this
    /// I also found that the value 1 is great when you are beyond a colider and 15 is great when you are right next to the slide
    /// so maybe i should just make it a binary. 
    /// </summary>
/*    private void UpdateSpringFrequency()
    {
        //ok so this script is here to allow for a taught spring when the target is the ideal distance from the target,
        //but become loose as it gets further away. Ideally this creates a hand that will follow closely but not spring through barriers. 
        for (int i = 0; i <= Springs.Count - 1; i++)
        {

            // we're going to map the distance to an animation curve that is set in the editor
            float dist = Vector2.Distance(Springs[i].transform.position, connectedRigidBody.transform.position);
            //we have to account for the compression or stretch forces, so we want a difference whether we are close or apart
            dist = dist > Springs[i].distance ? dist + Springs[i].distance : dist - Springs[i].distance;
            //we get a value between 0-1 from the curve
            float frequencyScale = FrequencyCurve.Evaluate(dist);
            //which we can use to lerp the frequency
            Springs[i].frequency = Mathf.Lerp(FrequencyMaximum, FrequencyMininum, frequencyScale);
            Debug.Log(Springs[i].name + " Frequency Scale: " + frequencyScale);

        }

    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, HardFollowRadius);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, SoftFollowRadius);
    }
}
