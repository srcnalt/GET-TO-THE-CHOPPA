using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Predator : BadGuy, IDamageable
{
    public enum State
    {
        None,
        SearchingForVictim,
        Attacking
    }

    private const float CloseEnoughDetectionDistance = 15f;
    private const float CloseEnoughShootDistance = 25f;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private GoodGuy target;
    private State currentState = State.None;

    [SerializeField]
    private PredatorGun predatorGun;

    public State CurrentState
    {
        get { return currentState; }

        private set
        {
            if (value != currentState)
            {
                State oldState = currentState;
                currentState = value;
                OnStateChanged(oldState, currentState);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        CurrentState = State.SearchingForVictim;
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

    private void OnStateChanged(State oldState, State newState)
    {
        switch (newState)
        {
            case State.SearchingForVictim:
                StartCoroutine(SearchingRoutine());
                break;

            case State.Attacking:
                Debug.Log("Attacking poor victim: " + target);
                StartCoroutine(AttackingRoutine());
                break;

        }
    }

    private IEnumerator SearchingRoutine()
    {
        while (CurrentState == State.SearchingForVictim)
        {
            target = FindCloseEnoughTarget();

            if (target != null)
            {
                CurrentState = State.Attacking;
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator AttackingRoutine()
    {
        while (currentState == State.Attacking)
        {
            //if (IsCloseEnoughToTarget()) yield return Shoot();

            predatorGun.FireAtTarget(this, target.transform);

            yield return new WaitForSeconds(2f);
        }
    }

    private GoodGuy FindCloseEnoughTarget()
    {
        GoodGuy[] goodGuys = GameManager.Instance.TargetableGoodGuys;

        //Debug.Log("GoodGuys: "+goodGuys.Length);

        float closestDistance = float.PositiveInfinity;
        GoodGuy result = null;

        for (int i = 0; i < goodGuys.Length; i++)
        {
            GoodGuy goodGuy = goodGuys[i];
            float distance;

            if (IsCloseEnough(goodGuy.transform.position, out distance))
            {
                Debug.Log(distance);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    result = goodGuy;
                }
            }

        }

        return result;
    }

    private bool IsCloseEnoughToTarget()
    {
        float dist;
        return IsCloseEnough(target.transform.position, out dist);
    }

    private bool IsCloseEnough(Vector3 pos, out float dist)
    {
        Vector3 a = pos;
        a.y = 0;

        Vector3 b = transform.position;
        b.y = 0;

        dist = Vector3.Distance(a, b);

        float tresholdDistance = CurrentState == State.Attacking ? CloseEnoughShootDistance : CloseEnoughDetectionDistance;
        return dist <= tresholdDistance;
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
