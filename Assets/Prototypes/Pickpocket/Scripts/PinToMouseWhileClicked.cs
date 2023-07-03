using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinToMouseWhileClicked : MonoBehaviour
{

    // 
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            this.transform.position = mousePos;
        }
    }
}
