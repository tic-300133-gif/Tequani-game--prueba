using UnityEngine;

public class EfectoMar : MonoBehaviour
{
    [Header("Ajustes de la marea")]
    public float velocidadOlas = 1.2f;    // Qué tan rápido van y vienen las olas
    public float fuerzaX = 0.12f;          // Distancia de avance y retroceso (horizontal)
    public float fuerzaY = 0.05f;          // Sutil subida y bajada (vertical)

    private Vector3 posicionOriginal;

    void Start()
    {
        // Guardamos la posición inicial exacta del agua
        posicionOriginal = transform.position;
    }

    void Update()
    {
        // Generamos un movimiento ondulatorio suave en base al tiempo
        float movimientoX = Mathf.Sin(Time.time * velocidadOlas) * fuerzaX;
        float movimientoY = Mathf.Cos(Time.time * velocidadOlas * 1.5f) * fuerzaY;

        // Aplicamos el movimiento a la capa de agua
        transform.position = new Vector3(
            posicionOriginal.x + movimientoX,
            posicionOriginal.y + movimientoY,
            posicionOriginal.z
        );
    }
}