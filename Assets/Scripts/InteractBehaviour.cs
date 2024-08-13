using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    bool isInRange;
    Camera cam;

    [SerializeField] float interactRange;
    [SerializeField] LayerMask whatIsInteractable;

    // References
    [SerializeField]
    Transform objectPoint;
    Animator objectAnim;
    [SerializeField] Animator armAnim;

    [HideInInspector] public Item item;


    private void Start()
    {
        cam = Camera.main;
        objectAnim = objectPoint.GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, whatIsInteractable))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(hit.transform.TryGetComponent<Item>(out item))
                {
                    ItemInteract();
                }

                if (hit.transform.TryGetComponent<NPC>(out NPC npc))
                {
                    NPCInteract(npc);
                }
            }
        }


        // Créer un autre script pour ce fonctionnement
        if(Input.GetKeyDown(KeyCode.Mouse0) && item != null && !GetComponent<GrabBehaviour>().CanGrab())
        {
            objectAnim.SetTrigger("Slash");
            armAnim.SetBool("Show", false);
            CameraShake.instance.ShakeCamera();
        }


    }


    void ItemInteract()
    {
        item.Interact();
        item.transform.parent = objectPoint;
        item.transform.localPosition = item.item.posOnArm;
        item.transform.localRotation = item.item.rotOnArm;
    }

    void NPCInteract(NPC npc)
    {
        DialogueSystem.instance.StartDialogue(npc.dialogues, npc.npcName, npc.firstTalkEvent);
        npc.firstTalkEvent = null;
    }
}
