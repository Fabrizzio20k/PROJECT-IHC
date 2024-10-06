using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIntitPage : MonoBehaviour
{
    private GameObject InitPage;
    void Start()
    {
        InitPage = GameObject.Find("InitPage");
        StartCoroutine(FadeOutBackground());
    }

    IEnumerator FadeOutBackground()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime / 2)
        {
            InitPage.GetComponent<CanvasGroup>().alpha = i;
            yield return null;
        }
        Destroy(InitPage);
    }
}
