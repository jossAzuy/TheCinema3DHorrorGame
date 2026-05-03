using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] lines;
}
