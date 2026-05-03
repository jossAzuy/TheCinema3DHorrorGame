using UnityEngine;
using UnityEngine.UI;

public class PlayerObjectPickup : MonoBehaviour
{
    [Header("Settings")]
    public float interactDistance = 3f;
    public float interactSnapDistance = 3f;
    public float sphereRaycastRadius = 0.5f;
    public float extraDistanceDownwards = 2f;
    public float dropImpulseForce = 2f;
    public LayerMask interactMask;
    public LayerMask snapMask;
    public KeyCode interactKey = KeyCode.E;
    public Transform holdPoint;

    private GameObject heldObject;
    private Rigidbody heldRb;

    private Transform cam;
    private SnapPoint currentSnap;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey) || Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                TryPickObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    void TryPickObject()
    {
        var detector = PlayerInteractionDetector.Instance;

        // Solo permitir pickup si el detector detecta un objeto con tipo Pickup
        if (!detector.HasHit || detector.CurrentType != InteractionType.Pickup)
            return;

        var hit = detector.CurrentHit;

        heldObject = hit.collider.gameObject;
        heldRb = heldObject.GetComponent<Rigidbody>();

        // Si ya estamos sosteniendo algo, liberarlo antes de agarrar el nuevo
        if (currentSnap != null)
        {
            currentSnap.Release();
            currentSnap = null;
        }

        if (heldRb != null)
        {
            heldRb.isKinematic = true;
        }

        // Aplicar offset de posición/rotación si el objeto lo tiene
        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    void DropObject()
    {
        var detector = PlayerInteractionDetector.Instance;

        if (detector.HasHit && detector.CurrentType == InteractionType.Snap)
        {
            SnapPoint snap = detector.CurrentHit.collider.GetComponent<SnapPoint>();

            if (snap != null)
            {
                heldRb.isKinematic = false;

                snap.SnapObject(heldObject);
                currentSnap = snap;

                heldObject = null;
                heldRb = null;
                return;
            }
        }

        // Drop normal
        heldObject.transform.SetParent(null);

        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
            heldRb.linearVelocity = cam.forward * dropImpulseForce;
        }

        heldObject = null;
        heldRb = null;
    }

    /* void TryPickObject()
    {
        RaycastHit hit;

        if (GetDynamicSphereCast(out hit, interactMask))
        {
            heldObject = hit.collider.gameObject;
            heldRb = heldObject.GetComponent<Rigidbody>();

            // Si ya estamos sosteniendo algo, liberarlo antes de agarrar el nuevo
            if (currentSnap != null)
            {
                currentSnap.Release();
                currentSnap = null;
            }

            if (heldRb != null)
            {
                // heldRb.useGravity = false;
                heldRb.isKinematic = true;

                // heldRb.linearVelocity = Vector3.zero;
                //  heldRb.angularVelocity = Vector3.zero;
            }

            heldObject.transform.SetParent(holdPoint);

            if (heldObject.TryGetComponent(out HoldPointOffset offset))
            {
                Debug.Log("Applying HoldPointOffset for " + heldObject.name);
                heldRb.isKinematic = true;

                heldObject.transform.localPosition = offset.localPositionOffset;
                heldObject.transform.localRotation = Quaternion.Euler(offset.localRotationOffset);
                // Alternativamente, si quieres usar el metodo de extension:
                // heldObject.transform.SetLocalPositionAndRotation(offset.localPositionOffset, Quaternion.Euler(offset.localRotationOffset));

            }
            else
            {
                Debug.Log("No HoldPointOffset found on " + heldObject.name + ", using default position/rotation.");

                heldObject.transform.SetLocalPositionAndRotation(
                    Vector3.zero,
                    Quaternion.identity
                    );
            }

        }
    } */

    /* void DropObject()
    {
        // OLD VERSION

        Ray ray = new(cam.position, cam.forward);

        // Intentar colocar en SnapPoint
        if (Physics.Raycast(ray, out RaycastHit hit, interactSnapDistance, snapMask))
        {
            SnapPoint snap = hit.collider.GetComponent<SnapPoint>();

            if (snap != null && !snap.isOccupied)
            {
                heldRb.isKinematic = false;

                snap.SnapObject(heldObject);

                currentSnap = snap; // guardar referencia

                heldObject = null;
                heldRb = null;

                return;
            }
        }

        // Si no hay snap, soltar normal
        heldObject.transform.SetParent(null);

        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
            heldRb.linearVelocity = cam.forward * dropImpulseForce; // Lanzar ligeramente hacia adelantexx
        }

        heldObject = null;
        heldRb = null;
    } */

    bool GetDynamicSphereCast(out RaycastHit hit, LayerMask mask)
    {
        Ray ray = new Ray(cam.position, cam.forward);

        float dynamicDistance = interactDistance;
        float downwardFactor = Mathf.Clamp01(-cam.forward.y);
        dynamicDistance += downwardFactor * extraDistanceDownwards;

        return Physics.SphereCast(ray, sphereRaycastRadius, out hit, dynamicDistance, mask);
    }


    void OnDrawGizmos()
    {
        if (cam == null) return;

        Ray ray = new Ray(cam.position, cam.forward);

        float dynamicDistance = interactDistance;
        float downwardFactor = Mathf.Clamp01(-cam.forward.y);
        dynamicDistance += downwardFactor * extraDistanceDownwards;

        Vector3 end = ray.origin + ray.direction * dynamicDistance;

        // Detectar hit aquí también (solo para debug)
        if (Physics.SphereCast(ray, sphereRaycastRadius, out RaycastHit hit, dynamicDistance, interactMask))
        {
            Gizmos.color = hit.collider.CompareTag("Interactable") ? Color.green : Color.yellow;

            Gizmos.DrawLine(ray.origin, hit.point);
            Gizmos.DrawWireSphere(hit.point, sphereRaycastRadius);
        }
        else
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(ray.origin, end);
            Gizmos.DrawWireSphere(end, sphereRaycastRadius);
        }

        // --- DEBUG SNAP ---
        Ray snapRay = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(snapRay, out RaycastHit snapHit, interactSnapDistance, snapMask))
        {
            SnapPoint snap = snapHit.collider.GetComponent<SnapPoint>();

            if (snap != null)
            {
                if (!snap.isOccupied)
                {
                    Gizmos.color = Color.purple; // disponible
                }
                else
                {
                    Gizmos.color = Color.pink; // ocupado
                }

                // Línea hasta el impacto
                Gizmos.DrawLine(cam.position, snapHit.point);

                // Punto de impacto (pequeño)
                Gizmos.DrawSphere(snapHit.point, 0.05f);

                // Normal del impacto (MUY útil)
                Gizmos.DrawRay(snapHit.point, snapHit.normal * 0.3f);
            }
        }
        else
        {
            Gizmos.color = Color.gray;

            Gizmos.DrawLine(snapRay.origin, snapRay.origin + snapRay.direction * interactSnapDistance);
        }
    }

}