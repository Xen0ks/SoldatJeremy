using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public List<GameObject> uisToDisable = new List<GameObject>();
    public List<GameObject> uisToReenable = new List<GameObject>();

    public Animator dialogueUIAnim;
    public Text dialogueText;
    public Text npcNameText;
    public Text nextButtonText;
    public GameObject answerButtonPrefab;
    public Transform answersButtonsParent;

    private List<DialogContent> sentences = new List<DialogContent>();
    private int actualDiplayedSentence = 0;
    private int nextDialogIndex;

    // On stoque les r�ponses pour pouvoir les supprimer


    public static DialogueSystem instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartDialogue(List<DialogContent> dialogueSentences, string npcName, UnityEvent firstTalkEvent)
    {
        npcNameText.text = npcName;
        firstTalkEvent.Invoke();
        sentences.Clear();
        foreach (var dialogueContent in dialogueSentences)
        {
            sentences.Add(dialogueContent);
        }

        actualDiplayedSentence = 0;


        //D�sactiver les UI a d�sactiver
        foreach (var ui in uisToDisable)
        {
            ui.SetActive(false);
        }

        dialogueUIAnim.SetBool("Show", true);
        Util.instance.OpenMenu();

        DisplayDialogue();
    }
    public void DisplayNextDialogue()
    {
        if (!sentences[actualDiplayedSentence].isEnd)
        {
            actualDiplayedSentence++;
            DisplayDialogue();
            return;
        }
            EndDialogue();
    }

    public void DisplayDialogue()
    {
        dialogueText.text = sentences[actualDiplayedSentence].dialogue;
        if (sentences[actualDiplayedSentence].answers.Count > 0)
        {
            nextButtonText.text = "";
            nextButtonText.transform.GetComponent<Button>().enabled = false;
            DisplayAnswers();
        }
        else
        {
            nextButtonText.transform.GetComponent<Button>().enabled = true;
            nextButtonText.text = "Suite";
            if (sentences[actualDiplayedSentence].isEnd)
            {
                nextButtonText.text = "Fin";
            }
        }
    }
    
    public void DisplayAnswers()
    {
        DestroyAnswers();
        // On cr�er les r�ponses
        answersButtonsParent.GetComponent<Animator>().SetBool("Show", true);
        foreach (var answer in sentences[actualDiplayedSentence].answers)
        {   
            GameObject answerButton = Instantiate(answerButtonPrefab, answersButtonsParent);
            answerButton.transform.GetChild(0).GetComponent<Text>().text = answer.text;
            answerButton.GetComponent<AnswerButton>().nextDialogIndex = answer.nextDialogIndex;
            answerButton.GetComponent<AnswerButton>().answerEvent = answer.answerEvent;
        }
    }

    public void AnswerDialogue(int nextDialogIndex, UnityEvent answerEvent)
    {
        if (actualDiplayedSentence < sentences.Count)
        {
            actualDiplayedSentence = nextDialogIndex;
            answerEvent.Invoke();
            Invoke("DestroyAnswers", 0.3f);
            DisplayDialogue();
            answersButtonsParent.GetComponent<Animator>().SetBool("Show", false);
            return;
        }
        EndDialogue();
    }

    private void DestroyAnswers()
    {
        // On supprime toutes les r�ponses cr�es
        foreach (Transform answerButton in answersButtonsParent)
        {
            Destroy(answerButton.gameObject);
        }
    }



    public void EndDialogue()
    {
        dialogueUIAnim.SetBool("Show", false);
        Util.instance.CloseMenu();
    }
}
