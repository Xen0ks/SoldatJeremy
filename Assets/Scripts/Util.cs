using UnityEngine;

public class Util : MonoBehaviour
{
    public static Util instance;
    private int menuCount = 0;

    // References
    [SerializeField] Transform player; 

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

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<InteractBehaviour>().enabled = false;
        player.GetComponent<GrabBehaviour>().enabled = false;

    }

    public void CloseMenu()
    {
        menuCount--;
        if(menuCount <= 0)
        {
            menuCount = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<InteractBehaviour>().enabled = true;
            player.GetComponent<GrabBehaviour>().enabled = true;
        }
    }
}
