using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{

    [SerializeField] QuestButton questButtonPrefab;
    [SerializeField] Transform questButtonsParent;

    public List<Quest> unlockedQuests = new List<Quest>();

    

    public static QuestSystem instance;

    private void Awake()
    {
        instance = this;
    }


    public void UpdateQuestDisplay()
    {
        foreach (Quest quest in unlockedQuests)
        {
            QuestButton instantiatedButton = Instantiate(questButtonPrefab, questButtonsParent);
            instantiatedButton.Setup(quest);
        }
    }

    public void DisplayQuest(Quest quest)
    {

    }
}
