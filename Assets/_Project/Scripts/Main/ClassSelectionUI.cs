using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassSelectionUI : MonoBehaviour
{
    public static ClassSelectionUI Instance;

    public TMP_Text statusText;
    private string selectedClass;

    void Awake() => Instance = this;

    public void SelectClass(string classId)
    {
        selectedClass = classId;
        PlayerPrefs.SetString("SelectedClass", classId);
        statusText.text = $"Classe selecionada: {classId.ToUpper()}";
    }

    public void OnConfirm()
    {
        if (string.IsNullOrEmpty(selectedClass))
            return;
        SceneManager.LoadScene("Game"); // nome da cena de jogo
    }
}
