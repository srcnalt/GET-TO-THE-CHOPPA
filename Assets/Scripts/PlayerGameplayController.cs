using UnityEngine;

public class PlayerGameplayController : MonoBehaviour
{
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
                    selectedNicholas = nicholas;
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
                selectedNicholas = null;
            }
        }
    }
}
