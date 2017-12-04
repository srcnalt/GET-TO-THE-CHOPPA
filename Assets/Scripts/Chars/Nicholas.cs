using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Nicholas : GoodGuy, IDamageable
{
    private const float CloseEnoughDistance = 0.5f;
    private const float CloseEnoughDistanceFollow = 1.5f;

    public enum State
    {
        None,
        Captured,
        Released,
        Wandering,
        Following,
        Dead
    }

    [SerializeField]
    private AudioClip[] randomSounds;

    [SerializeField]
    private Renderer facialExpressionRenderer;

    [SerializeField]
    private Texture2D captured;

    [SerializeField]
    private Texture2D released;

    [SerializeField]
    private Texture2D wandering;

    [SerializeField]
    private Texture2D following;

    [SerializeField]
    private Texture2D dead;

    State currentState = State.None;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private AudioSource _audioSource;

    #region Properties
    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            if (currentState != value)
            {
                State prevState = currentState;
                currentState = value;
                OnStateChanged(prevState, currentState);
            }
        }
    }
    #endregion

    #region Mono
    private void Start()
    {
        CurrentState = State.Captured;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (CurrentState != State.Captured && projectile != null)
        {
            ApplyDamage(projectile.Damage);
            Instantiate(projectile.particle, projectile.transform.position, Quaternion.identity);
            Destroy(projectile.gameObject);
        }
    }

    protected override void Update()
    {

    }

    #endregion

    #region Public Methods

    public void PleaseFollow()
    {
        MakeRandomSound();
        CurrentState = State.Following;
    }

    public void Release()
    {
        transform.SetParent(null);
        CurrentState = State.Released;
    }

    #endregion

    #region State Handling
    private void OnStateChanged(State oldState, State newState)
    {
        switch (oldState)
        {
            case State.Released:
                StopCoroutine(ReleasedRoutine());
                break;
            case State.Wandering:
                StopCoroutine(WanderingRoutine());
                break;
            case State.Following:
                StopCoroutine(FollowingRoutine());
                break;
        }

        switch (newState)
        {
            case State.Captured:
                HandleCaptured();
                break;
            case State.Released:
                HandleReleased();
                break;
            case State.Wandering:
                HandleWandering();
                break;
            case State.Following:
                HandleFollowing();
                break;
            case State.Dead:
                HandleDead();
                break;
        }
    }

    private void HandleCaptured()
    {
        ChangeFacialExpression(captured);
    }

    private void HandleReleased()
    {
        ChangeFacialExpression(released);
        StartCoroutine(ReleasedRoutine());
    }

    private void HandleWandering()
    {
        ChangeFacialExpression(wandering);
        StartCoroutine(WanderingRoutine());
    }

    private void HandleFollowing()
    {
        ChangeFacialExpression(following);
        StartCoroutine(FollowingRoutine());
    }

    private void HandleDead()
    {
        ChangeFacialExpression(dead);
    }

    #endregion

    #region State Routines

    private IEnumerator ReleasedRoutine()
    {
        yield return new WaitForSeconds(1f);

        _navMeshAgent.SetDestination(transform.position + transform.forward * 3f);

        EnableMovement(true);
        MakeRandomSound();
        yield return new WaitUntil(() => IsCloseEnough());
        EnableMovement(false);

        yield return new WaitForSeconds(3f);
        CurrentState = State.Wandering;
    }

    private IEnumerator WanderingRoutine()
    {
        while (CurrentState == State.Wandering)
        {
            Vector3 destinationPoint;
            RandomPoint(transform.position, 10f, out destinationPoint);

            _navMeshAgent.SetDestination(destinationPoint);

            EnableMovement(true);
            MakeRandomSound();
            yield return new WaitUntil(() => IsCloseEnough());
            EnableMovement(false);

            float idleTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(idleTime);
        }
    }

    private IEnumerator FollowingRoutine()
    {
        _navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
        EnableMovement(true);

        float startedFollowingTime = Time.time;
        float timeAfterToLoseInterest = Random.Range(5f, 15f);

        while (CurrentState == State.Following)
        {
            if (startedFollowingTime + timeAfterToLoseInterest < Time.time)
            {
                EnableMovement(false);
                MakeRandomSound();
                _navMeshAgent.SetDestination(Vector3.zero);
                CurrentState = State.Wandering;
            }

            _navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);

            EnableMovement(!IsCloseEnough(), true);

            yield return null;
        }
    }

    #endregion

    private void ChangeFacialExpression(Texture2D expressionTex)
    {
        facialExpressionRenderer.material.SetTexture("_MainTex", expressionTex);
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }


    private float GetDistanceFrom(Vector3 target)
    {
        Vector3 a = transform.position;
        a.y = 0;

        Vector3 b = _navMeshAgent.destination;
        b.y = 0;
        return Vector3.Distance(a, b);

    }

    private float GetDistanceFromTarget()
    {
        return GetDistanceFrom(_navMeshAgent.destination);

    }

    private bool IsCloseEnough()
    {
        float closeEnoughDistance = CurrentState == State.Following ? CloseEnoughDistanceFollow : CloseEnoughDistance;
        return GetDistanceFromTarget() <= closeEnoughDistance;
    }

    private void EnableMovement(bool flag, bool stopImmediately = false)
    {
        //Debug.LogFormat("EnableMovement flag: {0} immeadiate?: {1}", flag, immeadiateStop);

        if (!flag && stopImmediately) _navMeshAgent.velocity = Vector3.zero;
        _animator.SetBool("IsRunning", flag);
        _navMeshAgent.isStopped = !flag;
    }

    public void ApplyDamage(float dmg)
    {
        MakeRandomSound();
        health -= dmg;
        if (health <= 0f && CurrentState != State.Dead) Die();
    }

    public void Die()
    {
        MakeRandomSound();
        _animator.SetBool("IsDead", true);
        CurrentState = State.Dead;
        StopAllCoroutines();
        GameManager.Instance.NicholasDied(this);

        _navMeshAgent.enabled = false;
        _characterController.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    private void MakeRandomSound()
    {
        StartCoroutine(MakeRandomSoundRoutine());
    }

    private IEnumerator MakeRandomSoundRoutine()
    {
        int index = Random.Range(0, randomSounds.Length);
        AudioClip audioClip = randomSounds[index];
        _audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
    }
}
