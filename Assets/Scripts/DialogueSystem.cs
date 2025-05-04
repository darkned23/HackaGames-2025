using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText; // Referencia al texto en pantalla
    [SerializeField] private GameObject dialoguePanel; // Panel que contiene el diálogo
    [SerializeField] private string[] dialogueLines; // Líneas de diálogo
    [SerializeField] private float typingSpeed = 0.05f; // Velocidad de escritura

    [SerializeField] private PlayerInput playerInput;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool canContinue = false;

    private void Start()
    {
        // Vincula la acción "Interact" manualmente
        playerInput.actions["Interact"].performed += ctx => OnInteract();
        dialoguePanel.SetActive(false); // Asegúrate de que el panel esté desactivado al inicio
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;
        StartCoroutine(TypeLine());
    }

    private void OnInteract()
    {
        if (dialoguePanel.activeSelf)
        {
            if (isTyping)
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
                }
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
    }
}