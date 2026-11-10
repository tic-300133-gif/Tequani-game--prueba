using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    
    [Header("Configuración")]
    [Tooltip("El tiempo en segundos que tardará en desaparecer tras presionar una tecla.")]
    [SerializeField] private float tiempoDeEspera = 3f;

    // Variable interna para asegurar que el temporizador solo se inicie una vez
    private bool seHaPresionadoTecla = false;

    void Update()
    {
        // Si ya hemos detectado la tecla y empezado la cuenta atrás, no hacemos nada más.
        if (seHaPresionadoTecla) return;

        // Detectar entrada horizontal.
        // Input.GetAxisRaw("Horizontal") devuelve:
        // -1 si se presiona A o Flecha Izquierda
        //  1 si se presiona D o Flecha Derecha
        //  0 si no se presiona nada
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            // Marcamos que se ha presionado para no reiniciar el temporizador si el jugador sigue presionando.
            seHaPresionadoTecla = true;

            // Iniciamos la rutina de espera
            StartCoroutine(OcultarTextoRutina());
        }
    }

    // Esta es la corrutina que maneja la espera
    private IEnumerator OcultarTextoRutina()
    {
        // Espera los segundos definidos en 'tiempoDeEspera' sin congelar el juego
        yield return new WaitForSeconds(tiempoDeEspera);

        // Desactiva el GameObject al que este script está unido.
        // Esto hace que el texto desaparezca.
        gameObject.SetActive(false);

        // NOTA: Si prefieres que el objeto se destruya completamente para liberar memoria
        // (porque nunca más volverá a aparecer), usa la siguiente línea en lugar de SetActive(false):
        // Destroy(gameObject);
    }
}
