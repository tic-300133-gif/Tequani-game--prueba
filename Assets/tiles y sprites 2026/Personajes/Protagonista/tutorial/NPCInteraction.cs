using UnityEngine;
using TMPro; // Quita esta línea si usas el Text tradicional de UnityEngine.UI

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject textoInteraccion; // Arrastra tu elemento de texto aquí desde el inspector

    private void Start()
    {
        if (textoInteraccion != null)
        {
            textoInteraccion.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entró tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            textoInteraccion.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Al alejarse el jugador, oculta el texto
        if (other.CompareTag("Player"))
        {
            textoInteraccion.SetActive(false);
        }
    }
}