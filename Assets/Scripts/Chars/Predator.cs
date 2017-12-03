using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Predator : Character, IDamageable
{
    private const float CloseEnoughShootDistance = 15f;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            Destroy(collision.gameObject);

            float damage = Random.Range(5, 25);

            ApplyDamage(damage);
        }
    }

    public void ApplyDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0f) Die();
    }

    public void Die()
    {
        _animator.SetBool("IsDead", true);
    }

}
