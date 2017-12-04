using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public List<AudioClip> lines;
    public AudioSource source;

    private int count = 0;

    private void Start()
    {
        InvokeRepeating("SaySomething", 3, 5);
    }

    private void SaySomething()
    {
        source.clip = lines[count % lines.Count];
        source.Play();
        count++;

        if(count == lines.Count)
        {
            Application.Quit();
        }
    }
}
