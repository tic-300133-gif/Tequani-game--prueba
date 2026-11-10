using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Necesario para Image y Text

// Se remueve la dependencia de FollowCameraRotation y HealthSystemForDummies
// [RequireComponent(typeof(FollowCameraRotation))] // Ya no necesitamos esto
public class HealthBar : MonoBehaviour
{
    [SerializeField] bool isBillboarded = true;
    [SerializeField] bool shouldShowHealthNumbers = true;

    // =========================================================
    // *** MODIFICADO: Referencia a tu propio script Vida.cs ***
    // =========================================================
    [Header("Referencias (Asigna en el Inspector)")]
    [Tooltip("Arrastra aquí el script 'Vida' de tu jugador (el componente del Player).")]
    [SerializeField] private Vida playerVida; // ¡Nueva referencia a tu script Vida!
    [Tooltip("Arrastra aquí la imagen que representa el relleno de la barra de vida.")]
    [SerializeField] private Image imageFill; // Renombrado para mayor claridad (antes 'image')
    [Tooltip("Arrastra aquí el componente Text que muestra los números de vida.")]
    [SerializeField] private Text textNumbers; // Renombrado para mayor claridad (antes 'text')
    // =========================================================

    // Caches para componentes opcionales que tu script ya usaba
    private FollowCameraRotation followCameraRotation; // Si quieres mantenerlo, debe estar en el mismo GameObject
    // private HealthSystemForDummies healthSystem; // ¡Ya no se usa!

    // Variables de animación (mantendremos tu lógica de animación)
    float finalValue;
    float animationDuration = 0.1f; // Usamos tu 'animationSpeed' como 'animationDuration'
    float leftoverAmount = 0f; // Tu variable original

    private void Awake()
    {
        // Intentamos obtener FollowCameraRotation si existe en este GameObject
        followCameraRotation = GetComponent<FollowCameraRotation>();

        // Cargar referencias de imagen y texto si no están asignadas en el Inspector (fallback)
        if (imageFill == null) imageFill = GetComponentInChildren<Image>();
        if (textNumbers == null) textNumbers = GetComponentInChildren<Text>();

        // Validar que tenemos todo lo necesario
        if (playerVida == null)
        {
            Debug.LogError("HealthBar: ¡No se ha asignado el script 'Vida' del jugador! Asigna el Player con el script Vida en el Inspector.");
            enabled = false; // Deshabilitar este script si no hay referencia vital
            return;
        }
        if (imageFill == null)
        {
            Debug.LogWarning("HealthBar: No se encontró un componente Image para el relleno de la barra de vida. La barra visual no funcionará.");
        }
        if (textNumbers == null && shouldShowHealthNumbers)
        {
            Debug.LogWarning("HealthBar: No se encontró un componente Text para mostrar los números de vida. Deshabilitando shouldShowHealthNumbers.");
            shouldShowHealthNumbers = false;
        }

        // Suscribirse a los eventos de TU script Vida.cs
        playerVida.OnVidaCambiada += ChangeHealthFill;
        playerVida.OnRevivir += OnPlayerRevive; // Para asegurar que la barra se actualice al revivir

        // Asegurar que la barra se muestre correctamente al inicio
        UpdateHealthBarDisplay(playerVida.GetVidaActual(), playerVida.GetVidaMaxima());
    }

    private void OnDestroy()
    {
        // Es crucial desuscribirse de los eventos cuando el objeto se destruye para evitar errores
        if (playerVida != null)
        {
            playerVida.OnVidaCambiada -= ChangeHealthFill;
            playerVida.OnRevivir -= OnPlayerRevive;
        }
    }

    void Update()
    {
        // No necesitamos 'animationSpeed = healthSystem.AnimationDuration;'
        // La duración de la animación puede ser un valor fijo o configurable aquí.

        // =========================================================
        // *** MODIFICADO: Actualización de texto con TU vida ***
        // =========================================================
        if (textNumbers != null)
        {
            textNumbers.text = $"{playerVida.GetVidaActual()}/{playerVida.GetVidaMaxima()}";
            textNumbers.enabled = shouldShowHealthNumbers;
        }
        // =========================================================

        // =========================================================
        // *** MODIFICADO: Lógica de billboard para la cámara ***
        // =========================================================
        if (followCameraRotation != null)
        {
            followCameraRotation.enabled = isBillboarded;
        }
        // =========================================================
    }

    // =========================================================
    // *** MODIFICADO: Recibe los valores de TU script Vida.cs ***
    // =========================================================
    private void ChangeHealthFill(float currentHealth, float maximumHealth)
    {
        // Convertimos tu 'currentHealth' y 'maximumHealth' a un porcentaje para la barra de vida
        float percentage = (maximumHealth > 0) ? (currentHealth / maximumHealth) * 100 : 0;

        StopAllCoroutines();
        // Le pasamos el porcentaje de vida directamente al Coroutine
        StartCoroutine(ChangeFillAmount(percentage));
    }

    // =========================================================
    // *** NUEVO: Para actualizar la barra cuando el jugador revive ***
    // =========================================================
    private void OnPlayerRevive()
    {
        UpdateHealthBarDisplay(playerVida.GetVidaActual(), playerVida.GetVidaMaxima());
    }

    // =========================================================
    // *** NUEVO: Método para actualizar la barra sin animación (útil al iniciar o revivir) ***
    // =========================================================
    private void UpdateHealthBarDisplay(float currentHealth, float maximumHealth)
    {
        if (imageFill != null && maximumHealth > 0)
        {
            imageFill.fillAmount = currentHealth / maximumHealth;
        }
        if (textNumbers != null)
        {
            textNumbers.text = $"{currentHealth}/{maximumHealth}";
            textNumbers.enabled = shouldShowHealthNumbers;
        }
    }
    // =========================================================

    private IEnumerator ChangeFillAmount(float targetPercentage) // Recibe porcentaje directamente
    {
        if (imageFill == null) yield break; // Salir si no hay imagen de relleno

        // Convertir porcentaje a un valor de 0 a 1 para fillAmount
        finalValue = targetPercentage / 100f;

        // float cacheLeftoverAmount = this.leftoverAmount; // Tu lógica original, puede necesitar ajuste si no viene de HealthSystemForDummies

        float timeElapsed = 0;
        float startFillAmount = imageFill.fillAmount; // Valor actual de la barra al inicio de la animación

        while (timeElapsed < animationDuration)
        {
            // Lerp desde el valor actual de la barra hasta el valor final deseado
            imageFill.fillAmount = Mathf.Lerp(startFillAmount, finalValue, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.leftoverAmount = 0; // Tu lógica original
        imageFill.fillAmount = finalValue; // Asegurarse de que el valor final sea exacto
    }
}
