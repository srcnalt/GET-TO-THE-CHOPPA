using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected AudioClip fireSound;

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

}
