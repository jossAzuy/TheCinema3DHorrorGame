using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public Transform snapTransform; // punto exacto donde se coloca
    public bool isOccupied = false;

    public void SnapObject(GameObject obj)
    {
        obj.transform.SetParent(snapTransform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        isOccupied = true;
    }
}