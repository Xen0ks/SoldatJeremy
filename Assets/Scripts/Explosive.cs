using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Explosive : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private float power = 5000f;

    Animator anim;
    Collider coll;
    Rigidbody rb;
    MeshRenderer mr;
    [SerializeField] VisualEffect vfx;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
    }

    public IEnumerator Explode()
    {
        anim.SetTrigger("Explosion");
        yield return new WaitForSeconds(2);


        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in colliders)
        {
            if(hit.TryGetComponent<Explosable>(out Explosable e))
            {
                if (e.pushable)
                {
                    e.rb.AddExplosionForce(power, transform.position, radius);
                }
                if(e.destructible)
                {
                    Destroy(e.gameObject);
                }
            }
        }
        Destroy(rb);
        Destroy(coll);
        Destroy(mr);
        Destroy(anim);
        transform.localScale = Vector3.one;
        vfx.Play();
        Destroy(gameObject, 1f);
    }
}
