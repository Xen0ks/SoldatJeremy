using UnityEngine;

public class Quest : MonoBehaviour
{
    public string name;
    public string[] descriptions;
    public bool finished = false;
    public int maxstate, state;


    public void UpdateQuest()
    {
        state++;

        if(state >= maxstate)
        {
            finished = true;
        }
        QuestSystem.instance.UpdateQuestDisplay();
    }
}
