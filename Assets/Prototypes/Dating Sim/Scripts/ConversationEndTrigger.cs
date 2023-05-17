using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationEndTrigger : MonoBehaviour
{
    public GameObject obj;
    public void ConversationEndEffect()
    {
        obj.SetActive(true);
    }
}
