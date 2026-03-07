using UnityEditor;

[CustomEditor(typeof(DialogueSequence))]
public class DialogueSequenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Target
        DialogueSequence dialogueSequence = (DialogueSequence)target;

        // Cannot have no dialogue
        if (dialogueSequence.dialogueList.Count == 0)
        {
            dialogueSequence.dialogueList.Add(new Dialogue());
        }

        for (int i = 0; i < dialogueSequence.dialogueList.Count; i++)
        {
            int headerIndex = i + 1;
            string headerText = "(" + headerIndex + ") ";

            if (string.IsNullOrWhiteSpace(dialogueSequence.dialogueList[i].message))
            {
                if (dialogueSequence.dialogueList[i].buttonTextList.Count > 0)
                {
                    string buttonCount = "[Buttons: " + dialogueSequence.dialogueList[i].buttonTextList.Count + "] ";
                    headerText += buttonCount + dialogueSequence.dialogueList[i].buttonTextList[0];
                }
            }
            else
            {
                headerText += "[Message] " + dialogueSequence.dialogueList[i].message;
            }

            dialogueSequence.dialogueList[i].header = headerText;

            if (dialogueSequence.dialogueList[i].buttonTextList.Count > 8)
            {
                // Remove the last element if there are more than 8
                dialogueSequence.dialogueList[i].buttonTextList.RemoveAt(dialogueSequence.dialogueList[i].buttonTextList.Count - 1);
                EditorUtility.DisplayDialog("Button limit reached", "Limited to 8 buttons per dialogue box.", "OK");
            }
        }

        // Show the default inspector with the modified buttonTextList
        DrawDefaultInspector();
    }
}
