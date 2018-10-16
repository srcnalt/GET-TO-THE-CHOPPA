using Sacristan.Utils;
using UnityEngine;

public class PlayerGameplayController : Singleton<PlayerGameplayController>
{
    public delegate void EventHandler();
    public event EventHandler OnNickolasRequestFollow;

    private static Nicholas selectedNicholas = null;

    private void Start()
    {
        selectedNicholas = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Nicholas nicholas = other.gameObject.GetComponent<Nicholas>();

        if (nicholas != null)
        {
            switch (nicholas.CurrentState)
            {
                case Nicholas.State.Released:
                case Nicholas.State.Wandering:
                    var heading = nicholas.transform.position - transform.position;
                    float dot = Vector3.Dot(heading, transform.forward);

                    if (dot > 0f) selectedNicholas = nicholas;

                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Nicholas nicholas = other.gameObject.GetComponent<Nicholas>();

        if (nicholas != null)
        {
            if (selectedNicholas == nicholas) selectedNicholas = null;
        }
    }

    private void Update()
    {

        bool isNicholasSelected = selectedNicholas != null;
        GameUI.Instance.ShowFollowInstructions(isNicholasSelected);

        if (isNicholasSelected)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                selectedNicholas.PleaseFollow();
                if (OnNickolasRequestFollow != null) OnNickolasRequestFollow.Invoke();
                selectedNicholas = null;
            }
        }
    }
}
