using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private float farArea = 2;
    private float farDamageMultiplier = 75;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity, transform);

        Collider[] colls = Physics.OverlapSphere(transform.position, farArea);

        foreach (Collider coll in colls)
        {
            Pawn pawn = coll.GetComponent<Pawn>();

            if (pawn is IDamageable)
            {
                float distance = Vector3.Distance(coll.transform.position, transform.position);
                float damage = 1 / distance * farDamageMultiplier;

                IDamageable damageablePawn = (IDamageable)pawn;
                damageablePawn.ApplyDamage(damage);
            }
        }

        Destroy(this.gameObject, 2f);
    }

}
