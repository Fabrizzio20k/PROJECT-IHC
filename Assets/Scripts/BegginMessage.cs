using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BegginMessage : MonoBehaviour
{
    private GameObject BegginMessageObject;
    void Start()
    {
        BegginMessageObject = GameObject.Find("BeginMessagePanel");
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(2);
        for (float i = 0; i <= 1; i += Time.deltaTime / 2)
        {
            BegginMessageObject.GetComponent<CanvasGroup>().alpha = i;
            yield return null;
        }
        yield return new WaitForSeconds(5);
        for (float i = 1; i >= 0; i -= Time.deltaTime / 2)
        {
            BegginMessageObject.GetComponent<CanvasGroup>().alpha = i;
            yield return null;
        }
        Destroy(BegginMessageObject);
    }

}
