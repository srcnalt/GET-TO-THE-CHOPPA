using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public List<AudioClip> lines;
    public AudioSource source;

    private int count = 0;

    private void Start()
    {
        InvokeRepeating("SaySomething", 3, 5);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(Scenes.Menu);
        }
    }

    private void SaySomething()
    {
        source.clip = lines[count % lines.Count];
        source.Play();
        count++;

        if(count == lines.Count)
        {
            SceneManager.LoadScene(Scenes.Menu);
        }
    }
}
