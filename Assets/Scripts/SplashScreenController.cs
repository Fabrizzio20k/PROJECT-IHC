using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour
{
    public Image backgroundImage;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeOutBackground());
    }

    IEnumerator FadeOutBackground()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;
        Color color = backgroundImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            backgroundImage.color = color;
            yield return null;
        }
        backgroundImage.gameObject.SetActive(false);
    }
}
