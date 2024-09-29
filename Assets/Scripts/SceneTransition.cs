using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f; // Duración del fade

    private void Start()
    {
        fadeImage.enabled = true;
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        // Llamamos al Fade Out y luego cargamos la escena
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        // Hace el fade in desde un color negro (alpha 1) hasta transparente (alpha 0)
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration)); // Gradualmente reduce el alpha
            fadeImage.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        // Hace el fade out desde transparente (alpha 0) hasta negro (alpha 1)
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Gradualmente aumenta el alpha
            fadeImage.color = color;
            yield return null;
        }

        // Cargar la nueva escena una vez terminado el fade
        SceneManager.LoadScene(sceneName);
    }
}
