using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Nyaargh.Splashscreen
{
    public class NyaarghSplashScreenController : MonoBehaviour
    {
        private const float AudioDelay = 0.5f;
        private const float FadeTime = 2f;

        [SerializeField] private Image splashLogo;
        private AudioSource _audioSource;
        private AsyncOperation asyncSceneLoadOperation;

        private bool canSwitchScene = false;

        private IEnumerator Start()
        {
            StartCoroutine(BackgroundLoadSceneRoutine());
            _audioSource = GetComponent<AudioSource>();

            float t = 0f;

            Color color = splashLogo.color;
            Vector2 minScale = splashLogo.rectTransform.sizeDelta * 0.8f;
            Vector2 maxScale = splashLogo.rectTransform.sizeDelta;

            StartCoroutine(SoundRoutine());

            while (t <= 1f)
            {
                t += Time.deltaTime / FadeTime;
                color.a = Mathf.Lerp(0f, 1f, t);
                splashLogo.color = color;
                splashLogo.rectTransform.sizeDelta = Vector2.Lerp(minScale, maxScale, t);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            t = 0f;

            while (t <= 1f)
            {
                t += Time.deltaTime / FadeTime;
                color.a = Mathf.Lerp(1f, 0f, t);
                splashLogo.color = color;
                // splashLogo.rectTransform.sizeDelta = Vector2.Lerp(maxScale, t);
                yield return null;
            }

            yield return null;

            canSwitchScene = true;
        }

        private void Update()
        {
            if (canSwitchScene) return;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) canSwitchScene = true;
        }

        private IEnumerator SoundRoutine()
        {
            yield return new WaitForSeconds(AudioDelay);
            _audioSource.Play();
        }

        private IEnumerator BackgroundLoadSceneRoutine()
        {
            asyncSceneLoadOperation = SceneManager.LoadSceneAsync(1);
            asyncSceneLoadOperation.allowSceneActivation = false;

            while (!asyncSceneLoadOperation.isDone)
            {
                if (asyncSceneLoadOperation.progress >= 0.9f)
                {
                    if (canSwitchScene) asyncSceneLoadOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

    }
}
