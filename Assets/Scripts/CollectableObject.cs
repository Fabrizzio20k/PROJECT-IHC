using System.Collections;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    private Depth_ScreenToWorldPosition _manager;
    private bool isCollected = false;
    private bool isAnimating = false; // Para evitar múltiples corutinas al mismo tiempo

    [SerializeField]
    private float _collectDistance = 3f;

    [SerializeField]
    private GameObject _infoPanelPrefab;

    private GameObject _advicePanel;

    private AudioSource _audioSourceOk;
    private AudioSource _audioSourceError;

    public void Initialize(Depth_ScreenToWorldPosition manager)
    {
        _manager = manager;
    }

    void Start()
    {
        _advicePanel = GameObject.Find("AdvicePanel");
        _audioSourceOk = GameObject.Find("SoundCollect").GetComponent<AudioSource>();
        _audioSourceError = GameObject.Find("Error").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isCollected) return;

        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                int collectableLayerMask = LayerMask.GetMask("Collectable");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, collectableLayerMask))
                {
                    if (hit.transform == this.transform)
                    {
                        if (distance <= _collectDistance)
                        {
                            Collect();
                        }
                        else
                        {
                            Handheld.Vibrate();
                            _audioSourceError.Play();
                            if (!isAnimating)
                            {
                                StartCoroutine(ShowAndHideAdvicePanel());
                            }
                        }
                    }
                }
            }
        }
    }

    private void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;

            ResetAdvicePanel();

            Handheld.Vibrate();
            _audioSourceOk.Play();

            _manager.CollectPrefab(gameObject);

            if (_infoPanelPrefab != null)
            {
                GameObject infoPanelInstance = Instantiate(_infoPanelPrefab);
            }
        }
    }

    private void ResetAdvicePanel()
    {
        CanvasGroup canvasGroup = _advicePanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        isAnimating = false;
    }

    private IEnumerator ShowAndHideAdvicePanel()
    {
        isAnimating = true;

        CanvasGroup canvasGroup = _advicePanel.GetComponent<CanvasGroup>();
        float fadeDuration = 0.5f;
        float waitTime = 2f;

        canvasGroup.alpha = 0f;

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(waitTime);

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, fadeDuration));

        isAnimating = false;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
