using UnityEngine;

public class Helpers
{
    public static void RotateToFaceAway(GameObject a, GameObject b, float angleOffset = 0)
    {
        //rotates a to face b
        Vector3 vectorToTarget = b.transform.position- a.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        a.transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward); 
    }
    public static void RotateToFace(GameObject a, GameObject b, float angleOffset = 0)
    {
        //rotates a to face b
        Vector3 vectorToTarget = a.transform.position - b.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        a.transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward); 
    }

    //object to Vector3
    public static void RotateToFace(GameObject a, Vector3 b, float angleOffset = 0)
    {
        //rotates a to face b
        Vector3 vectorToTarget = a.transform.position - b;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        a.transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward); 
    }

    public static void SlerpToFace(GameObject a, Vector3 b, float speed = 1f, float angleOffset = 0)
    {
        //rotates a to face b
        Vector3 vectorToTarget = a.transform.position - b;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);
        a.transform.rotation = Quaternion.Slerp(a.transform.rotation, q, Time.deltaTime * speed);
    }

}


