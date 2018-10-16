using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage { get; set; }
    public Character Source { get; set; }

    public GameObject particle;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.1f);
    }
}