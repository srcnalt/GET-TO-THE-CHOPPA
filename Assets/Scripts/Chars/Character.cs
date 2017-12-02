using UnityEngine;

public abstract class Character : MonoBehaviour
{
    #region Interfaces
    public interface IDamageable
    {
        void ApplyDamage(float dmg);
        void Die();
    }
    #endregion

    [SerializeField]
    protected float health = 100;

    [SerializeField]
    protected float maxHealth = 100;

    [Header("Physics")]

    [SerializeField]
    protected float maxSpeed = 5;

    [SerializeField]
    protected float velocityDamping = 5;

    [Header("AudioClips")]

    [SerializeField]
    protected AudioClip[] painSounds;
    [SerializeField]
    protected AudioClip[] deathSounds;
    [SerializeField]
    protected AudioClip[] jumpSounds;

    protected Vector3 velocity;
    protected CharacterController _characterController;

    protected virtual void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        if (_characterController == null) return;

        velocity.x -= velocity.x * velocityDamping * Time.deltaTime;
        velocity.z -= velocity.z * velocityDamping * Time.deltaTime;

        _characterController.Move(velocity * Time.deltaTime);
        if (_characterController.isGrounded) velocity.y = -5;
        else velocity += Physics.gravity * Time.deltaTime;
    }

}


