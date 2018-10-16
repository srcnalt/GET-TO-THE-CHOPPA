using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRangedProjectile : WeaponBase {

    [SerializeField]
    protected GameObject projectilePrefab;

    [SerializeField]
    protected Transform projectileEjector;
}
