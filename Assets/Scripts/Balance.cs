using UnityEngine;
using UnityEngine.Events;

public class Balance : MonoBehaviour
{
    [SerializeField] Transform partA;
    [SerializeField] Transform partB;

    [SerializeField] UnityEvent onEvent;


    private void Update()
    {
        if(partA.position.y == partB.position.y)
        {
            if(onEvent != null)
            {
                onEvent.Invoke();
                onEvent = null;
            }

        }
    }
}
