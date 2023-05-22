using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public List<CinemachineVirtualCamera> Cameras;
    private int camIndex = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cameras[camIndex].m_Priority = -1;
            camIndex++;
            if (camIndex > Cameras.Count - 1) camIndex = 0;
            Cameras[camIndex].m_Priority = 10;
        }
    }
}
