using UnityEngine;
using System.Collections;

public class WeaponRifle : WeaponRangedProjectile
{
    private float bulletThrust = 15f;

    public override void Fire(Character character)
    {
        this.fireFrame = Time.frameCount;

        Vector3 pos;
        Quaternion rot;

        Player player = (Player) character;

        PlayerCamera.Instance.CalculateCameraAimTransform(character.transform, player.Pitch, out pos, out rot);

        GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileEjector.transform.position, rot);
        projectile.GetComponent<Projectile>().Damage = damage;
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        rigidBody.AddForce(projectile.transform.forward * bulletThrust, ForceMode.Impulse);

        Destroy(projectile, 1f);
    }
}
