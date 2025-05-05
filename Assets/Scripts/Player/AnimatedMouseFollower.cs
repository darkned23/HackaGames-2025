using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatedMouseFollower : MonoBehaviour
{
    private float initialY;
    [SerializeField] private Vector3 pivotOffset; // Offset para ajustar el punto de seguimiento

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        Plane plane = new Plane(Vector3.up, new Vector3(0, initialY, 0)); // Plano horizontal en Y=initialY

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 worldPos = ray.GetPoint(enter);
            transform.position = new Vector3(worldPos.x, initialY, worldPos.z) + pivotOffset;
        }
    }
}
