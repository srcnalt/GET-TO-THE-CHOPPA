using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorGun : WeaponRangedProjectile
{
    private float thrust = 5f;

    public void FireAtTarget(Character character, Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();

        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileEjector.transform.position, rot);
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        rigidBody.AddForce(dir * thrust, ForceMode.Impulse);

        Destroy(projectile, 3f);
    }
}
