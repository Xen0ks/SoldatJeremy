using UnityEngine;


public class LVLA : LVL
{
    public Animator doorA;
    public Animator doorB;
    public Animator doorC;
    int doorACount;

    private void Start()
    {
        DialogueSystem.instance.StartDialogue(GetComponent<NPC>().dialogues, GetComponent<NPC>().npcName);
    }

    public void OpenDoorA()
    {
        doorACount++;

        if(doorACount >= 2)
        {
            doorACount = 2;
            doorA.SetBool("Open", true);
            Destroy(doorA.gameObject, 2f);
        }
    }

    public void CloseDoorA()
    {
        if (doorACount >= 2) return;
        doorACount--;
    }

    public void OpenDoorB()
    {
        doorB.SetBool("Open", true);
        Destroy(doorB.gameObject, 2f);

    }

    public void OpenDoorC()
    {
        doorC.SetBool("Open", true);
        Destroy(doorC.gameObject, 2f);

    }
}

public class LVL : MonoBehaviour
{
    public Transform spawnPoint;

    public void StartLevel()
    {
        Player.instance.transform.position = spawnPoint.position;
        if(TryGetComponent<NPC>(out NPC npc))
        {
            DialogueSystem.instance.StartDialogue(npc.dialogues, npc.npcName);
        }
    }
}
