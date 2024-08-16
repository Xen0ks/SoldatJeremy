using UnityEngine;

public class BalancePart : MonoBehaviour
{
    public int count;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            return;
        }
        count++;
        collision.transform.SetParent(transform);

        if(count <= 6)
        {
            transform.position -= new Vector3(0, 0.5f, 0f);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            return;
        }
        count--;
        collision.transform.parent = null;
        if (count >= 0)
        {
            transform.position += new Vector3(0, 0.5f, 0f);
        }
    }
}
