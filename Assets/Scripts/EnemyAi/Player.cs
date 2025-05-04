using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool isFacingRight = true;
    private Vector2 input;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        input = new Vector2 (xInput, yInput).normalized;
        Flip();
        animator.SetFloat("Speed", input.magnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
    }

    private void Flip()
    {

        if((input.x < 0 && isFacingRight) || (input.x > 0 && !isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}

