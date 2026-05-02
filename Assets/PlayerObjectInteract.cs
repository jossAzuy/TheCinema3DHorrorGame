using UnityEngine;

public class PlayerObjectInteract : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool debugMode = false;
    public LayerMask interactMask;
    public float interactDistance = 3f;
    public float sphereCastRadius = 0.5f;

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereCastRadius, out hit, interactDistance, interactMask))
        {
            if (hit.collider.TryGetComponent(out Door door))
            {
                door.Interact();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!debugMode) return;
        if (cam == null) return;

        Ray ray = new Ray(cam.position, cam.forward);
        Vector3 origen = ray.origin;
        Vector3 direccion = ray.direction * interactDistance;
        Vector3 final = origen + direccion;

        // Color base
        Gizmos.color = Color.red;

        // Línea del raycast
        Gizmos.DrawLine(origen, final);

        // Esfera al final (representa el radio)
        Gizmos.DrawWireSphere(final, sphereCastRadius);

        // Si hay hit, mostrar punto exacto
        if (Physics.SphereCast(ray, sphereCastRadius, out RaycastHit hit, interactDistance, interactMask))
        {
            Gizmos.color = Color.green;

            // Línea hasta el impacto
            Gizmos.DrawLine(origen, hit.point);

            // Punto de impacto
            Gizmos.DrawSphere(hit.point, 0.05f);

            // Normal de la superficie (MUY útil)
            Gizmos.DrawRay(hit.point, hit.normal * 0.3f);
        }
    }
}