using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    [HideInInspector] public string header;

    [TextArea(3,10)]
    public string message;

    [Space(5)]
    public List<string> buttonTextList = new List<string>();

    [Space(5)]
    [Header("Number parameter represent button index pressed (0, 1, 2, 3, etc)")]
    public UnityEvent<int> onButtonPressed;

    public Dialogue()
    {
        buttonTextList.Add("OK");
    }
}

public class DialogueSequence : MonoBehaviour
{
    public bool onlyPlaySequenceOnce;
    public List<Dialogue> dialogueList = new List<Dialogue>();
    bool dialoguePlayed;
    int currentDialougeIndex = -1;
    int prevCurrentDialougeIndex = -1;

    public void StartDialogue()
    {
        if (dialoguePlayed && onlyPlaySequenceOnce)
        {
            return;
        }
        if (PlayerDialogueBoxScript.instance.currentSequence == null)
        {
            PlayerDialogueBoxScript.instance.currentSequence = this;
            currentDialougeIndex = 0;
            ProcessDialogueEntry(dialogueList[currentDialougeIndex]);
            if (!dialoguePlayed)
            {
                dialoguePlayed = true;
            }
        }
    }

    void ProcessDialogueEntry(Dialogue dialogue)
    {
        PlayerDialogueBoxScript.instance.TogglePopup(true);
        PlayerDialogueBoxScript.instance.SetPlayerPopupText(dialogue.message);
        PlayerDialogueBoxScript.instance.SetButtonText(dialogue.buttonTextList);
        PlayerDialogueBoxScript.instance.RefreshPlayerLayout();
    }

    internal void MoveOnToNextLine(int lastPressedButtonIndex)
    {
        prevCurrentDialougeIndex = currentDialougeIndex;
        currentDialougeIndex++;
        if (currentDialougeIndex < dialogueList.Count)
        {
            ProcessDialogueEntry(dialogueList[currentDialougeIndex]);
        }
        else
        {
            // Finished the last dialogue line
            PlayerDialogueBoxScript.instance.currentSequence = null;
        }
        if (lastPressedButtonIndex >= 0)
        {
            InvokeButtonAction(lastPressedButtonIndex);
        }
    }

    void InvokeButtonAction(int inputButtonIndex)
    {
        if (prevCurrentDialougeIndex >= 0)
        {
            dialogueList[prevCurrentDialougeIndex].onButtonPressed.Invoke(inputButtonIndex);
        }
    }
}