using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int actualLevel = 0;
    [SerializeField] GameObject[] LVLObjects;

    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void NextLevel()
    {
        foreach (var l in LVLObjects)
        {
            l.SetActive(false);
        }

        Invoke(nameof(LoadLevel), 0.5f);
        Transition.instance.PerformTransition();
    }

    void LoadLevel()
    {
        actualLevel++;
        LVLObjects[actualLevel].SetActive(true);
        LVLObjects[actualLevel].GetComponent<LVL>().StartLevel();

    }
}
