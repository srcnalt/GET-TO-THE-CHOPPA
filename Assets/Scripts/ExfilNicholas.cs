using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExfilNicholas : MonoBehaviour
{
    private bool canSaveArnie = false;

    public void OnTriggerEnter(Collider other)
    {

        Nicholas nicholas = other.GetComponent<Nicholas>();

        if (nicholas != null)
        {
            GameManager.Instance.NicholasSaved(nicholas);
            Destroy(nicholas.gameObject);
        }

    }

}
