using UnityEngine;

public class PlayerDetectionBehavour : MonoBehaviour
{
    [SerializeField]
    private LayerMask whatIsObstacle;

    [SerializeField]
    private Transform checkPoint;

    [SerializeField]
    float detectionRadius;

    public Transform targetPoint;



    private void Awake()
    {
        
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(checkPoint.position, (targetPoint.position - checkPoint.position).normalized, out hit, detectionRadius, whatIsObstacle) && (targetPoint.position - checkPoint.position).normalized.magnitude > 0)
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.tag == "Player")
            {
                Debug.Log("PLAYER DETECTED : ");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(checkPoint.position, (targetPoint.position - checkPoint.position).normalized * detectionRadius);
    }
}
