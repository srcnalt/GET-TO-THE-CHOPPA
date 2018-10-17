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

    [SerializeField]
    private LayerMask aiVisionMask;

    [SerializeField]
    private bool checkRaycasts = false;

    [SerializeField]
    private AudioClip[] deathSounds;

    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        CurrentState = State.SearchingForVictim;
    }

    protected override void Update()
    {
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
                //Debug.Log("Attacking poor victim: " + target);
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
            if (!GameManager.Instance.TargetableGoodGuys.Contains(target))
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
                //Debug.Log(distance);

                if (checkRaycasts)
                {
                    Vector3 direction = goodGuy.transform.position - transform.position;
                    direction.Normalize();

                    Ray ray = new Ray(transform.position + transform.up * 3f, direction);

                    Debug.DrawRay(ray.origin, ray.direction, Color.red);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, aiVisionMask)) continue;
                }

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
        else HandleDamage();
    }

    public void Die()
    {
        _animator.SetBool("IsDead", true);
        CurrentState = State.Dead;
        StopAllCoroutines();
        GameManager.Instance.PredatorDied(this);

        MakeDeathSound();

        _navMeshAgent.enabled = false;
        _characterController.enabled = false;

        GetComponent<Collider>().enabled = false;
    }

    private void EnableMovement(bool flag, bool stopImmediately = false)
    {
        //Debug.LogFormat(  "EnableMovement flag: {0} immeadiate?: {1}", flag, immeadiateStop);

        if (!flag && stopImmediately) _navMeshAgent.velocity = Vector3.zero;
        _animator.SetBool("IsRunning", flag);
        _navMeshAgent.isStopped = !flag;
    }


    private void HandleDamage()
    {
        switch (CurrentState)
        {
            case State.SearchingForVictim:
                target = GameManager.Instance.Player;
                CurrentState = State.Attacking;
                break;
        }
    }


    private void MakeDeathSound()
    {
        StartCoroutine(MakeDeathSoundRoutine());
    }

    private IEnumerator MakeDeathSoundRoutine()
    {
        int index = Random.Range(0, deathSounds.Length);
        AudioClip audioClip = deathSounds[index];
        _audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
    }
}

