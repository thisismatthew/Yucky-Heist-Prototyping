using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmParticle : MonoBehaviour
{
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float Damping = 0.8f;
    public bool Locked;

    public void ApplyForce(Vector2 force)
    {
        if (Locked) return;
        Acceleration += force;
    }
    public void UpdateParticle()
    {
        if (Locked) return;
        Acceleration = new Vector3(Mathf.Clamp(Acceleration.x, -10, 10), Mathf.Clamp(Acceleration.y, -10, 10), 0);
        Velocity += Acceleration;
        Velocity *= Damping;
        Vector3 vel3 = Velocity;
        transform.position += vel3;
        Acceleration = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, .5f);
    }
}
