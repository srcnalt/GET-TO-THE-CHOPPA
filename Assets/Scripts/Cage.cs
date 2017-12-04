using UnityEngine;

public class Cage : MonoBehaviour, IDamageable
{
    private enum State
    {
        Undamaged,
        Damaged,
        Dead
    }

    [SerializeField]
    private int life;

    [SerializeField]//hp of cage
    private GameObject damaged;

    [SerializeField]
    private GameObject undamaged;

    [SerializeField]
    private Transform exitPoint;

    [SerializeField]//where the prisoner will walk
    private ParticleSystem explosion;

    [SerializeField]
    private Nicholas nicholas;

    private State state = State.Undamaged;

    private void OnCollisionEnter(Collision collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            ApplyDamage(projectile.Damage);
            Destroy(projectile.gameObject);
        }
    }

    public void ApplyDamage(float dmg)
    {
        life -= (int)dmg;

        if (life <= 0 && state == State.Undamaged) Die();
    }

    public void Die()
    {
        state = State.Damaged;

        Instantiate(explosion, transform.position, Quaternion.identity, transform);

        Invoke("ShowDamaged", 1.2f);

        nicholas.Release();
        GameManager.Instance.NicholasReleased(nicholas);
    }

    private void ShowDamaged()
    {
        undamaged.SetActive(false);
        damaged.SetActive(true);
    }
}
