using UnityEngine;

public class ItemCapa : MonoBehaviour
{
    [Header("Configuración de Recogida")]
    [Tooltip("Si marcas esto, el objeto se recoge solo con tocarlo. Si lo desmarcas, hay que pulsar la tecla.")]
    [SerializeField] private bool recogerAlTocar = false;

    [Tooltip("La tecla que hay que pulsar para recogerlo (si la opción anterior está desmarcada). Ej: E, F, Space...")]
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;

    // Variables internas para recordar si el jugador está cerca
    private bool jugadorEnRango = false;
    private movimiento scriptDelJugador; // Referencia al script del jugador

    private void Update()
    {
        // Si NO es automático, el jugador está cerca Y presiona la tecla elegida...
        if (!recogerAlTocar && jugadorEnRango && Input.GetKeyDown(teclaInteraccion))
        {
            RecogerObjeto();
        }
    }

    // Cuando el jugador ENTRA en el área del objeto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que sea el Player por su etiqueta
        if (collision.CompareTag("Player"))
        {
            // 1. Recordamos que está cerca
            jugadorEnRango = true;

            // 2. Guardamos una referencia a su script "movimiento" para usarla luego
            scriptDelJugador = collision.GetComponent<movimiento>();

            // 3. Si está configurado para recogerse solo al tocar...
            if (recogerAlTocar)
            {
                RecogerObjeto();
            }
            else
            {
                // (Opcional) Aquí podrías hacer aparecer un texto en pantalla tipo "Presiona E"
                Debug.Log("Jugador cerca. Presiona " + teclaInteraccion + " para recoger.");
            }
        }
    }

    // Cuando el jugador SALE del área sin recogerlo
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ya no está en rango y olvidamos la referencia
            jugadorEnRango = false;
            scriptDelJugador = null;
            // (Opcional) Aquí ocultarías el texto de "Presiona E"
        }
    }

    // La función principal que hace el trabajo sucio
    private void RecogerObjeto()
    {
        // Por seguridad, revisamos que tengamos la referencia al jugador
        if (scriptDelJugador != null)
        {
            Debug.Log("¡Objeto recogido!");

            // 1. LLAMAMOS A LA FUNCIÓN EN EL SCRIPT DEL JUGADOR
            // Esta es la línea mágica que activa el doble salto y la animación
            scriptDelJugador.ActivarHabilidadCapa();

            // 2. (Opcional) Aquí podrías reproducir un sonido de "item obtenido"

            // 3. DESTRUIMOS ESTE OBJETO
            // Lo hacemos desaparecer del mundo porque ya lo tenemos.
            Destroy(gameObject);
        }
    }
}