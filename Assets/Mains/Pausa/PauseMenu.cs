using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Configuración de Escenas")]
    [Tooltip("El nombre EXACTO de tu escena de menú principal (ej. 'MainMenu' o 'MenuInicial').")]
    [SerializeField] private string nombreEscenaMenuPrincipal = "Menu"; // Corregido: Ahora coincide con el nombre en Build Settings
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

    public void Pausa()
    {
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Salir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenuPrincipal);
    }
}