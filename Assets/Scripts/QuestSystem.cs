using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{

    [SerializeField] QuestButton questButtonPrefab;
    [SerializeField] Transform questButtonsParent;

    public List<Quest> unlockedQuests = new List<Quest>();

    [SerializeField] Text questNameText;
    [SerializeField] Text questDescriptionText;
    

    public static QuestSystem instance;

    private void Awake()
    {
        instance = this;
    }


    public void UnlockQuest(Quest quest)
    {
        unlockedQuests.Add(quest);
        UpdateQuestDisplay();
    }

    public void UpdateQuestDisplay()
    {
        for (int i = 0; i < questButtonsParent.childCount; i++)
        {
            Destroy(questButtonsParent.GetChild(i).gameObject);
        }
        foreach (Quest quest in unlockedQuests)
        {
            QuestButton instantiatedButton = Instantiate(questButtonPrefab, questButtonsParent);
            instantiatedButton.Setup(quest);
        }
    }

    public void DisplayQuest(Quest quest)
    {
        questNameText.text = quest.name;
        questDescriptionText.text = quest.descriptions[quest.state-1];
    }
}
