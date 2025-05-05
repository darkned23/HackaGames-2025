using UnityEngine;

public class LookAI : MonoBehaviour
{
    [SerializeField] private Transform player;

    private bool isFacingRight = true;

    void Update()
    {
        bool isPlayerRight = transform.position.x < player.transform.position.x;
        Flip(isPlayerRight);
    }

    private void Flip(bool isPlayerRight)
    {
        if ((isFacingRight && !isPlayerRight) || (!isFacingRight && isPlayerRight))
        {
            isFacingRight = !isFacingRight;

            float yRotation = isFacingRight ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}
