using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogueBoxScript : MonoBehaviour
{
    public static PlayerDialogueBoxScript instance;

    [Header("References")]
    [SerializeField] Canvas playerUICanvas;
    [SerializeField] GameObject playerUIObj;
    [SerializeField] TMP_Text boxText;
    [SerializeField] List<Button> buttonRef = new List<Button>();
    [SerializeField] List<TMP_Text> buttonTextRef = new List<TMP_Text>();

    [Header("Properties")]
    public int newlineEveryCharacterMessage;
    public int newlineEveryCharacterButton;
    public int framesPerCharacter;

    [HideInInspector] public DialogueSequence currentSequence;
    Coroutine typewritingCoroutine = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Can only have one PlayerDialogueBoxScript at a time!");
        }
    }

    private void Start()
    {
        if (playerUICanvas)
        {
            playerUICanvas.worldCamera = Camera.main;
        }
    }

    public void ForceClosePopup()
    {
        playerUIObj.SetActive(false);
        currentSequence = null;
    }

    public void TogglePopup(bool state)
    {
        playerUIObj.SetActive(state);
    }

    public void SetPlayerPopupText(string text)
    {
        string textWithNewLine = AddNewlineEveryN(text, newlineEveryCharacterMessage);

        if (typewritingCoroutine != null)
        {
            StopCoroutine(typewritingCoroutine);
        }
        typewritingCoroutine = StartCoroutine(TypeWritingCoroutine(textWithNewLine));
    }

    IEnumerator TypeWritingCoroutine(string msg)
    {
        yield return null;
        boxText.text = "";
        for (int i = 0; i < msg.Length; i++)
        {
            boxText.text += msg[i];
            for (int j = 0; j < framesPerCharacter; j++)
            {
                yield return null;
            }
        }
    }

    string AddNewlineEveryN(string input, int maxLineLength)
    {
        if (string.IsNullOrEmpty(input)) return input;
        if (maxLineLength <= 0) return input; // safeguard

        StringBuilder result = new StringBuilder();
        int lineLength = 0;

        string[] words = input.Split(' ');

        foreach (string word in words)
        {
            // +1 for the space if it's not the first word on the line
            int extraSpace = lineLength == 0 ? 0 : 1;

            if (lineLength + word.Length + extraSpace > maxLineLength)
            {
                result.Append('\n');
                lineLength = 0;
                extraSpace = 0;
            }
            else if (lineLength > 0)
            {
                result.Append(' ');
                lineLength++;
            }

            result.Append(word);
            lineLength += word.Length;
        }

        return result.ToString();
    }

    public void SetButtonText(List<string> buttonTextList)
    {
        for (int i = 0; i < buttonRef.Count; i++)
        {
            buttonRef[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < buttonTextList.Count; i++)
        {
            buttonRef[i].gameObject.SetActive(true);
            string textWithNewLine = AddNewlineEveryN(buttonTextList[i], newlineEveryCharacterButton);
            buttonTextRef[i].text = textWithNewLine;
        }
    }

    public void PlayerUIClickButton(int buttonIndex)
    {
        if (currentSequence)
        {
            playerUIObj.SetActive(false);
            currentSequence.MoveOnToNextLine(buttonIndex);
        }
    }

    public void RefreshPlayerLayout()
    {
        StartCoroutine(ForceUpdateLayout(playerUIObj, 10));
    }

    internal IEnumerator ForceUpdateLayout(GameObject panel, int reiterateCount)
    {
        yield return null;
        var layoutgroup = panel.GetComponentInChildren<LayoutGroup>();
        for (int i = 0; i < reiterateCount; i++)
        {
            yield return null;
            if (layoutgroup != null)
            {
                layoutgroup.enabled = false;
                layoutgroup.enabled = true;
            }
        }
    }
}
