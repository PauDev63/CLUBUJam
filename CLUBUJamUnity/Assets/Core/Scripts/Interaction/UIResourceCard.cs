using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResourceCard : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    private int quantity;

    public void Initialize(Sprite sprite, int startQuantity)
    {
        icon.sprite = sprite;
        UpdateQuantity(startQuantity);
    }

    public void UpdateQuantity(int newQuantity)
    {
        if(newQuantity >= 0)
        {
            quantity = newQuantity;
            quantityText.text = newQuantity.ToString();
        }
        else
        {
            quantity = 0;
            quantityText.text = "Worker";    
        }
    }
    

    public void UpdateSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void EmptyCard()
    {
        icon.sprite = null;
        quantityText.text = "0";
    }

}
