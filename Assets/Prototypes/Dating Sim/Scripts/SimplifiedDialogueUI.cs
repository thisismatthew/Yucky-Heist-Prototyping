using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class SimplifiedDialogueUI: StandardDialogueUI
{
    
    //public ChoiceTriggerScript[] ChoiceTriggers = new ChoiceTriggerScript[] { };
    public event EventHandler<SelectedResponseEventArgs> SelectedResponseHandler;
    private ChoiceSelectionManager cm;

    /*    
        public void Open()
    {
        // Add your code here to do any setup at the beginning of a conversation -- for example,
        // activating or initializing GUI controls.
    }
    */

    public override void Close()
    {
        Debug.Log("Trig");
        FindAnyObjectByType<ConversationEndTrigger>()?.ConversationEndEffect();
    }
    

    public override void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
    {
        cm = FindObjectOfType<ChoiceSelectionManager>();
        cm.PopulateResponses(responses);
        // Add your code here to show the player response menu. Populate the menu with the contents
        // of the responses array. When the player selects a response, call:
        //  SelectedResponseHandler(this, new SelectedResponseEventArgs(responses[0]));
        // where the argument "response" is the response that the player selected.
        // If (timeout > 0), auto-select a response when timeout seconds have passed.

        
        //find all choice objects in the scene and populate them/link them to this response call
/*        Debug.Log("Show Response Called");
        ChoiceTriggers = FindObjectsOfType<ChoiceTriggerScript>();
        foreach (ChoiceTriggerScript c in ChoiceTriggers)
        {
            c.RevealChoice();
        }
        int choicesInSceneIndex = 0;
        foreach (Response r in responses)
        {
            ChoiceTriggers[choicesInSceneIndex].Chosen = false;
            ChoiceTriggers[choicesInSceneIndex].response = responses[choicesInSceneIndex];
            ChoiceTriggers[choicesInSceneIndex].gameObject.GetComponentInChildren<TextMeshPro>().text = r.destinationEntry.DialogueText;
            choicesInSceneIndex++;
        }*/
        
        
        //Now we need to make the choice triggers actually call the response triggers
    }

    public override void HideResponses()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
