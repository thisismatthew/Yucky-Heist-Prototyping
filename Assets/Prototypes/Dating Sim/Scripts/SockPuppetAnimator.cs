using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockPuppetAnimator : MonoBehaviour
{
    [SerializeField] private List<Transform> bones;
    [SerializeField] private SpringOnlyArmManager armManager;

    // Update is called once per frame
    void Update()
    {
        //bone_1 needs to point away from the particle at the middle
        //lets get the direction between taht particle and the wrist particle.
        /*        var dir = armManager.MiddleParticle.transform.position - armManager.WristParticle.transform.position;
                dir.Normalize();*/

        Helpers.RotateToFaceAway(bones[0].gameObject, armManager.MiddleParticle.gameObject);

    }
}
