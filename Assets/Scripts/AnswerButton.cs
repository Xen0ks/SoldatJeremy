using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [HideInInspector] public int nextDialogIndex;
    [HideInInspector] public UnityEvent answerEvent;

    public void Click()
    {
        DialogueSystem.instance.AnswerDialogue(nextDialogIndex, answerEvent);
    }
}
