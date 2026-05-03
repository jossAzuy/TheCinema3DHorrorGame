using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    public InteractionType CurrentType { get; private set; }

    public static PlayerInteractionDetector Instance;

    public bool debugMode = false;
    public LayerMask interactMask;
    public LayerMask snapMask;
    public float interactDistance = 3f;
    public float sphereCastRadius = 0.5f;

    private Transform cam;

    public bool HasHit { get; private set; }
    public RaycastHit CurrentHit { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        CheckInteractable();
    }

    void CheckInteractable()
    {
        Ray ray = new Ray(cam.position, cam.forward);

        RaycastHit hit;
        HasHit = false;

        CurrentType = InteractionType.None;

        // PRIORIDAD 1: INTERACT
        if (Physics.SphereCast(ray, sphereCastRadius, out hit, interactDistance))
        {
            if (hit.collider.GetComponent<Door>() != null)
            {
                HasHit = true;
                CurrentHit = hit;
                CurrentType = InteractionType.Interact;
            }
        }

        // PRIORIDAD 2: PICKUP
        /*   if (!HasHit && Physics.SphereCast(ray, sphereCastRadius, out hit, interactDistance, interactMask))
          {
              HasHit = true;
              CurrentHit = hit;
              CurrentType = InteractionType.Pickup;
          }

          // PRIORIDAD 3: SNAP
          else if (!HasHit && Physics.Raycast(ray, out hit, interactDistance, snapMask))
          {
              SnapPoint snap = hit.collider.GetComponent<SnapPoint>();

              if (snap != null && !snap.isOccupied)
              {
                  HasHit = true;
                  CurrentHit = hit;
                  CurrentType = InteractionType.Snap;
              }
          } */

        // UI SOLO PARA INTERACT
        // UIDynamicInteractIcon.Instance.SetAvailable(CurrentType == InteractionType.Interact);
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