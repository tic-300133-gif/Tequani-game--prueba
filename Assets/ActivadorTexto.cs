using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorTexto : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí el objeto del texto que está oculto.")]
    [SerializeField] private GameObject textoSaltoOculto;

    [Tooltip("¿Quieres que este texto desaparezca también después de un tiempo? (Opcional)")]
    [SerializeField] private bool desapareceSolo = true;

    // Esta función mágica de Unity detecta cuando algo entra en el área del trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Primero, asegurarnos de que lo que entró es el Jugador y no un enemigo o una bala.
        // (Asegúrate de que tu personaje tenga la etiqueta "Player")
        if (collision.CompareTag("Player"))
        {
            // 1. Activamos el texto para que se vea.
            textoSaltoOculto.SetActive(true);

            // 2. (Opcional) Si el texto activado tiene el script de desaparecer que hicimos antes,
            // y queremos que desaparezca, necesitamos asegurarnos de que su script se active.
            if (desapareceSolo)
            {
                var scriptDesaparecer = textoSaltoOculto.GetComponent<ActivadorTexto>();
                if (scriptDesaparecer != null)
                {
                    // Si lo encontramos, nos aseguramos de que esté listo para funcionar
                    scriptDesaparecer.enabled = true;
                }
            }

            // 3. Destruimos este objeto trigger.
            // ¿Por qué? Porque ya cumplió su función. Si no lo destruimos,
            // y el jugador vuelve a pasar por aquí, intentará activar el texto de nuevo.
            Destroy(gameObject);
        }
    }
}

