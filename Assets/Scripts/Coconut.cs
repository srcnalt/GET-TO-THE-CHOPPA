using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    public GameObject explosion;

    private float farArea = 2;
    private float farDamageMultiplier = 75;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity, transform);

        Invoke("DestroyCoconut", 2);

        Collider[] colls = Physics.OverlapSphere(transform.position, farArea);

        foreach (Collider coll in colls)
        {
            if(coll.tag == "Enemy")
            {
                float distance = Vector3.Distance(coll.transform.position, transform.position);

                float damage = 1 / distance * farDamageMultiplier;
            }
        }
    }

    private void DestroyCoconut()
    {
        Destroy(this.gameObject);
    }
}
