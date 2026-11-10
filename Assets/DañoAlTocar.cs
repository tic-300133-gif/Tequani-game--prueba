using UnityEngine;

public class DañoAlTocar : MonoBehaviour
{
    public float daño = 10f;
    public float tiempoEntreDaños = 1f;

    private float tiempoSiguienteDaño = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= tiempoSiguienteDaño)
            {
                VidaJugador vidaJugador = other.GetComponent<VidaJugador>();
                if (vidaJugador != null)
                {
                    vidaJugador.TomarDaño(daño);
                }

                tiempoSiguienteDaño = Time.time + tiempoEntreDaños;
            }
        }
    }
}
