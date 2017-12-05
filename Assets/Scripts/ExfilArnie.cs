using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExfilArnie : MonoBehaviour
{
    private bool canSaveArnie = false;
    private bool startEpilogue = false;

    public Transform choppa;
    public List<Transform> wayPoints;
    public Transform camPosition;
    public RectTransform panel;
    public Image whiteScreen;
    public GameObject explosion;
    public AudioSource audioSource;

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
            {
                startEpilogue = true;
                panel.gameObject.SetActive(false);

                GameManager.Instance.Player.gameObject.SetActive(false);
                GameManager.Instance.PlayerCamera.GetComponent<PlayerCamera>().enabled = false;

                Camera.main.transform.position = camPosition.position;

                //GameManager.Instance.HeroSaved();
            }
            else
                GameUI.Instance.ShowNicholasNotSavedInfo();

        }
    }

    private void Update()
    {
        if (startEpilogue)
        {
            Camera.main.transform.LookAt(choppa);

            choppa.position = Vector3.MoveTowards(choppa.position, wayPoints[0].position, .3f);

            if(wayPoints.Count > 0 && choppa.position == wayPoints[0].position)
            {
                wayPoints.RemoveAt(0);
            }

            if (wayPoints.Count == 0)
            {
                Instantiate(explosion, choppa.position, Quaternion.identity);
                StartCoroutine(FadeToWhite());
                startEpilogue = false;
            }
        }
    }

    IEnumerator FadeToWhite()
    {
        yield return new WaitForSeconds(0.5f);
        choppa.gameObject.SetActive(false);

        Color color = new Color(1, 1, 1, 0);

        while (color.a < 1f)
        {
            audioSource.volume -= Time.deltaTime * 10;
            color.a += Time.deltaTime * 1;
            whiteScreen.color = color;
            yield return null;
        }

        SceneManager.LoadScene(2);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
