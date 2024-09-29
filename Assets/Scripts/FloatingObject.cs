using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    // Variables para controlar el movimiento de flotación
    public float amplitude = 0.5f; // Qué tan alto sube y baja el objeto
    public float frequency = 1f;   // Velocidad del movimiento

    [SerializeField]
    private float rotationSpeed = 50f; // Velocidad de rotación del objeto

    private Vector3 startPosition; // Posición inicial del objeto

    void Start()
    {
        // Guardamos la posición inicial del objeto al iniciar
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

        // Rotamos el objeto sobre su propio eje Y de forma local
        transform.Rotate(Vector3.up, rotationAmount, Space.Self);
    }
}
