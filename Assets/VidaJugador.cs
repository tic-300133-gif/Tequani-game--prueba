using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public float vidaMax = 100;
    private float vidaActual;

    private void Start()
    {
        vidaActual = vidaMax;
    }

    public void TomarDaño(float daño)
    {
        vidaActual -= daño;
        Debug.Log("Jugador recibió daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log("Jugador murió");
        // Aquí luego metemos animación + pantalla de muerte + respawn
    }
}
