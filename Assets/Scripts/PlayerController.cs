using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    Vector2 moveInput;

    public float jumpForce;
    [SerializeField] bool jumpInput = false;

    public Transform grdChecker;
    public LayerMask Ground;
    public float rayLength;
    [SerializeField] bool grounded;

    [SerializeField] bool backTurned;

    public bool flipped;
    public float flipSpeed;

    Quaternion flipLeft = Quaternion.Euler(0, -180, 0);
    Quaternion flipRight = Quaternion.Euler(0, 0, 0);

    Rigidbody theRB;
    Animator theAnim;

    private PlayerInput playerInput;

    private void Start()
    {
        theRB = GetComponent<Rigidbody>();
        theAnim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        // Vincula las acciones manualmente
        playerInput.actions["Move"].performed += ctx => Move(ctx);
        playerInput.actions["Move"].canceled += ctx => Move(ctx);
        playerInput.actions["Jump"].performed += ctx => Jump(ctx);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            jumpInput = true;
        }
    }

    private void Update()
    {
        theAnim.SetFloat("MoveSpeed", theRB.linearVelocity.magnitude);

        // Manejo de rotación para voltear al personaje
        if (!flipped && moveInput.x < 0)
        {
            flipped = true;
        }
        else if (flipped && moveInput.x > 0)
        {
            flipped = false;
        }

        if (flipped) transform.rotation = Quaternion.Slerp(transform.rotation, flipLeft, flipSpeed * Time.deltaTime);
        else if (!flipped) transform.rotation = Quaternion.Slerp(transform.rotation, flipRight, flipSpeed * Time.deltaTime);

        // Manejo de animación para "BackTurned"
        if (!backTurned && moveInput.y > 0) backTurned = true;
        else if (backTurned && moveInput.y < 0) backTurned = false;

        theAnim.SetBool("BackTurned", backTurned);
        theAnim.SetBool("Grounded", grounded);
    }

    private void FixedUpdate()
    {
        // Movimiento del personaje
        theRB.linearVelocity = new Vector3(moveInput.x * moveSpeed, theRB.linearVelocity.y, moveInput.y * moveSpeed);

        // Verifica si el personaje está en el suelo
        RaycastHit hit;
        if (Physics.Raycast(grdChecker.position, Vector3.down, out hit, rayLength, Ground)) grounded = true;
        else grounded = false;

        Debug.DrawRay(grdChecker.position, Vector3.down * rayLength, Color.red);

        // Realiza el salto si se detectó la entrada
        if (jumpInput) Jump();
    }

    void Jump()
    {
        // Aplica la fuerza de salto
        theRB.linearVelocity = new Vector3(theRB.linearVelocity.x, jumpForce, theRB.linearVelocity.z);
        jumpInput = false;
    }
}