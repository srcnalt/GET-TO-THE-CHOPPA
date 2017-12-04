using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Predator : BadGuy, IDamageable
{
    public enum State
    {
        None,
        SearchingForVictim,
        Attacking,
        Dead
    }

    private const float CloseEnoughDetectionDistance = 35f;
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
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (projectile != null && projectile.Source != this)
        {
            ApplyDamage(projectile.Damage);
            Instantiate(projectile.particle, projectile.transform.position, Quaternion.identity);
            Destroy(projectile.gameObject);
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
            if(!GameManager.Instance.TargetableGoodGuys.Contains(target))
            {
                CurrentState = State.SearchingForVictim;
                yield break;
            }

            if (IsCloseEnoughToTarget())
            {
                EnableMovement(false, true);

                Vector3 lookAtPos = new Vector3(
                    target.transform.position.x,
                    transform.position.y,
                    target.transform.position.z
                );

                transform.LookAt(lookAtPos);

                if (predatorGun.CanFire()) ShootAtTarget();
            }
            else
            {
                _navMeshAgent.SetDestination(target.transform.position);
                EnableMovement(true);
            }

            yield return null;
        }
    }

    private void ShootAtTarget()
    {
        predatorGun.FireAtTarget(this, target.transform);
    }

    private GoodGuy FindCloseEnoughTarget()
    {
        GoodGuy[] goodGuys = GameManager.Instance.TargetableGoodGuys.ToArray();

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
        if (health <= 0f && CurrentState != State.Dead) Die();
    }

    public void Die()
    {
        _animator.SetBool("IsDead", true);
        CurrentState = State.Dead;
        StopAllCoroutines();
        GameManager.Instance.PredatorDied(this);
    }

    private void EnableMovement(bool flag, bool stopImmediately = false)
    {
        //Debug.LogFormat(  "EnableMovement flag: {0} immeadiate?: {1}", flag, immeadiateStop);

        if (!flag && stopImmediately) _navMeshAgent.velocity = Vector3.zero;
        _animator.SetBool("IsRunning", flag);
        _navMeshAgent.isStopped = !flag;
    }

}
