using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public Quest quest;

    public void Setup(Quest quest)
    {
        this.quest = quest;
        GetComponent<Button>().onClick.AddListener(delegate { QuestSystem.instance.DisplayQuest(quest); }) ;
        transform.GetChild(0).GetComponent<Text>().text = quest.name;
    }

}
