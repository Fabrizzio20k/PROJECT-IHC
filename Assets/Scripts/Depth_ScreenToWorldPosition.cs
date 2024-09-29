using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PrefabData
{
    public GameObject prefab;
}

public class Depth_ScreenToWorldPosition : MonoBehaviour
{
    [SerializeField]
    private ARRaycastManager _raycastManager;  // Usaremos esto para raycast sobre los planos

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private List<PrefabData> _prefabsData;

    [SerializeField]
    private TextMeshProUGUI _collectionCounterText;

    [SerializeField]
    private int _maxObjects = 15; // Máximo número de objetos que se pueden generar

    [SerializeField]
    private float _spawnInterval = 5f; // Intervalo entre spawns

    [SerializeField]
    private float distance = 2f; // Distancia a la que se generan los objetos

    private int _spawnedObjectsCount = 0; // Contador de objetos spawneados
    private int _collectedCount = 0; // Contador de recolectados
    private int _availablePrefabsCount; // Número de prefabs disponibles
    private List<GameObject> _spawnedObjects = new List<GameObject>(); // Lista de objetos spawneados

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private void OnEnable()
    {
        _availablePrefabsCount = Mathf.Min(_prefabsData.Count, _maxObjects);

        // Iniciar la generación de objetos
        StartCoroutine(SpawnObjectsAtIntervals());
        UpdateCollectionCounter(); // Actualizar el contador al inicio
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnObjectsAtIntervals());
    }

    private IEnumerator SpawnObjectsAtIntervals()
    {
        while (_spawnedObjectsCount < _availablePrefabsCount)
        {
            yield return new WaitForSeconds(_spawnInterval);
            SpawnRandomPrefab();
        }
    }

    private void SpawnRandomPrefab()
    {
        if (_prefabsData.Count > 0)
        {
            // Seleccionar un prefab aleatorio de la lista
            var prefabData = _prefabsData[Random.Range(0, _prefabsData.Count)];

            // Obtener la posición en el centro de la pantalla
            Vector2 screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

            // Realizar un raycast sobre los planos detectados
            if (_raycastManager.Raycast(screenPosition, _hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = _hits[0].pose;

                // Calcular la dirección desde la cámara hacia el plano detectado
                Vector3 direction = (hitPose.position - _camera.transform.position).normalized;

                // Calcular la nueva posición del objeto, en la dirección del plano detectado pero más lejos de la cámara
                Vector3 spawnPosition = _camera.transform.position + direction * distance;

                var spawnedObject = Instantiate(prefabData.prefab, spawnPosition, prefabData.prefab.transform.rotation);
                _spawnedObjects.Add(spawnedObject);
                spawnedObject.GetComponent<CollectableObject>().Initialize(this);

                _prefabsData.Remove(prefabData);

                _spawnedObjectsCount++;
                UpdateCollectionCounter();
            }
        }
    }




    // Método que se llama al recolectar un prefab
    public void CollectPrefab(GameObject prefab)
    {
        _spawnedObjects.Remove(prefab);
        Destroy(prefab);
        _collectedCount++;
        UpdateCollectionCounter();
    }

    private void UpdateCollectionCounter()
    {
        _collectionCounterText.text = $"Collected:{_collectedCount} / {_spawnedObjectsCount + _prefabsData.Count}";
    }
}
