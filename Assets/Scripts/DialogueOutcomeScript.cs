using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueOutcomeScript : MonoBehaviour
{
    [Serializable]
    public class OutcomeData
    {
        public int intData;
        public string stringData;
    }

    [Serializable]
    public class DialogueOutcome
    {
        public OutcomeData data;
        public UnityEvent<OutcomeData> onIndexFired;
    }

    public List<DialogueOutcome> dialogueOutcomeList;

    public void ExecuteOutcome(int buttonIndex)
    {
        dialogueOutcomeList[buttonIndex].onIndexFired.Invoke(dialogueOutcomeList[buttonIndex].data);
    }
}
