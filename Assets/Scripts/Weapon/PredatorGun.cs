using UnityEngine;

public class PredatorGun : WeaponRangedProjectile
{
    private float thrust = 20f;

    public void FireAtTarget(Character character, Transform target)
    {
        this.fireFrame = Time.frameCount;

        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();

        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject projectileObj = GameObject.Instantiate(projectilePrefab, projectileEjector.transform.position, rot);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.Damage = damage;
        projectile.Source = character;

        Rigidbody rigidBody = projectileObj.GetComponent<Rigidbody>();

        rigidBody.AddForce(dir * thrust, ForceMode.Impulse);

        Destroy(projectileObj, 3f);
        PlaySFXAtPos(projectileObj.transform.position);
    }


}
