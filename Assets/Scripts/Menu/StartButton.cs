using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private Animator buttonAnimator;

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void OnMouseOver()
    {
        Debug.Log("yo");
        buttonAnimator.SetBool("Selected", true);
    }

    private void OnMouseExit()
    {
        buttonAnimator.SetBool("Selected", false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
