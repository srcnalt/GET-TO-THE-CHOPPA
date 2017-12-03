using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponRanged : WeaponBase
{
    [SerializeField]
    protected GameObject shellPrefab;

    [SerializeField]
    protected GameObject impactPrefab;

    [SerializeField]
    protected GameObject trailPrefab;

    [SerializeField]
    protected Transform muzzleFlash;

    [SerializeField]
    protected Transform shellEjector;

    [SerializeField]
    protected AudioClip dryFireSound;

    [SerializeField]
    protected AudioClip reloadSound;

}