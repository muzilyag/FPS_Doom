using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlackOut : MonoBehaviour
{
    public Image blackImage;
    public float fadeDuration = 3f;

    public void StartFade()
    {
        StartCoroutine(FadeOut());
        //blackImage.gameObject.SetActive(true); 
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = blackImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f);

        while(timer < fadeDuration)
        {
            blackImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        blackImage.color = endColor;
    }
}
