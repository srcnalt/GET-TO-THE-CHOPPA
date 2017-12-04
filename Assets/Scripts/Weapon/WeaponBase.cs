using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected AudioClip[] fireSounds;

    [SerializeField]
    protected byte damage = 25;

    [SerializeField]
    protected int refireRate = 5;

    [SerializeField]
    protected int fireFrame;

    public int RefireRate { get { return refireRate; } }
    public int FireFrame { get { return fireFrame; } }

    public virtual void Fire(Character character)
    {

    }

    public bool CanFire()
    {
        return FireFrame + RefireRate <= Time.frameCount;
    }

    public void PlaySFXAtPos(Vector3 pos)
    {
        int index = Random.Range(0, fireSounds.Length);
        AudioClip audioClip = fireSounds[index];

        GameObject sgxGo = new GameObject("sfx", typeof(AudioSource));
        AudioSource sfx = sgxGo.GetComponent<AudioSource>();

        sfx.PlayOneShot(audioClip);

        Destroy(sgxGo, audioClip.length);
    }

}
