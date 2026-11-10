using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class CambioEscena : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribe aquí el nombre exacto de la escena a la que quieres ir")]
    public string nombreEscenaDestino;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si lo que entró al área es el personaje
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}