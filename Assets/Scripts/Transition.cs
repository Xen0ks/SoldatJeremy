using UnityEngine;

public class Transition : MonoBehaviour
{
    public static Transition instance;

    private void Awake()
    {
        instance = this;
    }

    public void PerformTransition()
    {
        GetComponent<Animator>().SetTrigger("Transition");
    }
}
