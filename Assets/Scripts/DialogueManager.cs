using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; 

    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public bool isTalking = false;

    private string[] currentSentences;
    private int currentLine = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        isTalking = true;
        dialoguePanel.SetActive(true);
        currentSentences = lines;
        currentLine = 0;

        ShowLine();
    }

    public void DisplayNextSentence()
    {
        currentLine++;
        if (currentLine < currentSentences.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowLine()
    {
        // Tách chu?i theo d?u ":" (Ví d?: "T?m: Xin chào" -> Name="T?m", Text=" Xin chào")
        string fullLine = currentSentences[currentLine];
        string[] parts = fullLine.Split(':');

        if (parts.Length > 1)
        {
            nameText.text = parts[0].Trim(); // L?y ch? T?m/Cám
            dialogueText.text = parts[1].Trim(); // L?y n?i dung
        }
        else
        {
            nameText.text = "";
            dialogueText.text = fullLine;
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialoguePanel.SetActive(false);
    }
}