using UnityEngine;
using System; // Necesario para 'Action'

public class Vida : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private float vidaMaxima = 3;
    [SerializeField] private float vidaActual;

    // Eventos para que otros scripts (como Movimiento o la UI) puedan reaccionar
    public event Action<float, float> OnVidaCambiada; // Argumentos: (vidaActual, vidaMaxima)
    public event Action OnMuerte;                     // Se dispara cuando la vida llega a cero
    public event Action OnRevivir;                    // Se dispara cuando el personaje vuelve a la vida

    // Propiedad para que otros scripts puedan saber si el personaje está vivo
    public bool EstaVivo { get; private set; } // Valor interno para el estado de vida

    // Propiedades para que otros scripts puedan leer la vida actual y máxima
    public float GetVidaActual() => vidaActual;
    public float GetVidaMaxima() => vidaMaxima;

    private void Awake()
    {
        InicializarVida(); // Asegura que la vida se configure al inicio del juego/nivel
    }

    // Método para inicializar o resetear la vida (útil para inicio de nivel o respawn)
    public void InicializarVida()
    {
        vidaActual = vidaMaxima;
        EstaVivo = true;
        OnVidaCambiada?.Invoke(vidaActual, vidaMaxima); // Notifica a los oyentes (ej. barra de vida)
        OnRevivir?.Invoke(); // Notifica que ha revivido
        Debug.Log(gameObject.name + " inicializado con " + vidaActual + "/" + vidaMaxima + " de vida.");
    }

    public void TomarDaño(float daño)
    {
        if (!EstaVivo) return; // Si ya está muerto, no puede recibir más daño

        vidaActual -= daño;
        if (vidaActual < 0) vidaActual = 0; // Asegurarse de que la vida no sea negativa

        OnVidaCambiada?.Invoke(vidaActual, vidaMaxima); // Notifica el cambio de vida

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    public void Curar(float cantidad)
    {
        if (!EstaVivo) return; // No se puede curar a un personaje muerto

        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima; // No exceder la vida máxima

        OnVidaCambiada?.Invoke(vidaActual, vidaMaxima); // Notifica el cambio de vida
    }

    // Función para que otros scripts puedan llamar directamente a revivir
    public void Revivir()
    {
        InicializarVida(); // Resetea la vida al máximo y dispara los eventos correspondientes
    }

    private void Muerte()
    {
        EstaVivo = false; // Marcamos como muerto
        Debug.Log(gameObject.name + " ha muerto.");

        OnMuerte?.Invoke(); // Notifica a los oyentes que el personaje ha muerto
    }
}