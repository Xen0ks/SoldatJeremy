using UnityEngine;

public class ScriptsManager : MonoBehaviour
{



    public static ScriptsManager instance;

    private void Awake()
    {
        instance = this;
    }

}
