using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public Transform holdPoint; // Punto frente a la cámara donde se sostiene el objeto
    public KeyCode interactKey = KeyCode.E;

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
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
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                heldObject = hit.collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();

                if (heldRb != null)
                {
                    heldRb.useGravity = false;
                    heldRb.linearVelocity = Vector3.zero;
                    heldRb.angularVelocity = Vector3.zero;
                }

                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void DropObject()
    {
        heldObject.transform.SetParent(null);

        if (heldRb != null)
        {
            heldRb.useGravity = true;
        }

        heldObject = null;
        heldRb = null;
    }
}