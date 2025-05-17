using UnityEngine;

public class ClassCard : MonoBehaviour
{
    public string classId; // set no Inspector, tipo "sword"

    public void SelectClassOnClick()
    {
        ClassSelectionUI.Instance.SelectClass(classId);
    }
}
