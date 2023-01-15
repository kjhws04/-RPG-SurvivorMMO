using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuickSlot : MonoBehaviour
{
    [SerializeField]
    public UI_Slot[] _quickSlots;
    [SerializeField]
    public Image[] _quickImage;

    [SerializeField]
    public Define.SlotType _slotType = Define.SlotType.Normal;

    private void Start()
    {
        _quickSlots = GetComponentsInChildren<UI_Slot>();

        for (int i = 0; i < _quickSlots.Length; i++)
        {
            _quickSlots[i]._slotType = Define.SlotType.Quick;
        }

        _quickSlots[0]._quickSlotType = Define.Item.Equipment;
        _quickSlots[1]._quickSlotType = Define.Item.Armor;
        _quickSlots[2]._quickSlotType = Define.Item.Shoes;
        _quickSlots[3]._quickSlotType = Define.Item.Potion;
    }

    private void Update()
    {
        if (_quickSlots[0]._item == null)
            ChangeColor(false, _quickImage[0]);
        else
            ChangeColor(true, _quickImage[0]);

        if (_quickSlots[1]._item == null)
            ChangeColor(false, _quickImage[1]);
        else
            ChangeColor(true, _quickImage[1]);

        if (_quickSlots[2]._item == null)
            ChangeColor(false, _quickImage[2]);
        else
            ChangeColor(true, _quickImage[2]);

        if (_quickSlots[3]._item == null)
            ChangeColor(false, _quickImage[3]);
        else
            ChangeColor(true, _quickImage[3]);
    }

    IEnumerator CheckQuickCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

    }

    private void ChangeColor(bool _black,Image _image) //이미지의 색을 조절
    {
        if (_black)
            _image.color = Color.black;
        else
            _image.color = Color.gray;
    }
}
