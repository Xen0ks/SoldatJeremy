using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform spine;
    [SerializeField] Transform gun;
    [SerializeField] Transform gunPoint;
    [SerializeField] float rotationSpeed = 4;
    [SerializeField] LayerMask whatIsObstacle;
    [SerializeField] float detectionRadius;
    [SerializeField] float chaseRadius;
    [SerializeField] GameObject bulletPrefab;
    Animator anim;

    NavMeshAgent navMesh;

    bool detected;
    bool shooting;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        target = GameObject.FindObjectOfType<PlayerMovement>().transform;
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        // Faire en sorte que la colonne vertébrale de l'ennemi regarde vers la cible
        spine.LookAt(target);

        // Calculer la distance entre l'ennemi et le joueur
        float distanceToTarget = Vector3.Distance(target.position, transform.position);

        // Calculer la direction vers le joueur
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // Calculer l'angle entre la direction de l'ennemi et la direction vers le joueur
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // Vérifier si le joueur est dans le champ de vision et dans le rayon de poursuite
        if (angleToTarget <= 180f / 2 && distanceToTarget <= chaseRadius && distanceToTarget > 2f)
        {
            detected = true;

            // Tourner l'ennemi vers la cible
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Déplacer l'ennemi vers la cible
            navMesh.SetDestination(target.position);
            anim.SetBool("chase", true);

            // Commencer à tirer si l'ennemi ne tire pas déjà
            if (!shooting)
            {
                StartCoroutine(Shot());
            }
        }
        else
        {
            // Si le joueur n'est plus dans le champ de vision, arrêter la poursuite
            navMesh.SetDestination(transform.position);
            StopCoroutine(Shot());
            detected = false;
            shooting = false;
            anim.SetBool("chase", false);
        }

        // Positionner et orienter l'arme
        gun.position = gunPoint.position;
        gun.rotation = gunPoint.rotation;
    }

    IEnumerator Shot()
    {
        shooting = true;

        while (shooting)
        {
            yield return new WaitForSeconds(0.4f);
            Transform instantiatedBullet = Instantiate(bulletPrefab).transform;

            instantiatedBullet.position = gunPoint.position;

            instantiatedBullet.GetComponent<Rigidbody>().AddForce(spine.transform.forward * 5000f);

            Destroy(instantiatedBullet.gameObject, 2f);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(gunPoint.position, detectionRadius);
        Gizmos.DrawWireSphere(gunPoint.position, chaseRadius);
        Vector3 directionToTarget = (target.position - spine.position).normalized;

        Gizmos.DrawRay(spine.position, directionToTarget);
    }
}
