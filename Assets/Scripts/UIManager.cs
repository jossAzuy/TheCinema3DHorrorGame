/* using UnityEngine;

public class UIManager : MonoBehaviour
{
    void OnEnable()
    {
        GameEventsManager.OnShowMessage += ShowMessage;
    }

    void OnDisable()
    {
        GameEventsManager.OnShowMessage -= ShowMessage;
    }

    void ShowMessage(string text)
    {
        Debug.Log(text);
        // Mostrar en pantalla
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameEventsManager.OnShowMessage?.Invoke("Mensaje de prueba");
        }
    }
} */