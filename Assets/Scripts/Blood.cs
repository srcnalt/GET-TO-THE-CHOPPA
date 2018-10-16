using UnityEngine;

public class Blood : MonoBehaviour
{
	void Start () {
        Invoke("RemoveBlood", 1);
	}

    void RemoveBlood()
    {
        Destroy(gameObject);
    }
}
