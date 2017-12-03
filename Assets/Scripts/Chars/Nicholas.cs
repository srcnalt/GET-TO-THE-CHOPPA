using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Nicholas : Character
{
    private const float CloseEnoughDistance = 0.5f;

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

    private void Start()
    {
        CurrentState = State.Captured;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Release()
    {
        transform.SetParent(null);
        CurrentState = State.Released;
    }

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
    }

    private void ChangeFacialExpression(Texture2D expressionTex)
    {
        facialExpressionRenderer.material.SetTexture("_MainTex", expressionTex);
    }

    private IEnumerator ReleasedRoutine()
    {
        yield return new WaitForSeconds(1f);
        _navMeshAgent.SetDestination(transform.position + transform.forward * 3f);
        yield return new WaitForSeconds(3f);
        CurrentState = State.Wandering;
    }

    private IEnumerator WanderingRoutine()
    {
        while (CurrentState == State.Wandering)
        {
            _navMeshAgent.isStopped = false;

            Vector3 destinationPoint;
            RandomPoint(transform.position, 10f, out destinationPoint);

            _navMeshAgent.SetDestination(destinationPoint);

            yield return new WaitUntil(() => Vector3.Distance(transform.position, destinationPoint) <= CloseEnoughDistance);
            _navMeshAgent.isStopped = true;

            yield return new WaitForSeconds(3f);
        }
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

}
