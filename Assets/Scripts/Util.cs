using UnityEngine;

public class Util : MonoBehaviour
{
    public static Util instance;
    private int menuCount = 0;

    

    // References
    [SerializeField] Transform player;
    [SerializeField] MouseLook mouseLook;

    private void Awake()
    {
        instance = this;
    }

    public void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SetLayerRecursively(obj.transform.GetChild(i).gameObject, layer);
        }
    }


    public void OpenMenu()
    {
        menuCount++;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<InteractBehaviour>().enabled = false;
        player.GetComponent<GrabBehaviour>().enabled = false;
        player.GetComponent<StickBehaviour>().enabled = false;
        mouseLook.enabled = false;

    }

    public void CloseMenu()
    {
        menuCount--;
        if(menuCount <= 0)
        {
            menuCount = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<InteractBehaviour>().enabled = true;
            player.GetComponent<GrabBehaviour>().enabled = true;
            player.GetComponent<StickBehaviour>().enabled = true;

            mouseLook.enabled = true;

        }
    }
}
