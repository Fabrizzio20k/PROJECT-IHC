using UnityEngine;

public enum RotationType
{
    X,
    Y,
    Z
}

public class FloatingObject : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    [SerializeField]
    private float rotationSpeed = 50f;

    private Vector3 startPosition;

    [SerializeField]
    private RotationType rotationAxis = RotationType.Y;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {

        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        float rotationAmount = rotationSpeed * Time.deltaTime;

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        if (rotationAxis == RotationType.X)
            transform.Rotate(Vector3.right, rotationAmount, Space.Self);
        else if (rotationAxis == RotationType.Y)
            transform.Rotate(Vector3.up, rotationAmount, Space.Self);
        else if (rotationAxis == RotationType.Z)
            transform.Rotate(Vector3.forward, rotationAmount, Space.Self);
        else
            Debug.LogError("Invalid rotation axis");
    }
}
