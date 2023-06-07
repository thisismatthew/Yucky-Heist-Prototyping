using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmKeyboardControlls : MonoBehaviour
{
    private SpringOnlyArmManager spm;
    private Vector3 movement;
    public float Speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        spm = GetComponent<SpringOnlyArmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
    }
    private void FixedUpdate()
    {
        if (movement != Vector3.zero)
            spm.MoveWristToPosition(spm.Hand.transform.position + (Speed * movement));
    }
}
