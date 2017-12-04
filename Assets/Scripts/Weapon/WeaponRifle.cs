using UnityEngine;
using System.Collections;

public class WeaponRifle : WeaponRangedProjectile
{
    private float bulletThrust = 10f;

    public override void Fire(Character character)
    {
        this.fireFrame = Time.frameCount;

        Vector3 pos;
        Quaternion rot;

        Player player = (Player) character;

        PlayerCamera.Instance.CalculateCameraAimTransform(character.transform, player.Pitch, out pos, out rot);

        GameObject projectileObj = GameObject.Instantiate(projectilePrefab, projectileEjector.transform.position, rot);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.Damage = damage;
        projectile.Source = character;

        Rigidbody rigidBody = projectileObj.GetComponent<Rigidbody>();

        rigidBody.AddForce(projectileObj.transform.forward * bulletThrust, ForceMode.Impulse);

        Destroy(projectileObj, 1f);
        PlaySFXAtPos(projectileObj.transform.position);
    }
}
