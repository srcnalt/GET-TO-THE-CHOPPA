using UnityEngine;

public class Cage : MonoBehaviour {
    public int life;                    //hp of cage
    public GameObject damaged;
    public GameObject undamaged;
    public Transform exitPoint;         //where the prisoner will walk
    public ParticleSystem explosion;


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        
        life -= Random.Range(5, 25);

        if(life <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity, transform);
            undamaged.SetActive(false);
            damaged.SetActive(true);
        }
    }
}
