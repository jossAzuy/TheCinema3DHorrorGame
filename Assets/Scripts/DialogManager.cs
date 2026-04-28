using UnityEngine;
using Febucci.TextAnimatorForUnity;
using Febucci.TextAnimatorForUnity.TextMeshPro;

public class DialogManager : MonoBehaviour
{
    // REFERENCES
    [SerializeField] private TextAnimator_TMP textAnimator;
    [SerializeField] private TypewriterComponent typewriter;
    public DialogData dialogData;


    void OnEnable()
    {
        GameEventsManager.OnShowDialogue += ShowDialogue;
    }

    void OnDisable()
    {
        GameEventsManager.OnShowDialogue -= ShowDialogue;
    }

    void ShowDialogue(DialogData dialog)
    {
        // textAnimator.SetText(dialog.lines[0]);
        textAnimator.SetText(dialog.lines[1]);

        typewriter.StartShowingText(textAnimator);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameEventsManager.OnShowDialogue?.Invoke(dialogData);
        }
    }
}