using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public List<AudioClip> lines;
    public AudioSource source;
    public GameObject head2;

    private int count = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InvokeRepeating("SaySomething", 3, 5);
    }

    private void SaySomething()
    {

        source.clip = lines[count % lines.Count];
        source.Play();
        count++;

        head2.SetActive(true);

        Invoke("CloseMouth", source.clip.length);
    }

    public void CloseMouth()
    {
        head2.SetActive(false);
    }
}
