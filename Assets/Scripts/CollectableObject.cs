using System.Collections;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    private Depth_ScreenToWorldPosition _manager;
    private bool isCollected = false;

    [SerializeField]
    private float _collectDistance = 3f;

    [SerializeField]
    private GameObject _infoPanelPrefab;

    public void Initialize(Depth_ScreenToWorldPosition manager)
    {
        _manager = manager;
    }

    void Update()
    {
        if (isCollected) return;

        // Detectar si el jugador está lo suficientemente cerca del objeto
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (distance <= _collectDistance)
        {
            // Detectar toques en dispositivos móviles
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Solo procesamos el toque cuando comienza
                if (touch.phase == TouchPhase.Began)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    // Detectar si el toque fue en este objeto
                    int collectableLayerMask = LayerMask.GetMask("Collectable");

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, collectableLayerMask))
                    {
                        if (hit.transform == this.transform)
                        {
                            Collect(); // Recolectar el objeto
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
            _manager.CollectPrefab(gameObject);

            if (_infoPanelPrefab != null)
            {
                // Instanciar el prefab del Canvas con el panel de información
                GameObject infoPanelInstance = Instantiate(_infoPanelPrefab);

                // Iniciar la animación de deslizamiento suave desde arriba
                StartCoroutine(AnimatePanelSlideDown(infoPanelInstance));
            }
        }
    }

    // Corutina para animar el panel deslizándose desde arriba hacia el centro de la pantalla
    private IEnumerator AnimatePanelSlideDown(GameObject panel)
    {
        float duration = 0.75f; // Duración de la animación
        float elapsedTime = 0f;

        RectTransform panelRect = panel.GetComponent<RectTransform>();

        // Asegurarse de que el panel esté activo
        panel.SetActive(true);

        // Guardamos la posición final (donde el panel debe quedarse al final de la animación)
        Vector2 endPosition = panelRect.anchoredPosition;

        // Posición inicial: fuera de la pantalla, por encima del Canvas
        Vector2 startPosition = new Vector2(endPosition.x, Screen.height * 1.5f);  // Empieza bien arriba

        // Colocamos el panel en la posición inicial (fuera de la pantalla)
        panelRect.anchoredPosition = startPosition;

        // Interpolación suave del movimiento
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);

            // Interpolamos la posición de arriba hacia el centro
            panelRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, progress);

            yield return null;
        }

        // Asegurarnos de que el panel termine exactamente en la posición final
        panelRect.anchoredPosition = endPosition;
    }

}
