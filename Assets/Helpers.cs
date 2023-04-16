using UnityEngine;

public class Helpers
{
    public static void RotateToFace(GameObject a, GameObject b, float angleOffset = 0)
    {
        //rotates a to face b
        Vector3 vectorToTarget = a.transform.position - b.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        a.transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward); ;
    }

    //object to Vector3
    public static void RotateToFace(GameObject a, Vector3 b, float angleOffset = 0)
    {
        //rotates a to face b
        Vector3 vectorToTarget = a.transform.position - b;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        a.transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward); ;
    }
}


