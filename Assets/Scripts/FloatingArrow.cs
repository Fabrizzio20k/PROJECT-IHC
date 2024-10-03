using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    [SerializeField]
    private float rotationSpeed = 50f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Movimiento vertical con función seno para la flotación
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Movimiento de rotación usando deltaTime para suavidad
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Aplicamos el movimiento en el eje Y manteniendo los otros ejes en la posición original
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        transform.Rotate(-Vector3.forward, rotationAmount, Space.Self);
    }
}
