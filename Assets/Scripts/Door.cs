using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 ejeRotacion = Vector3.up;
    public float grados = 90f;
    public float duracion = 1f;
    public bool invertirLado = false;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    private Quaternion rotacionInicial;
    private Quaternion rotacionObjetivo;

    private float tiempo = 0f;
    private bool rotando = false;
    private bool abierta = false;


    void Start()
    {
        if (invertirLado)
        {
            grados = -grados;
        }

        rotacionCerrada = transform.rotation;
        // rotacionAbierta = rotacionCerrada * Quaternion.Euler(ejeRotacion * grados);
        rotacionAbierta = rotacionCerrada * Quaternion.AngleAxis(grados, ejeRotacion.normalized);
    }

    void Update()
    {
        if (rotando)
        {
            tiempo += Time.deltaTime;
            // float t = Mathf.Clamp01(tiempo / duracion);
            float t = Mathf.Clamp01(tiempo / duracion);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionObjetivo, t);

            if (t >= 1f)
            {
                rotando = false;
            }
        }
    }

    public void Interact()
    {
        rotacionInicial = transform.rotation;
        rotacionObjetivo = abierta ? rotacionCerrada : rotacionAbierta;

        abierta = !abierta;
        tiempo = 0f;
        rotando = true;
    }
}