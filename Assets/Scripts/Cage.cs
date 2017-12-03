using UnityEngine;

public class Cage : MonoBehaviour, IDamageable
{
    private enum State
    {
        Undamaged,
        Damaged
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
        if (collision.gameObject.GetComponent<Projectile>())
        {
            Destroy(collision.gameObject);

            float damage = Random.Range(5, 25);

            ApplyDamage(damage);
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
        undamaged.SetActive(false);
        damaged.SetActive(true);

        nicholas.Release();
    }
}
