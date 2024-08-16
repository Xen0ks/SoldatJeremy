using UnityEngine;

public class LVLEND : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(TryGetComponent<NPC>(out NPC npc))
            {
                DialogueSystem.instance.StartDialogue(npc.dialogues, npc.npcName);
                Destroy(npc);
            }
            else
            {
                LevelManager.instance.NextLevel();
            }

        }
    }
}
