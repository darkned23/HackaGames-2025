using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float randomMoveSpeed = 2f;
    public float fleeSpeed = 5f;
    public float detectionRadius = 3f;

    [Header("Rango de Patrullaje")]
    public Vector2 xRange = new Vector2(-5f, 5f);
    public Vector2 zRange = new Vector2(-5f, 5f);

    private Vector3 targetPosition;
    private float initialY;

    void Start()
    {
        initialY = transform.localPosition.y;
        SetRandomTarget();
    }

    void Update()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        float distanceToMouse = Vector3.Distance(transform.position, mouseWorldPos);

        float currentSpeed = (distanceToMouse < detectionRadius) ? fleeSpeed : randomMoveSpeed;
        Patrol(currentSpeed);
    }

    void Patrol(float speed)
    {
        Vector3 target = new Vector3(targetPosition.x, initialY, targetPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            SetRandomTarget();
        }
    }

    void SetRandomTarget()
    {
        float randomX = Random.Range(xRange.x, xRange.y);
        float randomZ = Random.Range(zRange.x, zRange.y);
        targetPosition = new Vector3(randomX, initialY, randomZ);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        Plane plane = new Plane(Vector3.up, Vector3.zero); // Plano horizontal en Y = 0

        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return transform.position; // Fallback
    }
}
