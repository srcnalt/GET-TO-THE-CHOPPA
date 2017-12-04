using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArnoldAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] releasedCageAudio;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        GameManager.Instance.OnNicholasReleased += Instance_OnNicholasReleased;
    }

    private void Instance_OnNicholasReleased(Nicholas nicholasSaved)
    {
        _audioSource.PlayOneShot(GetRandomAudioClip(releasedCageAudio));
    }

    private AudioClip GetRandomAudioClip(AudioClip[] collection)
    {
        int index = Random.Range(0, collection.Length);
        return collection[index];
    }
}
