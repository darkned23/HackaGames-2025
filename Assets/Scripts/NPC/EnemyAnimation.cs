using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Vector2 previousPosition;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        previousPosition = transform.position; 
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;

        Vector2 velocity = (currentPosition - previousPosition) / Time.deltaTime;

        previousPosition = currentPosition;

        animator.SetFloat("Speed", velocity.magnitude); 
    }
}

