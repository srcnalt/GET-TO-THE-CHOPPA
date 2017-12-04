using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExfilArnie : MonoBehaviour
{
    private bool canSaveArnie = false;

    private void Start()
    {
        GameManager.Instance.OnNoMoreNicholasesRemaining += Instance_OnNoMoreNicholasesRemaining;
    }

    private void Instance_OnNoMoreNicholasesRemaining()
    {
        canSaveArnie = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            if (canSaveArnie)
                GameManager.Instance.HeroSaved();
            else
                GameUI.Instance.ShowNicholasNotSavedInfo();

        }
    }


}
