using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuItemAnimator : MonoBehaviour
{
    [SerializeField] private Vector2 startAnchorPos;
    [SerializeField] private float timeInSeconds = 2f;
    [SerializeField] private float delay = 0f;

    private IEnumerator Start()
    {
        RectTransform _rectTransform = GetComponent<RectTransform>();
        Vector2 targetPos = _rectTransform.anchoredPosition;

        _rectTransform.anchoredPosition = startAnchorPos;
        yield return new WaitForSeconds(delay);

        float t = 0f;

        while (t <= 1f)
        {
            t += Time.deltaTime / timeInSeconds;
            _rectTransform.anchoredPosition = Vector2.Lerp(startAnchorPos, targetPos, t);
            yield return null;
        }

    }



}
