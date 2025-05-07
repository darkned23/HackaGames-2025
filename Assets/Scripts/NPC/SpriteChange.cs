using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    [SerializeField] private Sprite newSprite; // El nuevo sprite que se asignará
    [SerializeField] private SpriteRenderer spriteRenderer; // Referencia al componente SpriteRenderer

    public void ChangeSprite()
    {
        if (spriteRenderer != null && newSprite != null)
        {
            // Cambiar el sprite del objeto
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer o nuevo sprite no asignados.");
        }
    }
}