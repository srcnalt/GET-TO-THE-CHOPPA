using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArnoldAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] releasedCageAudio;

    [SerializeField]
    private AudioClip[] randomAudio;

    [SerializeField]
    private AudioClip[] deathSounds;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Player player = GetComponentInParent<Player>();

        player.OnDeath += Player_OnDeath;
        player.OnDamageReceived += Player_OnDamageReceived;

        GameManager.Instance.OnNicholasReleased += Instance_OnNicholasReleased;
        GameManager.Instance.OnNicholasSaved += Instance_OnNicholasSaved;
        GameManager.Instance.OnPredatorDied += Instance_OnPredatorDied;
    }

    private void Player_OnDeath()
    {
        PlayAudioClip(GetRandomAudioClip(deathSounds));
    }

    private void Player_OnDamageReceived()
    {
        PlayRandomAudio();
    }

    private void Instance_OnPredatorDied(Character character)
    {
        PlayRandomAudio();
    }

    private void Instance_OnNicholasSaved(Character character)
    {
        PlayRandomAudio();
    }

    private void Instance_OnNicholasReleased(Character character)
    {
        PlayAudioClip( GetRandomAudioClip(releasedCageAudio) );
    }

    private void PlayRandomAudio()
    {
        PlayAudioClip(GetRandomAudioClip(randomAudio));
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (_audioSource.isPlaying) return;
        _audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomAudioClip(AudioClip[] collection)
    {
        int index = Random.Range(0, collection.Length);
        return collection[index];
    }
}
