using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =========================================================
// *** NUEVO: Necesario para que Unity añada automáticamente un AudioSource ***
// =========================================================
[RequireComponent(typeof(AudioSource))]
public class movimiento : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animator;
    public ParticleSystem ParticulasCorrer;

    // =========================================================
    // *** NUEVO: AUDIO SOURCE Y CLIPS DE SONIDO ***
    // =========================================================
    [Header("Audio del Jugador")]
    // La referencia al AudioSource ahora se puede autocompletar con [RequireComponent]
    private AudioSource playerAudioSource;
    [Tooltip("Clip de sonido para caminar (pasos).")]
    [SerializeField] private AudioClip sonidoPasos;
    [Tooltip("Clip de sonido para el salto.")]
    [SerializeField] private AudioClip sonidoSalto;
    [Tooltip("Clip de sonido para el ataque (si lo tienes).")]
    [SerializeField] private AudioClip sonidoAtaque;

    [Tooltip("El volumen para los pasos.")]
    [SerializeField] private float volumenPasos = 0.5f;
    [Tooltip("El volumen para el salto.")]
    [SerializeField] private float volumenSalto = 0.7f;
    [Tooltip("El volumen para el ataque.")]
    [SerializeField] private float volumenAtaque = 0.8f;
    // =========================================================

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [SerializeField] private float suavizadoDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto Normal")]
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionCaja;
    [SerializeField] private bool enSuelo;
    // private bool salto = false; // *** MODIFICADO: Ya no usamos esta variable flag para el salto ***

    // =========================================================
    // *** SECCIÓN EXISTENTE: DOBLE SALTO Y CAPA ***
    // =========================================================
    [Header("Mejora: Capa de Doble Salto")]
    [Tooltip("Fuerza del segundo salto (suele ser igual o un poco menor al normal).")]
    [SerializeField] private float fuerzaDobleSalto;
    private bool tieneHabilidadDobleSalto = false;
    private bool puedeHacerDobleSalto;
    // =========================================================

    [Header("Muerte por Altura (Cronómetro)")]
    [Tooltip("Tiempo máximo en segundos que puede estar cayendo antes de que el impacto sea mortal. Prueba con 1.0 o 1.5")]
    [SerializeField] private float tiempoLimiteCaida = 1.2f;
    private float cronometroCaida = 0f;

    [Header("Respawn")]
    [Tooltip("Arrastra aquí el objeto vacío que marca dónde reaparece el jugador al morir.")]
    [SerializeField] private Transform puntoDeRespawn;

    [Header("Sistema de Antorcha")]
    [Tooltip("(Opcional) Arrastra aquí un objeto hijo de tu personaje que tenga una Light 2D para encenderla de verdad.")]
    [SerializeField] private GameObject luzAntorchaObjeto;
    private bool dentroDeCueva = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // *** NUEVO: Obtener AudioSource que [RequireComponent] asegura que exista ***
        playerAudioSource = GetComponent<AudioSource>();
        playerAudioSource.playOnAwake = false; // Asegurar que no suene al inicio
        // *** FIN NUEVO ***

        // Inicialización de estados del Animator
        animator.SetBool("TieneAntorcha", false);
        animator.SetBool("TieneCapa", false); // Aseguramos que empiece sin capa visualmente

        if (luzAntorchaObjeto != null) luzAntorchaObjeto.SetActive(false);

        if (puntoDeRespawn == null)
        {
            GameObject respawnTemporal = new GameObject("RespawnTemporal_Auto");
            respawnTemporal.transform.position = transform.position;
            puntoDeRespawn = respawnTemporal.transform;
        }

        // Asegurar que las partículas empiecen apagadas
        if (ParticulasCorrer != null && ParticulasCorrer.isPlaying)
        {
            ParticulasCorrer.Stop();
        }
    }

    private void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        // =========================================================
        // LÓGICA DE SALTO (Normal + Doble)
        // =========================================================
        if (Input.GetButtonDown("Jump"))
        {
            bool haSaltado = false; // Flag para saber si se realizó un salto en este frame

            // CASO 1: Salto normal desde el suelo
            if (enSuelo)
            {
                Saltar(fuerzaDeSalto);
                haSaltado = true;
            }
            // CASO 2: Doble salto en el aire
            // Si NO estamos en el suelo, PERO tenemos la habilidad Y nos queda el doble salto disponible...
            else if (tieneHabilidadDobleSalto && puedeHacerDobleSalto)
            {
                Saltar(fuerzaDobleSalto);
                puedeHacerDobleSalto = false; // Gastamos el doble salto hasta volver a tocar suelo
                haSaltado = true;
            }

            // *** NUEVO: Reproducir sonido de salto si se realizó un salto ***
            if (haSaltado && playerAudioSource != null && sonidoSalto != null)
            {
                playerAudioSource.PlayOneShot(sonidoSalto, volumenSalto);
            }
            // *** FIN NUEVO ***
        }

        // *** NUEVO: Ejemplo de detección de input para "Atacar" y reproducir sonido ***
        // Asume que tienes una animación o lógica de ataque asociada a una tecla/botón
        if (Input.GetButtonDown("Fire1")) // Puedes cambiar "Fire1" por la tecla que uses para atacar
        {
            // Aquí iría tu lógica de ataque, por ejemplo:
            // animator.SetTrigger("Atacar"); 

            if (playerAudioSource != null && sonidoAtaque != null)
            {
                playerAudioSource.PlayOneShot(sonidoAtaque, volumenAtaque);
            }
        }
        // *** FIN NUEVO ***
    }

    private void FixedUpdate()
    {
        bool estabaEnSuelo = enSuelo;
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionCaja, 0f, queEsSuelo);

        // Si tocamos el suelo, recargamos la capacidad de hacer doble salto
        if (enSuelo)
        {
            puedeHacerDobleSalto = true;
        }

        // Lógica del Cronómetro de Caída
        if (rb2D.velocity.y < -0.1f && !enSuelo) cronometroCaida += Time.fixedDeltaTime;
        else if (rb2D.velocity.y > 0.1f || enSuelo) cronometroCaida = 0f;

        if (enSuelo && !estabaEnSuelo && cronometroCaida > tiempoLimiteCaida)
        {
            Debug.Log("¡Muerte por tiempo de caída!");
            RespawnJugador(); // <<-- Llama a tu función de RespawnJugador original
            cronometroCaida = 0f;
        }

        animator.SetBool("enSuelo", enSuelo);
        Mover(movimientoHorizontal * Time.fixedDeltaTime);
    }

    void CrearParticulasCorrer()
    {
        ParticulasCorrer.Play();
    }

    private void Saltar(float fuerza)
    {
        enSuelo = false;
        // IMPORTANTE: Reseteamos la velocidad vertical a 0 antes de aplicar la fuerza.
        // Esto hace que el segundo salto siempre tenga la misma potencia, tanto si estás subiendo como cayendo.
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        rb2D.AddForce(new Vector2(0f, fuerza));
    }

    private void Mover(float mover)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        // Lógica de partículas
        bool deberiaCorrer = enSuelo && Mathf.Abs(movimientoHorizontal) > 0.1f;

        if (deberiaCorrer)
        {
            if (!ParticulasCorrer.isPlaying)
            {
                CrearParticulasCorrer();
            }
            // *** NUEVO: Lógica para reproducir sonido de pasos ***
            if (playerAudioSource != null && sonidoPasos != null && !playerAudioSource.isPlaying)
            {
                // Si el AudioSource no está reproduciendo ya un sonido (evita superposiciones rápidas de pasos)
                playerAudioSource.PlayOneShot(sonidoPasos, volumenPasos);
            }
            // *** FIN NUEVO ***
        }
        else
        {
            if (ParticulasCorrer.isPlaying)
            {
                ParticulasCorrer.Stop();
            }
            // *** NUEVO: Opcional: Si el sonido de pasos es un loop constante, aquí lo detendrías ***
            // Con PlayOneShot, no es necesario detenerlo explícitamente ya que suena una vez.
            // if (playerAudioSource != null && playerAudioSource.isPlaying && playerAudioSource.clip == sonidoPasos)
            // {
            //     playerAudioSource.Stop();
            // }
            // *** FIN NUEVO ***
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void ActivarHabilidadCapa()
    {
        tieneHabilidadDobleSalto = true;
        animator.SetBool("TieneCapa", true);
        animator.SetBool("TieneAntorcha", false);
        Debug.Log("¡Habilidad de Capa y Doble Salto obtenida!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trampa"))
        {
            RespawnJugador(); // <<-- Llama a tu función de RespawnJugador original
        }
        else if (collision.CompareTag("AreaCueva"))
        {
            CambiarEstadoAntorcha(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AreaCueva"))
        {
            CambiarEstadoAntorcha(false);
        }
    }

    // Lógica de la antorcha para respetar la capa
    void CambiarEstadoAntorcha(bool estado)
    {
        dentroDeCueva = estado;

        // Solo cambiamos el parámetro visual de la antorcha si NO tenemos la capa.
        // Si tenemos capa, la capa manda visualmente en el Animator.
        if (!tieneHabilidadDobleSalto)
        {
            animator.SetBool("TieneAntorcha", estado);
        }

        // La luz real (el objeto Light2D) siempre obedece a si estamos en cueva o no
        if (luzAntorchaObjeto != null)
        {
            luzAntorchaObjeto.SetActive(estado);
        }
    }

    public void RespawnJugador()
    {
        Debug.Log("Respawn del jugador ejecutado.");

        rb2D.velocity = Vector2.zero;
        velocidad = Vector3.zero;
        cronometroCaida = 0f;
        transform.position = puntoDeRespawn.position;

        if (!dentroDeCueva) CambiarEstadoAntorcha(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionCaja);
    }
}