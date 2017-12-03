using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Nicholas : Character
{
    private const float CloseEnoughDistance = 0.25f;
    private const float CloseEnoughDistanceFollow = 1.5f;

    public enum State
    {
        None,
        Captured,
        Released,
        Wandering,
        Following
    }

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

    State currentState = State.None;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    #region Properties
    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            if (currentState != value)
            {
                currentState = value;
                OnStateChanged(currentState);
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
    }
    #endregion

    #region Public Methods

    public void Release()
    {
        transform.SetParent(null);
        CurrentState = State.Released;
    }

    #endregion

    #region State Handling
    private void OnStateChanged(State newState)
    {
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

    #endregion

    #region State Routines

    private IEnumerator ReleasedRoutine()
    {
        yield return new WaitForSeconds(1f);

        _navMeshAgent.SetDestination(transform.position + transform.forward * 3f);

        EnableMovement(true);
        yield return new WaitUntil(() => IsCloseEnough());
        EnableMovement(false);

        yield return new WaitForSeconds(3f);
        CurrentState = State.Following;
    }

    private IEnumerator FollowingRoutine()
    {
        _navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
        EnableMovement(true);

        while (CurrentState == State.Following)
        {
            _navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);

            EnableMovement(!IsCloseEnough(), true);

            //Debug.Log(GetDistanceFromTarget());

            yield return null;
        }
    }

    private IEnumerator WanderingRoutine()
    {
        while (CurrentState == State.Wandering)
        {
            Vector3 destinationPoint;
            RandomPoint(transform.position, 10f, out destinationPoint);

            _navMeshAgent.SetDestination(destinationPoint);

            EnableMovement(true);
            yield return new WaitUntil(() => IsCloseEnough());
            EnableMovement(false);

            yield return new WaitForSeconds(3f);
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

    private void EnableMovement(bool flag, bool immeadiateStop=false)
    {
        //Debug.LogFormat("EnableMovement flag: {0} immeadiate?: {1}", flag, immeadiateStop);

        if(!flag && immeadiateStop) _navMeshAgent.velocity = Vector3.zero;
        _animator.SetBool("IsRunning", flag);
        _navMeshAgent.isStopped = !flag;
    }

}
