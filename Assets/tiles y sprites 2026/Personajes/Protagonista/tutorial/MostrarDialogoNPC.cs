using UnityEngine;

public class MostrarDialogoNPC : MonoBehaviour
{
    [Header("Arrastra el panel café aquí")]
    public GameObject panelDialogo;

    private void Start()
    {
        // Apaga el panel al iniciar el juego
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enciende el panel cuando el Player entra en el área
        if (collision.CompareTag("Player"))
        {
            if (panelDialogo != null)
            {
                panelDialogo.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Apaga el panel cuando el Player se aleja
        if (collision.CompareTag("Player"))
        {
            if (panelDialogo != null)
            {
                panelDialogo.SetActive(false);
            }
        }
    }
}