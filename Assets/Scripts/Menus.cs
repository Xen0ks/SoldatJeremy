using UnityEngine;

public class Menus : MonoBehaviour
{
    [SerializeField]
    private Vector3[] positions;

    [SerializeField]
    private Transform contentTransform;

    [SerializeField] private GameObject menuUI;
    [HideInInspector] public bool isOpen;

    public int target;


    Vector3 velocity;

    public static Menus instance;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        contentTransform.position = Vector3.SmoothDamp(contentTransform.position, positions[target], ref velocity, 0.1f);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    public void ChangeView(int i)
    {
        if(target + i > 2)
        {
            target = 0;
        }
        else if (target + i < 0)
        {
            target = 2;
        }
        else
        {
            target += i;
        }
    }

    public void ToggleMenu()
    {
        menuUI.SetActive(!menuUI.activeSelf);
        isOpen = menuUI.activeSelf;

        if (isOpen)
        {
            Util.instance.OpenMenu();
        }
        else
        {
            Util.instance.CloseMenu();
        }
    }
}
