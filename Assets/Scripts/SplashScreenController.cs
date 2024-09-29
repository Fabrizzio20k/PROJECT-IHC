using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour
{
    public Image backgroundImage;  // La imagen de fondo temporal
    public float fadeDuration = 1f;  // Duración del fade

    void Start()
    {
        StartCoroutine(FadeOutBackground());
    }

    IEnumerator FadeOutBackground()
    {
        // Espera un breve momento para asegurarse de que los otros elementos estén listos
        yield return new WaitForSeconds(1f); // Ajusta este valor si necesitas más tiempo de carga

        // Realiza el fade out de la imagen de fondo
        float elapsedTime = 0f;
        Color color = backgroundImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));  // Reducimos el alpha de la imagen
            backgroundImage.color = color;
            yield return null;
        }

        // Finalmente, desactiva la imagen cuando el fade haya terminado
        backgroundImage.gameObject.SetActive(false);
    }
}
