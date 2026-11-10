using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    [Header("Configuración de Movimiento Vertical")]
    [Tooltip("Velocidad a la que sube y baja.")]
    [SerializeField] private float speed = 2f;
    [Tooltip("Distancia total que subirá desde donde la colocaste.")]
    [SerializeField] private float moveDistance = 3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    // Usamos una variable para saber si estamos yendo hacia arriba o hacia abajo
    private bool movingUp = true;

    private void Start()
    {
        startPosition = transform.position;
        // --- EL CAMBIO CLAVE ESTÁ AQUÍ ---
        // En lugar de Vector3.right, usamos Vector3.up para que el objetivo sea hacia arriba.
        targetPosition = startPosition + Vector3.up * moveDistance;
    }

    private void FixedUpdate()
    {
        // Si movingUp es verdad, el destino es arriba. Si es falso, el destino es el inicio (abajo).
        Vector3 currentTarget = movingUp ? targetPosition : startPosition;

        // Movemos la plataforma suavemente
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.fixedDeltaTime);

        // Si llegamos al destino, cambiamos la dirección
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            movingUp = !movingUp;
        }
    }

    // --- ESTO ES CRUCIAL PARA PLATAFORMAS VERTICALES ---
    // Si quitas esto, el jugador no bajará suavemente con la plataforma.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // El jugador se vuelve hijo de la plataforma para moverse con ella
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // El jugador se libera al saltar de la plataforma
            collision.transform.SetParent(null);
        }
    }

    // Dibujamos una línea cian en el editor para ver el recorrido vertical
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 endPos = Application.isPlaying ? targetPosition : transform.position + Vector3.up * moveDistance;
        Gizmos.DrawLine(transform.position, endPos);
    }
}