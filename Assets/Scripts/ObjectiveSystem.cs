using UnityEngine;
public class ObjectiveSystem : MonoBehaviour
{
    // Script que dispara los eventos relacionados con los objetivos. 
    // Por ejemplo, al completar el primer objetivo, se llama a CompleteFirstObjective() 
    // para disparar el evento OnFirstObjectiveCompleted y mostrar un mensaje.
    public void CompleteFirstObjective()
    {
        GameEventsManager.OnFirstObjectiveCompleted?.Invoke();
        // GameEventsManager.OnShowMessage?.Invoke("Encuentra la salida...");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CompleteFirstObjective();
        }
    }
}