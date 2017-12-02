using UnityEngine;

public class Cage : MonoBehaviour
{
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

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);

        life -= Random.Range(5, 25);

        if (life <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity, transform);
            undamaged.SetActive(false);
            damaged.SetActive(true);

            nicholas.Release();
        }
    }

}
