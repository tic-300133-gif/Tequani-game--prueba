using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSeleccionNiveles : MonoBehaviour
{
    [Header("Paneles de UI")]
    [SerializeField] private GameObject panelSeleccionNiveles;

    // Abrir el panel con los botones de los niveles
    public void AbrirSeleccionNiveles()
    {
        if (panelSeleccionNiveles != null)
        {
            panelSeleccionNiveles.SetActive(true);
        }
    }

    // Cerrar el panel para volver al menú principal
    public void CerrarSeleccionNiveles()
    {
        if (panelSeleccionNiveles != null)
        {
            panelSeleccionNiveles.SetActive(false);
        }
    }

    // Métodos para cargar cada nivel según su nombre de escena
    public void CargarNivel1()
    {
        SceneManager.LoadScene("Nivel1"); // Cambia por el nombre real de tu escena
    }

    public void CargarNivel2()
    {
        SceneManager.LoadScene("Nivel2"); // Cambia por el nombre real de tu escena
    }

    public void CargarNivel3()
    {
        SceneManager.LoadScene("Nivel3"); // Cambia por el nombre real de tu escena
    }
}