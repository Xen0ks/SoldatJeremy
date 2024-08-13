using UnityEngine;
using UnityEngine.AI;

public class PlayerDetectionBehavour : MonoBehaviour
{
    [SerializeField]
    float detectionRadius;
    [SerializeField]
    float chaseRadius;
    Transform targetPoint;
    [SerializeField]
    private LayerMask whatIsObstacle;
    [SerializeField]
    private Transform checkPoint;
    Transform target;

    [SerializeField] float rotationSpeed;

    NavMeshAgent navMesh;


    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        target = GameObject.FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        RaycastHit hit;


        Vector3 directionToTarget = (targetPoint.position - target.position).normalized;

        // Raycast pour vérifier s'il y a des obstacles
        if (Physics.Raycast(checkPoint.position, directionToTarget, out hit, detectionRadius, whatIsObstacle))
        {
            // Vérifier si la cible est devant l'objet
            if (Vector3.Dot(transform.forward, directionToTarget) > 0)
            {
                if (hit.transform.CompareTag("Player"))
                {
                    target = hit.transform;
                }
            }
        }

        if(Vector3.Distance(targetPoint.position, checkPoint.position) <= chaseRadius && Vector3.Distance(target.position, checkPoint.position) > 4f)
        {
            Vector3 directionToTargetPoint = (targetPoint.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTargetPoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            navMesh.SetDestination(target.position);
        }
        else
        {
            target = null;
            navMesh.SetDestination(transform.position);
        }
    }
}
