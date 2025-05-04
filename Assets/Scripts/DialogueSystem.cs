using TMPro;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText; // Referencia al texto en pantalla
    [SerializeField] private GameObject dialoguePanel; // Panel que contiene el diálogo
    [SerializeField] private string[] dialogueLines; // Líneas de diálogo
    [SerializeField] private float typingSpeed = 0.05f; // Velocidad de escritura
    [SerializeField] private GameObject interactionPrompt; // Referencia al prompt de interacción
    [SerializeField] private PlayerInput playerInput;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool canContinue = false;
    private bool playerInRange = false;
    private bool dialogueStarted = false;
    private Action<InputAction.CallbackContext> interactActionDelegate;

    private void Awake()
    {
        interactActionDelegate = Interact;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInput.actions["Interact"].performed += interactActionDelegate;

            playerInRange = true;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInput.actions["Interact"].performed -= interactActionDelegate;

            playerInRange = false;
            dialogueStarted = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
            EndDialogue();
        }
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;
        StartCoroutine(TypeLine());
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        Interact();
    }

    private void Interact()
    {
        Debug.Log("Interact pressed");

        if (!playerInRange) return;

        if (!dialogueStarted)
        {
            dialogueStarted = true;
            StartDialogue();
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
        else if (isTyping)
        {
            // Si el texto está escribiéndose, muestra la línea completa
            StopAllCoroutines();
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
            canContinue = true;
        }
        else if (canContinue)
        {
            // Avanza al siguiente diálogo
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                StartCoroutine(TypeLine());
            }
            else
            {
                EndDialogue();
                dialogueStarted = false;
                if (interactionPrompt != null)
                    interactionPrompt.SetActive(true);
            }
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        canContinue = false;
        dialogueText.text = "";

        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        canContinue = true;
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueStarted = false;
        currentLineIndex = 0;
    }
}