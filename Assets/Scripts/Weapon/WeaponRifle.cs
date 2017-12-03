using UnityEngine;
using System.Collections;

public class WeaponRifle : WeaponBase
{
    [SerializeField]
    protected GameObject projectilePrefab;

    [SerializeField]
    protected Transform projectileEjector;

    private float bulletThrust = 5f;

    public override void Fire(Character character)
    {
        Vector3 pos;
        Quaternion rot;

        Player player = (Player) character;

        PlayerCamera.Instance.CalculateCameraAimTransform(character.transform, player.Pitch, out pos, out rot);

        GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileEjector.transform.position, rot);
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        rigidBody.AddForce(projectile.transform.forward * bulletThrust, ForceMode.Impulse);
    }
}
