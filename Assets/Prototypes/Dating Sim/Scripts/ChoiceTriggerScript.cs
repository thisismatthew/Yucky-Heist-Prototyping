using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PixelCrushers.DialogueSystem;
using Shapes;

public class ChoiceTriggerScript : MonoBehaviour
{
    private Disc disc;
    private bool isBeingSelected = false; 
    private bool isHidden = true;
    public bool Chosen = false;
    public Response response;
    public float ChooseSpeedMod = 3;

    private void Start()
    {
        disc = GetComponent<Disc>();
        HideChoice();
    }

    private void Update()
    {
        if (Chosen || isHidden) return;
        if (!isBeingSelected && disc.AngRadiansEnd > disc.AngRadiansStart)
        {
            
            disc.AngRadiansEnd -= Time.deltaTime * 2;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Chosen || isHidden) return;

        isBeingSelected = true;
        if (disc.AngRadiansEnd < disc.AngRadiansStart + 6.28319f)
        {
           
            disc.AngRadiansEnd += Time.deltaTime * ChooseSpeedMod;
        }
        else
        {
            disc.AngRadiansEnd = disc.AngRadiansStart;
            FindObjectOfType<ConversationView>().SelectResponse(new SelectedResponseEventArgs(response));
            Chosen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Chosen || isHidden) return;
        isBeingSelected = false;
    }

    public void RevealChoice()
    {
        isHidden = false;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideChoice()
    {
        isHidden = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
