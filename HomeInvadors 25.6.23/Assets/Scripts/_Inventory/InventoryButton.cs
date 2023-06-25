using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    // [SerializeField]
    // public TextMeshProUGUI descriptionText;

    [SerializeField]
    public TextMeshProUGUI amountText;
    [SerializeField]
    public RawImage icon;
    public Pickable pickable;
    public RectTransform buttonRectTransform;
    private Vector2 originalSize;

    private void Start()
    {
        originalSize = buttonRectTransform.sizeDelta;
    }
    public void UpdateButtonData(string _itemName, int _amount, Texture _iconTexture, Pickable _pickable)
    {
        nameText.text = _itemName;
        amountText.text = _amount.ToString();
        icon.texture = _iconTexture;
        this.pickable = _pickable;
    }

    public void OnPointerEnter()
    {
        Debug.Log("Mouse is over the button");
        // Expand the button when the mouse hovers over it
        buttonRectTransform.localScale = Vector3.one * 1.2f; // Increase the scale by 20%
    }

    public void OnPointerExit()
    {
        Debug.Log("Mouse is NOT over the button");

        // Restore the button's original scale when the mouse exits
        buttonRectTransform.localScale = Vector3.one;
    }
    public void OnButtonClick()
    {
        if (pickable != null)
        {
            this.pickable.UseItem();
        }
    }
}