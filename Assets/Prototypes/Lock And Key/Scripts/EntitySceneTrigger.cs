using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntitySceneTrigger : MonoBehaviour
{
    public Scene SceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "hand")
        {
            SceneManager.LoadScene(SceneToLoad.buildIndex);
        }
    }
}
