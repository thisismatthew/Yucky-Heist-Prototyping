using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class ChoiceSelectionManager : MonoBehaviour
{
    private SpringOnlyArmManager armManager;
    private int choiceSelection;
    private ResponseUIObject[] choicePositions;


    private void Start()
    {
        choicePositions = GetComponentsInChildren<ResponseUIObject>();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (armManager == null) 
        {
            try
            {
                armManager = FindObjectOfType<SpringOnlyArmManager>();
            }
            catch
            {
                Debug.LogError("No Spring Only Arm Manager found in scene");
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            choiceSelection--;
            if (choiceSelection < 0)
            {
                choiceSelection = choicePositions.Length - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            choiceSelection++;
            if (choiceSelection > choicePositions.Length-1)
            {
                choiceSelection = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<ConversationView>().
                SelectResponse(new SelectedResponseEventArgs(choicePositions[choiceSelection].response));
            ClearResponses();
        }
        
        armManager.MoveWristToPosition(choicePositions[choiceSelection].gameObject.transform.position);
    }

    public void PopulateResponses(Response[] _responces)
    {
        int i = 0;
        foreach (Response r in _responces)
        {
            choicePositions[i].response = r;
            choicePositions[i].tmp.text = r.destinationEntry.DialogueText;
            i++;
        }
    }

    public void ClearResponses()
    {
        foreach (ResponseUIObject r in choicePositions)
        {
            r.response = null;
            r.tmp.text = "";
        }
    }
}
