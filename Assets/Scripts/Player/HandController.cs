using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private int pointsPerEnemy = 10;
    private int score = 0;

    public int Score => score; // Propiedad pública para acceder a la puntuación

    public delegate void ScoreChanged(int newScore);
    public event ScoreChanged OnScoreChanged; // Evento para notificar cambios

    private PlayerInput playerInput;

    private void Start()
    {
        Cursor.visible = false;

        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Fire"].performed += ctx => OnFire(ctx);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        // Obtener la posición del mouse en pantalla
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // Crear un rayo desde la cámara hacia el punto del mouse
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        // Lanzar el raycast en 3D
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy: " + hit.collider.name);

                // Cambiar el sprite del objeto hijo que tenga SpriteRenderer
                GameObject enemyGO = hit.collider.gameObject;
                SpriteRenderer spriteRenderer = enemyGO.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    enemyGO.GetComponent<SpriteChange>().ChangeSprite(); // Cambiar el sprite
                }

                // Desactivar el script EnemyMovement
                EnemyMovement enemyMovement = enemyGO.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.enabled = false;
                }

                // Cambiar la posición Y del enemigo a 0.02
                Vector3 pos = enemyGO.transform.position;
                pos.y = 0.02f;
                enemyGO.transform.position = pos;

                score += pointsPerEnemy;
                OnScoreChanged?.Invoke(score); // Notificar cambio de puntuación
            }
            else
            {
                Debug.Log("Hit something else: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("Missed shot");
        }
    }
}