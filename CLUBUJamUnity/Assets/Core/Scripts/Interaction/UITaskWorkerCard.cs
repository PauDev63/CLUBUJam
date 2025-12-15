using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITaskWorkerCard : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _taskText;
    [SerializeField] private Button _dropTaskButton;

    //private int id

    public void Initialize(Sprite sprite, bool activeDropButton)
    {
        _icon.sprite = sprite;      // o le pasamos un Building con el sprite y el texto?
        _icon.gameObject.SetActive(true);
        _taskText.gameObject.SetActive(true);
        _dropTaskButton.gameObject.SetActive(activeDropButton);
    }

    public void UpdateText()
    {
        _taskText.text = "Updated";
    }

    public void ShowTaskCard()
    {
        _icon.gameObject.SetActive(true);
        _taskText.gameObject.SetActive(true);
        _dropTaskButton.gameObject.SetActive(true);
    }

    public void HideTaskCard()
    {
        _icon.gameObject.SetActive(false);
        _taskText.gameObject.SetActive(false);
        _dropTaskButton.gameObject.SetActive(false);
    }

    
}
