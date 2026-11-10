using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad a la que se mueve la plataforma.")]
    [SerializeField] private float speed = 2f;
    [Tooltip("Distancia total que se moverá hacia la derecha desde su posición inicial.")]
    [SerializeField] private float moveDistance = 3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * moveDistance;
    }

    private void FixedUpdate()
    {
        Vector3 currentTarget = movingRight ? targetPosition : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            movingRight = !movingRight;
        }
    }

    // --- NUEVA LÓGICA PARA PEGAR AL JUGADOR ---

    // Se ejecuta automáticamente cuando otro objeto con collider toca esta plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si el objeto que nos tocó tiene la etiqueta "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // ¡El truco! Hacemos que el jugador sea hijo de la plataforma.
            // Ahora se moverá automáticamente junto con ella.
            collision.transform.SetParent(this.transform);
        }
    }

    // Se ejecuta automáticamente cuando el objeto deja de tocar la plataforma
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verificamos si fue el jugador el que se fue
        if (collision.gameObject.CompareTag("Player"))
        {
            // Rompemos el vínculo. Establecemos su padre en "null" (ninguno).
            // El jugador vuelve a ser independiente en la jerarquía.
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 endPos = Application.isPlaying ? targetPosition : transform.position + Vector3.right * moveDistance;
        Gizmos.DrawLine(transform.position, endPos);
    }
}