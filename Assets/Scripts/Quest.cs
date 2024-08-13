using UnityEngine;

public class Quest : MonoBehaviour
{
    public string name;
    [TextArea(5, 10)]
    public string[] descriptions;
    [HideInInspector] public bool finished = false;
    public int maxstate, state;
    bool isNPC;
    NPC npc;

    private void Start()
    {
        maxstate = descriptions.Length;
        isNPC = TryGetComponent<NPC>(out npc);
    }

    public void StartQuest()
    {
        if (state > 0) return;
        QuestSystem.instance.UnlockQuest(this);
        Debug.Log("Starting Quest");
        state = 1;
    }

    public void UpdateQuest()
    {
        if(state <= 0) return;
        state++;

        if(state >= maxstate)
        {
            finished = true;
        }
        QuestSystem.instance.UpdateQuestDisplay();
    }
}
