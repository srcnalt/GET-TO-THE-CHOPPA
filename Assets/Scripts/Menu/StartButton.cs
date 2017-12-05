using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Image blackScreen;

    private Animator buttonAnimator;
    private float fadeSpeed = 1;

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    public void MouseOver()
    {
        buttonAnimator.SetBool("Selected", true);
        GetComponent<AudioSource>().Play();
    }

    public void MouseExit()
    {
        buttonAnimator.SetBool("Selected", false);
    }

    public void MouseDown(string target)
    {
        if (target == "game")
            StartCoroutine("StartGame");
        else if(target == "menu")
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(2);
    }

    public IEnumerator StartGame()
    {
        Color color = new Color(0, 0, 0, 0);

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            blackScreen.color = color;
            yield return null;
        }

        SceneManager.LoadScene("Game");
    }
}
