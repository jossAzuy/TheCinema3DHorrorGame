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
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(ejeRotacion * grados);
    }

    void Update()
    {
        if (rotando)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);

            transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionObjetivo, t);

            if (t >= 1f)
            {
                rotando = false;
            }
        }
    }

    public void Interact()
    {
        if (rotando) return; // Evitar iniciar una nueva rotacion mientras se esta rotando

        tiempo = 0f;
        rotacionInicial = transform.rotation;

        // Alternar estado
        if (abierta)
        {
            rotacionObjetivo = rotacionCerrada;
        }
        else
        {
            rotacionObjetivo = rotacionAbierta;
        }

        abierta = !abierta;
        rotando = true;
    }
}