using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector] public bool inHand = false;
    public ItemData item;

    Grabable grab;
    private void Awake()
    {
        grab = GetComponent<Grabable>();
    }

    public void Interact()
    {
        Util.instance.SetLayerRecursively(gameObject, 7);
        GetComponent<Grabable>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
}

