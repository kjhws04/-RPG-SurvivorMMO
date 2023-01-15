using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DragSlot : MonoBehaviour
{
    static public UI_DragSlot _instance;

    public UI_Slot _dragSlot;

    [SerializeField]
    private Image _itemImage;

    public void Start()
    {
        _instance = this;
    }

    public void DragSetImage(Image itemImage)
    {
        _itemImage.sprite = itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = _itemImage.color;
        color.a = _alpha;
        _itemImage.color = color;
    }
}
