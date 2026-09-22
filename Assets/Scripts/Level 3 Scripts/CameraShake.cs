using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalLocalPosition;
    public float shakeMagnitude = 0.05f; // Qué tan fuerte tiembla la cámara
    private bool isShaking = false;

    void Start()
    {
        // Guardamos la posición original de la cámara respecto al jugador
        originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            // Genera una posición aleatoria en un rango pequeño para simular el temblor
            transform.localPosition = originalLocalPosition + Random.insideUnitSphere * shakeMagnitude;
        }
        else
        {
            // Devuelve la cámara a la normalidad cuando no tiembla
            transform.localPosition = originalLocalPosition;
        }
    }

    public void StartShake()
    {
        isShaking = true;
        originalLocalPosition = transform.localPosition;
    }

    public void StopShake()
    {
        isShaking = false;
        transform.localPosition = originalLocalPosition;
    }
}