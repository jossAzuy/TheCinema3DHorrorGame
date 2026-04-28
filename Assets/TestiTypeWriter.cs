using UnityEngine;
using Febucci.TextAnimatorForUnity;
using Febucci.TextAnimatorForUnity.TextMeshPro; // <- import Text Animator's namespace
using TMPro;

public class TestiTypeWriter : MonoBehaviour
{
    [SerializeField] private TextAnimator_TMP textAnimator;
    [SerializeField] private TypewriterComponent typewriter;
    // [SerializeField] private string textValue;

    private void Start()
    {
        // Do: set text through Text Animator directly
        textAnimator.SetText("Me he quedado dormido!");

        // textAnimator.AppendText(" world!");

        // typewriter.ShowText(textAnimator.textFull);
        // typewriter.StartShowingText(textAnimator);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            // typewriter.ShowText(textValue);
            typewriter.StartShowingText(textAnimator);

            typewriter.StopShowingText();
            typewriter.SkipTypewriter();

            typewriter.StartDisappearingText();
            typewriter.StopDisappearingText();

            // typewriter.onTextShowed.AddListener(() => Debug.Log("Text showed!"));
        }
    }
}
