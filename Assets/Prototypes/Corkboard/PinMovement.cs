using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinMovement : MonoBehaviour
{
    Collider2D collider;
    private bool pickedUp = false;
    private CorkboardStrings cString;
    private PinMovement TwinPin;
    

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        cString = GetComponentInParent<CorkboardStrings>();
        PinMovement[] pins = cString.gameObject.GetComponentsInChildren<PinMovement>();
        foreach(PinMovement pin in pins)
        {
            if (pin != this)
            {
                TwinPin = pin;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (collider.OverlapPoint(mousePos)&& Input.GetMouseButtonDown(0))
        {
            pickedUp = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pickedUp = false;
        }
        if (pickedUp)
        {

            transform.position = mousePos;
        }
    }
}
