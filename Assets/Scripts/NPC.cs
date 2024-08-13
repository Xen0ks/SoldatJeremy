using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public string npcName;

    public List<DialogContent> dialogues = new List<DialogContent>();
    public UnityEvent firstTalkEvent;

}


[System.Serializable]
public class DialogContent
{
    [TextArea(5, 10)]
    public string dialogue;
    public bool isEnd;
    public List<Response> answers = new List<Response>();
}

[System.Serializable]
public class Response
{
    public string text; // Le texte de la r�ponse
    public int nextDialogIndex; // L'index du dialogue suivant apr�s cette r�ponse
    public UnityEvent answerEvent;
}
