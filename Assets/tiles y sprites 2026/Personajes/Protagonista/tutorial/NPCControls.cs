using UnityEngine;
using TMPro; // Necesario si usas TextMeshPro

public class NPCControls : MonoBehaviour
{
    [Header("UI del Cuadro de Diálogo")]
    public GameObject dialoguePanel;      // El Panel contenedor del texto
    public TextMeshProUGUI dialogueText;  // El texto de TextMeshPro
    public GameObject indicatorIcon;      // Icono flotante (ej. tecla "E" sobre la cabeza del NPC)

    [Header("Mensaje del NPC")]
    [TextArea(3, 5)]
    public string controlsMessage = "¡Hola viajero!\n\n- Usa A / D o Flechas para moverte.\n- Presiona ESPACIO para saltar.\n- Presiona E para interactuar.";

    private bool isPlayerNearby = false;
    private bool isDialogueActive = false;

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (indicatorIcon != null) indicatorIcon.SetActive(false);
    }

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla 'E'
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDialogue();
        }
    }

    void ToggleDialogue()
    {
        isDialogueActive = !isDialogueActive;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(isDialogueActive);
            if (dialogueText != null)
            {
                dialogueText.text = controlsMessage;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (indicatorIcon != null) indicatorIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            isDialogueActive = false;
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (indicatorIcon != null) indicatorIcon.SetActive(false);
        }
    }
}