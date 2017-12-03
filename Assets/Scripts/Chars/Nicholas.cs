using UnityEngine;

public class Nicholas : Character
{
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

    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            if(currentState != value)
            {
                currentState = value;
                OnStateChanged(currentState);
            }
        }
    }

    private void Start()
    {
        CurrentState = State.Captured;
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
    }

    private void HandleWandering()
    {
        ChangeFacialExpression(wandering);
    }

    private void HandleFollowing()
    {
        ChangeFacialExpression(following);
    }

    private void ChangeFacialExpression(Texture2D expressionTex)
    {
        facialExpressionRenderer.material.SetTexture("_MainTex", expressionTex);
    }
}
