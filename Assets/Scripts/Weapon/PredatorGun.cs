using UnityEngine;

public class PredatorGun : WeaponRangedProjectile
{
    private float thrust = 7.5f;

    public void FireAtTarget(Character character, Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();

        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileEjector.transform.position, rot);
        projectile.GetComponent<Projectile>().Damage = damage;
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        rigidBody.AddForce(dir * thrust, ForceMode.Impulse);

        Destroy(projectile, 3f);

        this.fireFrame = Time.frameCount;
    }

    public bool CanFire()
    {
        return FireFrame + RefireRate <= Time.frameCount;
    }
}
