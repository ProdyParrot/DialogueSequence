using UnityEngine;
using UnityEngine.SceneManagement;

public class TestDialogueScene : MonoBehaviour
{
    [SerializeField] DialogueSequence dialogueSequenceToTest;

    private void Start()
    {
        dialogueSequenceToTest.StartDialogue();
    }

    public void PrintMessage(string message)
    {
        Debug.Log(message);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
