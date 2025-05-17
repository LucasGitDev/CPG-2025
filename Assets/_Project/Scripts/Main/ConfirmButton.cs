using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    public void ConfirmOnClick()
    {
        ClassSelectionUI.Instance.OnConfirm();
    }
}
