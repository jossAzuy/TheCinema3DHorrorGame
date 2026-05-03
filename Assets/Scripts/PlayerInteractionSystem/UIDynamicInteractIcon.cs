using UnityEngine;
using UnityEngine.UI;

public class UIDynamicInteractIcon : MonoBehaviour
{
    public static UIDynamicInteractIcon Instance;

    [Header("References")]
    [SerializeField] private Image interactImage;

    [Header("Alpha Settings")]
    [SerializeField] private float normalAlpha = 0.2f;   // sin interacción
    [SerializeField] private float activeAlpha = 1f;     // con interacción
    [SerializeField] private float fadeSpeed = 5f;       // qué tan rápido hace el cambio

    private float targetAlpha;

    private void Awake()
    {
        Instance = this;

        targetAlpha = normalAlpha;
        SetAlphaInstant(normalAlpha);
    }

    private void Update()
    {
        // Fade suave hacia el target
        float currentAlpha = interactImage.color.a;
        float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

        SetAlpha(newAlpha);
    }

    public void SetAvailable(bool available)
    {
        targetAlpha = available ? activeAlpha : normalAlpha;
    }

    private void SetAlpha(float alpha)
    {
        Color c = interactImage.color;
        c.a = alpha;
        interactImage.color = c;
    }

    private void SetAlphaInstant(float alpha)
    {
        Color c = interactImage.color;
        c.a = alpha;
        interactImage.color = c;
    }
}